unit RicettaDematerializzataHelper;

// Helper Delphi per RicettaDematerializzata.Client
// mantenendo compatibilità di unit name già usato nel progetto.

{
  Helper Delphi per lavorare con la libreria RicettaDematerializzata.
  Fornisce funzioni di utilità per:
  - Costruire stringhe input key=value
  - Analizzare stringhe output key=value
  - Costanti per i codici servizio (enum DigitalPrescriptionService)
  - Wrapper semplificato attorno alla COM interface

  Prerequisito:
    - ricetta_dematerializzata.dll registrata con regasm /tlb
    - ricetta_dematerializzata.tlb importata in Delphi (Project → Import Type Library)
      oppure usare questa unit standalone con CreateOleObject.
}

interface

uses
  SysUtils, Classes, StrUtils, Variants, ComObj, ActiveX;

// ─── Costanti servizi (mirror di DigitalPrescriptionService enum) ─────────────

const
  // Prescrittore
  SRV_VISUALIZZA_PRESCRITTO              = 1;
  SRV_INVIO_PRESCRITTO                   = 2;
  SRV_ANNULLA_PRESCRITTO                 = 3;
  SRV_INTERROGA_NRE_UTILIZZATI           = 4;
  SRV_SERVICE_ANAG_PRESCRITTORE          = 5;
  SRV_INVIO_DICHIARAZIONE_SOSTITUZIONE   = 6;

  // Erogatore
  SRV_INVIO_EROGATO                      = 10;
  SRV_VISUALIZZA_EROGATO                 = 11;
  SRV_SOSPENDI_EROGATO                   = 12;
  SRV_ANNULLA_EROGATO                    = 13;
  SRV_RICERCA_EROGATORE                  = 14;
  SRV_REPORT_EROGATO_MENSILE             = 15;
  SRV_SERVICE_ANAG_EROGATORE             = 16;
  SRV_RICETTA_DIFFERITA                  = 17;
  SRV_ANNULLA_EROGATO_DIFF               = 18;
  SRV_RICEVUTE_SAC                       = 19;

  // A2F
  SRV_CREATE_AUTH                        = 20;
  SRV_REVOKE_AUTH                        = 21;
  SRV_CHECK_TOKEN                        = 22;

  // Ambienti
  AMB_TEST       = 0;
  AMB_PRODUZIONE = 1;

  // ProgID COM default della DLL
  DEFAULT_SANITA_PROGID                  = 'RicettaDematerializzata.Client';

// ─── Funzioni utility key=value ───────────────────────────────────────────────

/// Estrae il valore di una chiave dalla stringa key=value
function KVGet(const Stringa, Chiave: string): string;

/// Aggiunge/imposta una coppia chiave=valore nella stringa
function KVSet(const Stringa, Chiave, Valore: string): string;

/// Costruisce una stringa KV da due array paralleli (chiavi e valori)
function KVBuild(const Chiavi, Valori: array of string): string;

/// Verifica se la risposta contiene un errore
function KVIsErrore(const Output: string): Boolean;

/// Estrae numero errore dalla risposta
function KVGetErroreNumero(const Output: string): string;

/// Estrae descrizione errore dalla risposta
function KVGetErroreDescrizione(const Output: string): string;

/// Imposta una chiave figlia per strutture array/nodi SOAP (formato parent_index_child)
function KVSetArrayNode(const Stringa, ParentKey: string; Index: Integer; const ChildKey, Valore: string): string;

/// Imposta una chiave figlia semplice (formato parent_child)
function KVSetNode(const Stringa, ParentKey, ChildKey, Valore: string): string;

// ─── Wrapper COM ──────────────────────────────────────────────────────────────

type
  // Alias mantenuto per retrocompatibilità.
  TRicettaDematerializzataClient = class
  private
    FClient: Variant;
    FInizializzato: Boolean;
  public
    constructor Create; overload;
    constructor Create(const ProgId: string); overload;
    destructor Destroy; override;

    /// Configura il client (metodo principale).
    /// Ambiente: AMB_TEST (0) o AMB_PRODUZIONE (1).
    /// Seriale: seriale esadecimale del certificato nel Windows Certificate Store.
    ///          Se vuoto, viene usata solo la Basic Auth.
    function Configura(const Username, Password, Seriale: string;
                       Ambiente: Integer = AMB_TEST): Boolean;

    procedure ConfiguraAuthorization2F(const Authorization2F: string);
    function Chiama(Servizio: Integer; const Input: string): string;
    function ChiamaJson(Servizio: Integer; const InputJson: string): string;
    function ChiamaDict(Servizio: Integer; const Input: string): TStringList;
    function OttieniUrl(Servizio: Integer): string;
    function TestConnessione(Servizio: Integer): string;
    function CifraConSanitel(const TestoPiano: string): string;

    property Inizializzato: Boolean read FInizializzato;
  end;

implementation

// ─── Implementazione KV Utils ─────────────────────────────────────────────────

function KVGet(const Stringa, Chiave: string): string;
var
  Parti: TStringList;
  i: Integer;
  ChiaveUpper, Part, K, V: string;
  Sep: Integer;
begin
  Result := '';
  ChiaveUpper := UpperCase(Chiave);
  Parti := TStringList.Create;
  try
    Parti.Delimiter := ';';
    Parti.StrictDelimiter := True;
    Parti.DelimitedText := Stringa;
    for i := 0 to Parti.Count - 1 do
    begin
      Part := Parti[i];
      Sep := Pos('=', Part);
      if Sep > 0 then
      begin
        K := UpperCase(Trim(Copy(Part, 1, Sep - 1)));
        V := Copy(Part, Sep + 1, MaxInt);
        if K = ChiaveUpper then
        begin
          Result := V;
          Exit;
        end;
      end;
    end;
  finally
    Parti.Free;
  end;
end;

function KVSet(const Stringa, Chiave, Valore: string): string;
var
  Parti: TStringList;
  i: Integer;
  ChiaveUpper, Part, K: string;
  Sep: Integer;
  Trovato: Boolean;
begin
  ChiaveUpper := UpperCase(Chiave);
  Trovato := False;
  Parti := TStringList.Create;
  try
    Parti.Delimiter := ';';
    Parti.StrictDelimiter := True;
    Parti.DelimitedText := Stringa;

    for i := 0 to Parti.Count - 1 do
    begin
      Part := Parti[i];
      Sep := Pos('=', Part);
      if Sep > 0 then
      begin
        K := UpperCase(Trim(Copy(Part, 1, Sep - 1)));
        if K = ChiaveUpper then
        begin
          Parti[i] := UpperCase(Chiave) + '=' + Valore;
          Trovato := True;
          Break;
        end;
      end;
    end;

    if not Trovato then
      Parti.Add(UpperCase(Chiave) + '=' + Valore);

    Result := '';
    for i := 0 to Parti.Count - 1 do
    begin
      if (Result <> '') and (Parti[i] <> '') then
        Result := Result + ';';
      if Parti[i] <> '' then
        Result := Result + Parti[i];
    end;
  finally
    Parti.Free;
  end;
end;

function KVBuild(const Chiavi, Valori: array of string): string;
var
  i: Integer;
begin
  Result := '';
  for i := 0 to High(Chiavi) do
  begin
    if Result <> '' then Result := Result + ';';
    if i <= High(Valori) then
      Result := Result + UpperCase(Chiavi[i]) + '=' + Valori[i]
    else
      Result := Result + UpperCase(Chiavi[i]) + '=';
  end;
end;

function KVIsErrore(const Output: string): Boolean;
begin
  Result := KVGet(Output, 'ERRORE_NUMERO') <> '';
end;

function KVGetErroreNumero(const Output: string): string;
begin
  Result := KVGet(Output, 'ERRORE_NUMERO');
end;

function KVGetErroreDescrizione(const Output: string): string;
begin
  Result := KVGet(Output, 'ERRORE_DESCRIZIONE');
end;

function KVSetArrayNode(const Stringa, ParentKey: string; Index: Integer;
  const ChildKey, Valore: string): string;
var
  FullKey: string;
begin
  if Index < 1 then
    raise Exception.Create('Index deve essere >= 1');
  FullKey := Format('%s_%d_%s', [ParentKey, Index, ChildKey]);
  Result := KVSet(Stringa, FullKey, Valore);
end;

function KVSetNode(const Stringa, ParentKey, ChildKey, Valore: string): string;
var
  FullKey: string;
begin
  FullKey := ParentKey + '_' + ChildKey;
  Result := KVSet(Stringa, FullKey, Valore);
end;

// ─── Implementazione TRicettaDematerializzataClient ──────────────────────────

constructor TRicettaDematerializzataClient.Create;
begin
  Create(DEFAULT_SANITA_PROGID);
end;

constructor TRicettaDematerializzataClient.Create(const ProgId: string);
begin
  inherited Create;
  FInizializzato := False;
  try
    FClient := CreateOleObject(ProgId);
  except
    on E: Exception do
      raise Exception.CreateFmt(
        'Impossibile creare %s. Verificare registrazione COM della DLL con regasm /tlb /codebase. Dettaglio: %s',
        [ProgId, E.Message]);
  end;
end;

destructor TRicettaDematerializzataClient.Destroy;
begin
  FClient := Unassigned;
  inherited;
end;

function TRicettaDematerializzataClient.Configura(const Username, Password, Seriale: string;
                                         Ambiente: Integer): Boolean;
begin
  Result := False;
  try
    FClient.Configura(Username, Password, Seriale, Ambiente);
    FInizializzato := True;
    Result := True;
  except
    on E: Exception do
      raise Exception.CreateFmt('Errore configurazione: %s', [E.Message]);
  end;
end;

procedure TRicettaDematerializzataClient.ConfiguraAuthorization2F(const Authorization2F: string);
begin
  FClient.ConfiguraAuthorization2F(Authorization2F);
end;

function TRicettaDematerializzataClient.Chiama(Servizio: Integer; const Input: string): string;
begin
  if not FInizializzato then
    raise Exception.Create('Client non configurato. Chiamare Configura() prima.');
  Result := FClient.Chiama(Servizio, Input);
end;

function TRicettaDematerializzataClient.ChiamaJson(Servizio: Integer; const InputJson: string): string;
begin
  if not FInizializzato then
    raise Exception.Create('Client non configurato. Chiamare Configura() prima.');
  Result := FClient.ChiamaJson(Servizio, InputJson);
end;

function TRicettaDematerializzataClient.ChiamaDict(Servizio: Integer; const Input: string): TStringList;
var
  Output: string;
  Parti: TStringList;
  i, Sep: Integer;
  Part: string;
begin
  Output := Chiama(Servizio, Input);
  Result := TStringList.Create;
  Parti := TStringList.Create;
  try
    Parti.Delimiter := ';';
    Parti.StrictDelimiter := True;
    Parti.DelimitedText := Output;
    for i := 0 to Parti.Count - 1 do
    begin
      Part := Parti[i];
      Sep := Pos('=', Part);
      if Sep > 0 then
        Result.Add(Part);  // già nel formato Chiave=Valore
    end;
  finally
    Parti.Free;
  end;
end;

function TRicettaDematerializzataClient.OttieniUrl(Servizio: Integer): string;
begin
  if not FInizializzato then
    raise Exception.Create('Client non configurato. Chiamare Configura() prima.');
  Result := FClient.OttieniUrl(Servizio);
end;

function TRicettaDematerializzataClient.TestConnessione(Servizio: Integer): string;
begin
  if not FInizializzato then
    raise Exception.Create('Client non configurato. Chiamare Configura() prima.');
  Result := FClient.TestConnessione(Servizio);
end;

function TRicettaDematerializzataClient.CifraConSanitel(const TestoPiano: string): string;
begin
  if not FInizializzato then
    raise Exception.Create('Client non configurato. Chiamare Configura() prima.');
  Result := FClient.CifraConSanitel(TestoPiano);
end;

end.

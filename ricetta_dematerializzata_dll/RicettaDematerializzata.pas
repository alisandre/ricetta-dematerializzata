unit SanitaServiceHelper;

{
  Helper Delphi per lavorare con la libreria SanitaServiceLib.
  Fornisce funzioni di utilità per:
  - Costruire stringhe input key=value
  - Analizzare stringhe output key=value
  - Costanti per i codici servizio (enum ServizioSanita)
  - Wrapper semplificato attorno alla COM interface

  Prerequisito:
    - SanitaServiceLib.dll registrata con regasm /tlb
    - SanitaServiceLib.tlb importata in Delphi (Project → Import Type Library)
      oppure usare questa unit standalone con CreateOleObject.
}

interface

uses
  SysUtils, Classes, StrUtils, Variants, ComObj, ActiveX;

// ─── Costanti servizi (mirror di ServizioSanita enum) ────────────────────────

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

  // Ambienti
  AMB_TEST       = 0;
  AMB_PRODUZIONE = 1;

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

// ─── Wrapper COM ──────────────────────────────────────────────────────────────

type
  TSanitaClient = class
  private
    FClient: Variant;
    FInizializzato: Boolean;
  public
    constructor Create;
    destructor Destroy; override;

    /// Configura credenziali e ambiente
    function Configura(const Username, Password: string;
                       Ambiente: Integer = AMB_TEST;
                       IgnoraSsl: Boolean = True): Boolean;

    /// Configura certificati (per produzione)
    procedure ConfiguraCertificati(const PathSsl, PathCA, PathSanitel: string);

    /// Chiama un servizio, restituisce stringa KV
    function Chiama(Servizio: Integer; const Input: string): string;

    /// Chiama e restituisce dizionario (TStringList con chiave=valore)
    function ChiamaDict(Servizio: Integer; const Input: string): TStringList;

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

// ─── Implementazione TSanitaClient ───────────────────────────────────────────

constructor TSanitaClient.Create;
begin
  inherited;
  FInizializzato := False;
  try
    FClient := CreateOleObject('SanitaServiceLib.Client');
  except
    on E: Exception do
      raise Exception.CreateFmt(
        'Impossibile creare SanitaServiceLib.Client. ' +
        'Verificare che SanitaServiceLib.dll sia registrata con regasm. ' +
        'Dettaglio: %s', [E.Message]);
  end;
end;

destructor TSanitaClient.Destroy;
begin
  FClient := Unassigned;
  inherited;
end;

function TSanitaClient.Configura(const Username, Password: string;
                                  Ambiente: Integer;
                                  IgnoraSsl: Boolean): Boolean;
begin
  Result := False;
  try
    FClient.Configura(Username, Password, Ambiente, IgnoraSsl);
    FInizializzato := True;
    Result := True;
  except
    on E: Exception do
      raise Exception.CreateFmt('Errore configurazione: %s', [E.Message]);
  end;
end;

procedure TSanitaClient.ConfiguraCertificati(
  const PathSsl, PathCA, PathSanitel: string);
begin
  FClient.ConfiguraCertificati(PathSsl, PathCA, PathSanitel);
end;

function TSanitaClient.Chiama(Servizio: Integer; const Input: string): string;
begin
  if not FInizializzato then
    raise Exception.Create('Client non configurato. Chiamare Configura() prima.');
  Result := FClient.Chiama(Servizio, Input);
end;

function TSanitaClient.ChiamaDict(Servizio: Integer; const Input: string): TStringList;
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

end.

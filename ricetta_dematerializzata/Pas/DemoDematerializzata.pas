unit DemoDematerializzata;

{
  Demo completa del flusso Prescrittore via COM:
    1. Configurazione client (test o produzione)
    2. Verifica token in memoria con CheckToken
    3. Creazione token con CreateAuth (auto in test, manuale via email in produzione)
    4. Configurazione Authorization2F
    5. Chiamata InvioPrescritto

  Dati di esempio allineati al form di test (MainForm.cs).

  Prerequisito:
    - ricetta_dematerializzata.dll registrata COM:
        regasm.exe ricetta_dematerializzata.dll /tlb:ricetta_dematerializzata.tlb /codebase
    - RicettaDematerializzataHelper.pas inclusa nel progetto
}

interface

uses
  SysUtils, Dialogs, RicettaDematerializzataHelper;

// ─── Costanti dati di esempio (allineate a MainForm.cs) ─────────────────────

const
  // Credenziali
  DEMO_USERNAME              = 'PROVAX00X00X000Y';
  DEMO_PASSWORD              = 'Salve123';

  // A2F
  DEMO_USER_ID               = 'PROVAX00X00X000Y';
  DEMO_CF_UTENTE             = 'PROVAX00X00X000Y';
  DEMO_IDENTIFICATIVO        = 'LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=';
  DEMO_COD_REGIONE_AUTH      = '130';
  DEMO_COD_ASL_AO_AUTH       = '202';
  DEMO_COD_SSA_AUTH          = '000000';
  DEMO_COD_STRUTTURA_TOKEN   = '';

  // Prescrittore
  DEMO_CF_ASSISTITO          = 'GLLGNN37B51C286O';
  DEMO_COD_REGIONE_SERVIZI   = '190';
  DEMO_COD_ASL_AO_SERVIZI    = '201';
  DEMO_COD_STRUTTURA_SERVIZI = '201600104';

  // Produzione: seriale del certificato nel Windows Certificate Store.
  // Lasciare vuoto (solo Basic Auth).
  DEMO_SERIALE_CERTIFICATO   = '';

// ─── Punto di ingresso demo ──────────────────────────────────────────────────

/// Esegue l'intero flusso:
///   CreateAuth (o CheckToken se token già presente) → InvioPrescritto.
/// IsTest = True  → AMB_TEST,  token estratto dalla risposta.
/// IsTest = False → AMB_PRODUZIONE, token inserito manualmente dall'utente.
procedure EseguiDemoInvioPrescritto(const IsTest: Boolean = True);

implementation

// ─── Builder input A2F ───────────────────────────────────────────────────────

function BuildCreateAuthInput: string;
begin
  Result := '';
  Result := KVSet(Result, 'userId',          DEMO_USER_ID);
  Result := KVSet(Result, 'identificativo',   DEMO_IDENTIFICATIVO);
  Result := KVSet(Result, 'cfUtente',         DEMO_CF_UTENTE);
  Result := KVSet(Result, 'codRegione',       DEMO_COD_REGIONE_AUTH);
  Result := KVSet(Result, 'codAslAo',         DEMO_COD_ASL_AO_AUTH);
  Result := KVSet(Result, 'codSsa',           DEMO_COD_SSA_AUTH);
  Result := KVSet(Result, 'codiceStruttura',  DEMO_COD_STRUTTURA_TOKEN);
  Result := KVSet(Result, 'contesto',         'RICETTA-DEM');
  Result := KVSet(Result, 'applicazione',     'PRESCRITTORE');
end;

function BuildCheckTokenInput(const Token: string): string;
begin
  Result := '';
  Result := KVSet(Result, 'userId',        DEMO_USER_ID);
  Result := KVSet(Result, 'identificativo', DEMO_IDENTIFICATIVO);
  Result := KVSet(Result, 'cfUtente',      DEMO_CF_UTENTE);
  Result := KVSet(Result, 'token',         Token);
  Result := KVSet(Result, 'contesto',      'RICETTA-DEM');
  Result := KVSet(Result, 'applicazione',  'PRESCRITTORE');
end;

// ─── Builder input InvioPrescritto ───────────────────────────────────────────

function BuildInvioPrescrittoInput: string;
var
  DataOra: string;
begin
  // Data compilazione = ora corrente formattata come richiesto dalla DLL
  DataOra := FormatDateTime('yyyy-mm-dd hh:nn:ss', Now);

  Result := '';
  // Dati medico
  Result := KVSet(Result, 'pinCode',              '1234567890');
  Result := KVSet(Result, 'cfMedico1',            DEMO_USER_ID);
  Result := KVSet(Result, 'codRegione',           DEMO_COD_REGIONE_SERVIZI);
  Result := KVSet(Result, 'codASLAo',             DEMO_COD_ASL_AO_SERVIZI);
  Result := KVSet(Result, 'codStruttura',         DEMO_COD_STRUTTURA_SERVIZI);
  Result := KVSet(Result, 'codSpecializzazione',  'P');
  // Dati assistito
  Result := KVSet(Result, 'codiceAss',            DEMO_CF_ASSISTITO);
  Result := KVSet(Result, 'provAssistito',        'AG');
  Result := KVSet(Result, 'aslAssistito',         '201');
  // Dati prescrizione
  Result := KVSet(Result, 'tipoPrescrizione',     'P');
  Result := KVSet(Result, 'nonEsente',            '1');
  Result := KVSet(Result, 'codDiagnosi',          '401.9');
  Result := KVSet(Result, 'descrizioneDiagnosi',  'IPERTENSIONE ARTERIOSA ESSENZIALE');
  Result := KVSet(Result, 'dataCompilazione',     DataOra);
  Result := KVSet(Result, 'tipoVisita',           'A');
  Result := KVSet(Result, 'classePriorita',       'P');
  // Dettagli prescrizione — riga 1
  Result := KVSet(Result, 'ElencoDettagliPrescrizioni', 'ARRAY');
  Result := KVSetArrayNode(Result, 'ElencoDettagliPrescrizioni', 1, 'codProdPrest',    '89.7');
  Result := KVSetArrayNode(Result, 'ElencoDettagliPrescrizioni', 1, 'descrProdPrest',  'VISITA CARDIOLOGICA');
  Result := KVSetArrayNode(Result, 'ElencoDettagliPrescrizioni', 1, 'quantita',        '1');
  Result := KVSetArrayNode(Result, 'ElencoDettagliPrescrizioni', 1, 'tipoAccesso',     '1');
  // Dettagli prescrizione — riga 2
  Result := KVSetArrayNode(Result, 'ElencoDettagliPrescrizioni', 2, 'codProdPrest',    '89.52');
  Result := KVSetArrayNode(Result, 'ElencoDettagliPrescrizioni', 2, 'descrProdPrest',  'ELETTROCARDIOGRAMMA (ECG) BASALE');
  Result := KVSetArrayNode(Result, 'ElencoDettagliPrescrizioni', 2, 'quantita',        '1');
  Result := KVSetArrayNode(Result, 'ElencoDettagliPrescrizioni', 2, 'tipoAccesso',     '1');
end;

// ─── Helpers token ───────────────────────────────────────────────────────────

// Estrae il token dalla risposta CreateAuth (struttura CODICE/MESSAGGIO).
// Cerca prima CODICE=token → MESSAGGIO corrispondente, poi fallback su MESSAGGIO diretto.
function EstraiTokenDaCreateAuth(const OutputKv: string): string;
var
  Suffissi: array[0..9] of string;
  i: Integer;
  Codice, Messaggio: string;
begin
  Result := '';
  Suffissi[0] := '';
  Suffissi[1] := '_1';
  Suffissi[2] := '_2';
  Suffissi[3] := '_3';
  Suffissi[4] := '_4';
  Suffissi[5] := '_5';
  Suffissi[6] := '_6';
  Suffissi[7] := '_7';
  Suffissi[8] := '_8';
  Suffissi[9] := '_9';

  for i := 0 to High(Suffissi) do
  begin
    Codice   := KVGet(OutputKv, 'CODICE'   + Suffissi[i]);
    Messaggio := KVGet(OutputKv, 'MESSAGGIO' + Suffissi[i]);
    if SameText(Codice, 'token') and (Messaggio <> '') then
    begin
      Result := Messaggio;
      Exit;
    end;
  end;

  // Fallback: campo MESSAGGIO diretto
  Result := KVGet(OutputKv, 'MESSAGGIO');
end;

function TokenValido(const CheckOutput: string): Boolean;
begin
  if KVIsErrore(CheckOutput) then
  begin
    Result := False;
    Exit;
  end;
  // Valido se CODESITO=0 e nessun ERRORE_NUMERO
  Result := SameText(KVGet(CheckOutput, 'CODESITO'), '0') or
            (KVGet(CheckOutput, 'ERRORE_NUMERO') = '');
end;

// ─── Flusso principale ───────────────────────────────────────────────────────

procedure EseguiDemoInvioPrescritto(const IsTest: Boolean);
var
  Client: TRicettaDematerializzataClient;
  TokenCorrente: string;
  CheckOutput: string;
  CreateOutput: string;
  InvioOutput: string;
  TokenGiaValido: Boolean;
  Seriale: string;
  Ambiente: Integer;
begin
  // Token in memoria: in un'applicazione reale questa variabile
  // sarebbe un campo della classe/form o una variabile di modulo.
  TokenCorrente := '';

  if IsTest then
  begin
    Ambiente := AMB_TEST;
    Seriale  := '';
  end
  else
  begin
    Ambiente := AMB_PRODUZIONE;
    Seriale  := '';
  end;

  Client := TRicettaDematerializzataClient.Create;
  try

    // ── 1. Configurazione ──────────────────────────────────────────────────
    Client.Configura(DEMO_USERNAME, DEMO_PASSWORD, Seriale, Ambiente);

    // ── 2. Verifica token già presente in memoria ──────────────────────────
    TokenGiaValido := False;
    if Trim(TokenCorrente) <> '' then
    begin
      CheckOutput := Client.Chiama(SRV_CHECK_TOKEN, BuildCheckTokenInput(TokenCorrente));
      TokenGiaValido := TokenValido(CheckOutput);
      if not TokenGiaValido then
        TokenCorrente := ''; // token scaduto: lo azzero
    end;

    // ── 3. CreateAuth (solo se token assente o non valido) ─────────────────
    if not TokenGiaValido then
    begin
      CreateOutput := Client.Chiama(SRV_CREATE_AUTH, BuildCreateAuthInput);

      if KVIsErrore(CreateOutput) then
        raise Exception.CreateFmt(
          'CreateAuth fallita: [%s] %s',
          [KVGetErroreNumero(CreateOutput), KVGetErroreDescrizione(CreateOutput)]);

      if IsTest then
      begin
        // TEST: il token è nella risposta, estrazione automatica
        TokenCorrente := EstraiTokenDaCreateAuth(CreateOutput);
        if Trim(TokenCorrente) = '' then
          raise Exception.Create('Token A2F non trovato nella risposta di CreateAuth.');
      end
      else
      begin
        // PRODUZIONE: il token NON è nella risposta.
        // Il sistema lo invia via email all'utente che deve inserirlo manualmente.
        ShowMessage(
          'Richiesta token inviata con successo.' + #13#10 +
          'Riceverai il token via email.' + #13#10 +
          'Inseriscilo nel campo che apparirà a breve.'
        );
        TokenCorrente := Trim(InputBox(
          'Token A2F — Produzione',
          'Incolla il token ricevuto via email:',
          ''
        ));
        if TokenCorrente = '' then
          raise Exception.Create('Token non inserito. Operazione annullata.');
      end;

      // In un'applicazione reale: salvare TokenCorrente in memoria di sessione/utente
    end;

    // ── 4. Configura Authorization2F con il token valido ──────────────────
    Client.ConfiguraAuthorization2F(TokenCorrente);

    // ── 5. InvioPrescritto ────────────────────────────────────────────────
    InvioOutput := Client.Chiama(SRV_INVIO_PRESCRITTO, BuildInvioPrescrittoInput);

    if KVIsErrore(InvioOutput) then
      raise Exception.CreateFmt(
        'InvioPrescritto fallita: [%s] %s',
        [KVGetErroreNumero(InvioOutput), KVGetErroreDescrizione(InvioOutput)]);

    // ── 6. Esito ──────────────────────────────────────────────────────────
    ShowMessage(
      'InvioPrescritto completato.' + #13#10 +
      'NRE: '   + KVGet(InvioOutput, 'NRE') + #13#10 +
      'Esito: ' + KVGet(InvioOutput, 'CODICE_ESITO_INSERIMENTO')
    );

  finally
    Client.Free;
  end;
end;

end.

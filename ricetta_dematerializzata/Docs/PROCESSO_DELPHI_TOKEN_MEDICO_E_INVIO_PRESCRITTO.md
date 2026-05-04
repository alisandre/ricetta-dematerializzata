# Processo Delphi esterno: token medico + InvioPrescritto + flusso Erogatore

Questo documento descrive due flussi tipici di una applicazione Delphi esterna che usa la DLL via COM:

1. flusso prescrittore con token A2F e `InvioPrescritto`,
2. flusso erogatore con `VisualizzaErogato` (presa in carico) seguito da `InvioErogato` (conferma erogazione).

---

## Prerequisiti

- DLL buildata in Release:
  - `ricetta_dematerializzata.dll`
- DLL registrata COM con `regasm /tlb /codebase`
- ProgID disponibile:
  - `RicettaDematerializzata.Client`
- unit Delphi disponibile:
  - `RicettaDematerializzata.pas` (`RicettaDematerializzataHelper`)
- certificato Sanitel disponibile se richiesto dal contesto operativo.

---

## Ipotesi applicativa

Si assume che l'applicazione Delphi mantenga in memoria, per il medico loggato:

- `LoggedUserCfMedico`
- `LoggedUserUsername`
- `LoggedUserPassword`
- `CurrentTokenPrescrittore`

Il token può essere memorizzato:
- solo in RAM,
- in sessione utente,
- oppure anche persistito localmente.

In questo documento il focus è sul caso **in memoria**.

---

## Comportamento di `CreateAuth` per ambiente

> ⚠️ Il comportamento di `CreateAuth` cambia in base all'ambiente:

| Ambiente | Risposta `CreateAuth` | Come si ottiene il token |
|----------|-----------------------|--------------------------|
| **Test** | Il token è nella risposta KV (`MESSAGGIO` / `CODICE=token`) | Estratto automaticamente dalla risposta |
| **Produzione** | Il token **non è nella risposta** — viene inviato via **email** all'utente | L'utente deve inserirlo manualmente nell'applicazione |

Questo significa che il flusso di creazione token **deve essere differenziato** a livello applicativo in base all'ambiente configurato.

---

## Flusso logico

```text
[Login medico]
      ↓
[Leggo token già presente in memoria]
      ↓
[token presente?] ── no ──→ [CreateAuth]
      │                          │
      yes                        ↓
      ↓                   [Ambiente TEST?] ── sì ──→ [estrai token dalla risposta]
[CheckToken]                     │                          ↓
      ↓                          no                  [salvo token in memoria]
[token valido?]                  ↓
      │              [token inviato via email]
      no                         ↓
      │              [chiedo token all'utente]
      ↓                          ↓
[CreateAuth]          [salvo token in memoria]
      │
      yes
      ↓
[ConfiguraAuthorization2F(token)]
      ↓
[InvioPrescritto]
      ↓
[Gestione output/errore]
```

---

## Servizi coinvolti

### A2F (via `Chiama`)
- `SRV_CREATE_AUTH = 20`
- `SRV_REVOKE_AUTH = 21`
- `SRV_CHECK_TOKEN = 22`

### Prescrittore
- `SRV_INVIO_PRESCRITTO = 2`

### Erogatore
- `SRV_VISUALIZZA_EROGATO = 11` (presa in carico / blocco ricetta)
- `SRV_INVIO_EROGATO = 10` (conferma erogazione prestazioni)

---

## Dettaglio processo

### 1. Inizializzazione client COM

```pascal
uses
  SysUtils, RicettaDematerializzataHelper;

var
  Client: TRicettaDematerializzataClient;
begin
  Client := TRicettaDematerializzataClient.Create;
  // Test: seriale vuoto → Basic Auth, SSL validation disabilitata
  Client.Configura(LoggedUserUsername, LoggedUserPassword, '', AMB_TEST);
  // Produzione con certificato:
  // Client.Configura(LoggedUserUsername, LoggedUserPassword, 'F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D', AMB_PRODUZIONE);
end;
```

Nota:
- I servizi token A2F vengono chiamati anch'essi con `Client.Chiama(...)` usando gli ID `20/21/22`.

---

### 2. Verifica token già presente in memoria

```pascal
if Trim(CurrentTokenPrescrittore) <> '' then
begin
  // esiste già un token in memoria, provo a validarlo
end
else
begin
  // token assente, dovrò crearlo
end;
```

---

### 3. CheckToken

Per `CheckToken` la DLL espone il servizio A2F tramite `Chiama(...)` con `SRV_CHECK_TOKEN = 22`.

Esempio input KV:

```text
userId=PROVAX00X00X000Y;
identificativo_tipo=P;
identificativo_valore=BASE64_O_IDENTIFICATIVO;
cfUtente=PROVAX00X00X000Y;
token=<TOKEN>;
contesto=RICETTA-DEM;
applicazione=PRESCRITTORE
```

Esempio Delphi:

```pascal
function BuildCheckTokenInput(const UserId, CfUtente, Identificativo, Token: string): string;
begin
  Result := '';
  Result := KVSet(Result, 'userId', UserId);
  Result := KVSet(Result, 'identificativo', Identificativo);
  Result := KVSet(Result, 'cfUtente', CfUtente);
  Result := KVSet(Result, 'token', Token);
  Result := KVSet(Result, 'contesto', 'RICETTA-DEM');
  Result := KVSet(Result, 'applicazione', 'PRESCRITTORE');
end;
```

Chiamata:

```pascal
CheckOutput := Client.Chiama(
  SRV_CHECK_TOKEN,
  BuildCheckTokenInput(LoggedUserCfMedico, LoggedUserCfMedico, IdentificativoA2F, CurrentTokenPrescrittore)
);
```

Valutazione esito:

```pascal
function TokenValido(const OutputKv: string): Boolean;
begin
  Result := not KVIsErrore(OutputKv);
  if Result then
    Result := SameText(KVGet(OutputKv, 'CODESITO'), '0') or (KVGet(OutputKv, 'ERRORE_NUMERO') = '');
end;
```

Se il token non è valido, si passa a `CreateAuth` (`SRV_CREATE_AUTH = 20`).

---

### 4. CreateAuth

Esempio input Delphi:

```pascal
function BuildCreateAuthInput(const UserId, CfUtente, Identificativo: string): string;
begin
  Result := '';
  Result := KVSet(Result, 'userId', UserId);
  Result := KVSet(Result, 'identificativo', Identificativo);
  Result := KVSet(Result, 'cfUtente', CfUtente);
  Result := KVSet(Result, 'codRegione', '130');
  Result := KVSet(Result, 'codAslAo', '202');
  Result := KVSet(Result, 'codSsa', '000000');
  Result := KVSet(Result, 'codiceStruttura', '');
  Result := KVSet(Result, 'contesto', 'RICETTA-DEM');
  Result := KVSet(Result, 'applicazione', 'PRESCRITTORE');
end;
```

Chiamata:

```pascal
CreateOutput := Client.Chiama(
  SRV_CREATE_AUTH,
  BuildCreateAuthInput(LoggedUserCfMedico, LoggedUserCfMedico, IdentificativoA2F)
);
```

#### Recupero token — TEST

In ambiente di **test** il token è incluso nella risposta e può essere estratto direttamente:

```pascal
function EstraiTokenDaCreateAuth(const OutputKv: string): string;
begin
  Result := KVGet(OutputKv, 'MESSAGGIO');
  if Result = '' then
    Result := KVGet(OutputKv, 'MESSAGGIO_1');
end;

// Test:
CurrentTokenPrescrittore := EstraiTokenDaCreateAuth(CreateOutput);
if Trim(CurrentTokenPrescrittore) = '' then
  raise Exception.Create('Token A2F non ottenuto da CreateAuth.');
```

#### Recupero token — PRODUZIONE

In ambiente di **produzione** il token **non è presente nella risposta**. Il sistema lo invia via **email** all'utente. L'applicazione deve:

1. informare l'utente che il token è stato inviato per email,
2. attendere che l'utente lo inserisca manualmente in un campo apposito,
3. salvarlo in memoria per le chiamate successive.

```pascal
// Produzione: il token non è nella risposta, arriva via email
procedure RichiediTokenViaEmail(const CreateOutput: string);
var
  Token: string;
begin
  if KVIsErrore(CreateOutput) then
    raise Exception.Create('CreateAuth fallita: ' + KVGetErroreDescrizione(CreateOutput));

  // Informa l'utente
  ShowMessage(
    'La richiesta di token è stata inviata con successo.' + #13#10 +
    'Riceverai il token via email.' + #13#10 +
    'Inseriscilo nell''apposito campo quando disponibile.'
  );

  // L'utente incolla il token ricevuto per email
  Token := Trim(InputBox('Token A2F', 'Incolla il token ricevuto via email:', ''));
  if Token = '' then
    raise Exception.Create('Token non inserito. Operazione annullata.');

  CurrentTokenPrescrittore := Token;
end;
```

---

### 5. Memorizzazione token in memoria

```pascal
CurrentTokenPrescrittore := NuovoToken;
```

Opzionalmente si può salvare anche in:
- file locale cifrato,
- registry,
- session manager applicativo.

---

### 6. Configurazione Authorization2F nel client

Prima di chiamare `InvioPrescritto`:

```pascal
Client.ConfiguraAuthorization2F(CurrentTokenPrescrittore);
```

La DLL accetta sia:
- token puro,
- sia stringa già prefissata `Bearer ...`.

---

### 7. InvioPrescritto

Esempio input minimo:

```pascal
function BuildInvioPrescrittoInput(const CfMedico: string): string;
begin
  Result := '';
  Result := KVSet(Result, 'pinCode', '1234567890');
  Result := KVSet(Result, 'cfMedico1', CfMedico);
  Result := KVSet(Result, 'codRegione', '190');
  Result := KVSet(Result, 'codASLAo', '201');
  Result := KVSet(Result, 'codStruttura', '201600104');
  Result := KVSet(Result, 'codSpecializzazione', 'P');
  Result := KVSet(Result, 'tipoPrescrizione', 'F');
  Result := KVSet(Result, 'dataCompilazione', '2026-01-01 10:00:00');
  Result := KVSet(Result, 'tipoVisita', 'A');
  Result := KVSet(Result, 'ElencoDettagliPrescrizioni', 'ARRAY');
  Result := KVSetArrayNode(Result, 'ElencoDettagliPrescrizioni', 1, 'codProdPrest', '891011');
  Result := KVSetArrayNode(Result, 'ElencoDettagliPrescrizioni', 1, 'descrProdPrest', 'VISITA CARDIOLOGICA');
  Result := KVSetArrayNode(Result, 'ElencoDettagliPrescrizioni', 1, 'quantita', '1');
end;
```

Chiamata:

```pascal
InvioOutput := Client.Chiama(
  SRV_INVIO_PRESCRITTO,
  BuildInvioPrescrittoInput(LoggedUserCfMedico)
);
```

---

## Flusso Erogatore: presa in carico + conferma erogazione

Per l'erogatore il flusso applicativo raccomandato è:

1. `VisualizzaErogato` con `tipoOperazione=1` per prendere in carico la ricetta;
2. `InvioErogato` per confermare che le prestazioni sono state erogate.

### 1) VisualizzaErogato (presa in carico)

Input esempio:

```pascal
function BuildVisualizzaErogatoInput: string;
begin
  Result := '';
  Result := KVSet(Result, 'pinCode',                'TSTSIC00B01H501E');
  Result := KVSet(Result, 'codiceRegioneErogatore', '190');
  Result := KVSet(Result, 'codiceAslErogatore',     '201');
  Result := KVSet(Result, 'codiceSsaErogatore',     '888888');
  Result := KVSet(Result, 'nre',                    '1900A4005322015');
  Result := KVSet(Result, 'tipoOperazione',         '1');
  Result := KVSet(Result, 'cfAssistito',            'GLLGNN37B51C286O');
end;
```

Chiamata:

```pascal
VisualizzaOutput := Client.Chiama(
  SRV_VISUALIZZA_EROGATO,
  BuildVisualizzaErogatoInput
);
```

### 2) InvioErogato (conferma erogazione)

Input esempio:

```pascal
function BuildInvioErogatoInput: string;
var
  DataOra: string;
begin
  DataOra := FormatDateTime('yyyy-mm-dd hh:nn:ss', Now);

  Result := '';
  Result := KVSet(Result, 'pinCode',                'TSTSIC00B01H501E');
  Result := KVSet(Result, 'codiceRegioneErogatore', '190');
  Result := KVSet(Result, 'codiceAslErogatore',     '201');
  Result := KVSet(Result, 'codiceSsaErogatore',     '888888');
  Result := KVSet(Result, 'pwd',                    'TSTSIC00B01H501E');
  Result := KVSet(Result, 'nre',                    '1900A4005322015');
  Result := KVSet(Result, 'cfAssistito',            'GLLGNN37B51C286O');
  Result := KVSet(Result, 'tipoOperazione',         '1');
  Result := KVSet(Result, 'prescrizioneFruita',     '1');
  Result := KVSet(Result, 'dataSpedizione',         DataOra);
end;
```

Chiamata:

```pascal
InvioOutput := Client.Chiama(
  SRV_INVIO_EROGATO,
  BuildInvioErogatoInput
);
```

Sequenza:

```text
VisualizzaErogato (presa in carico) → InvioErogato (conferma erogazione)
```

---

## Procedura completa di esempio

```pascal
procedure EseguiInvioPrescrittoConToken(const IsTest: Boolean);
var
  Client: TRicettaDematerializzataClient;
  CheckOutput, CreateOutput, InvioOutput: string;
  TokenValidoInMemoria: Boolean;
begin
  Client := TRicettaDematerializzataClient.Create;
  try
    if IsTest then
      Client.Configura(LoggedUserUsername, LoggedUserPassword, '', AMB_TEST)
    else
      Client.Configura(LoggedUserUsername, LoggedUserPassword, 'SERIALE_CERTIFICATO', AMB_PRODUZIONE);

    TokenValidoInMemoria := False;

    if Trim(CurrentTokenPrescrittore) <> '' then
    begin
      CheckOutput := Client.Chiama(
        SRV_CHECK_TOKEN,
        BuildCheckTokenInput(
          LoggedUserCfMedico,
          LoggedUserCfMedico,
          IdentificativoA2F,
          CurrentTokenPrescrittore));

      TokenValidoInMemoria := TokenValido(CheckOutput);
    end;

    if not TokenValidoInMemoria then
    begin
      CreateOutput := Client.Chiama(
        SRV_CREATE_AUTH,
        BuildCreateAuthInput(
          LoggedUserCfMedico,
          LoggedUserCfMedico,
          IdentificativoA2F));

      if KVIsErrore(CreateOutput) then
        raise Exception.Create('CreateAuth fallita: ' + KVGetErroreDescrizione(CreateOutput));

      if IsTest then
      begin
        // TEST: token nella risposta, estrazione automatica
        CurrentTokenPrescrittore := EstraiTokenDaCreateAuth(CreateOutput);
        if Trim(CurrentTokenPrescrittore) = '' then
          raise Exception.Create('Token A2F non trovato nella risposta di CreateAuth.');
      end
      else
      begin
        // PRODUZIONE: token inviato via email, inserimento manuale obbligatorio
        ShowMessage(
          'La richiesta di token è stata inviata con successo.' + #13#10 +
          'Riceverai il token via email.' + #13#10 +
          'Inseriscilo nell''apposito campo quando disponibile.'
        );
        CurrentTokenPrescrittore := Trim(InputBox(
          'Token A2F',
          'Incolla il token ricevuto via email:',
          ''
        ));
        if CurrentTokenPrescrittore = '' then
          raise Exception.Create('Token non inserito. Operazione annullata.');
      end;
    end;

    Client.ConfiguraAuthorization2F(CurrentTokenPrescrittore);

    InvioOutput := Client.Chiama(
      SRV_INVIO_PRESCRITTO,
      BuildInvioPrescrittoInput(LoggedUserCfMedico));

    if KVIsErrore(InvioOutput) then
      raise Exception.Create('InvioPrescritto fallita: ' + KVGetErroreDescrizione(InvioOutput));

    // gestione esito applicativo
    Writeln('Invio completato. Output: ' + InvioOutput);
  finally
    Client.Free;
  end;
end;
```

---

## Output attesi

### Caso 1: token già valido
- CheckToken restituisce esito valido
- non viene chiamato CreateAuth
- InvioPrescritto parte subito

### Caso 2: token assente — TEST
- CreateAuth genera nuovo token
- token estratto automaticamente dalla risposta
- token salvato in memoria
- InvioPrescritto parte con nuovo token

### Caso 3: token assente — PRODUZIONE
- CreateAuth invia richiesta al server
- il token **non è nella risposta**: viene recapitato via **email** all'utente
- l'applicazione mostra un avviso e attende l'inserimento manuale del token
- token salvato in memoria dopo conferma utente
- InvioPrescritto parte con il token inserito

### Caso 4: token presente ma scaduto/non valido
- CheckToken fallisce o restituisce token non valido
- si ricade nel flusso CreateAuth (caso 2 o 3 a seconda dell'ambiente)
- token in memoria aggiornato
- InvioPrescritto parte con token aggiornato

---

## Raccomandazioni pratiche

1. salvare sempre il token con chiave utente/medico loggato;
2. non riusare token di un medico per un altro medico;
3. loggare separatamente:
   - output CheckToken,
   - output CreateAuth,
   - output InvioPrescritto;
4. in produzione l'inserimento del token via email richiede interazione utente: prevedere un campo dedicato nell'interfaccia (non un `InputBox` modale in contesti non interattivi);
5. impostare timeout e retry a livello applicativo solo dove appropriato;
6. in produzione usare sempre i certificati corretti e la configurazione ambiente coerente.

---

## Documenti correlati

- `INTERFACCIAMENTO_ESTERNO.md`
- `A2F_COMPENDIO.md`
- `README.md`

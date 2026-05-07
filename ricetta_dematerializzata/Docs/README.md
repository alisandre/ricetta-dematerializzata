# ricetta_dematerializzata

Libreria .NET Framework 4.8 per i servizi SAC/DEM Ricetta, utilizzabile:
- da applicazioni .NET (riferimento diretto alla DLL),
- da applicazioni esterne via **COM Interop** (es. Delphi).

> ProgID COM esposto: `RicettaDematerializzata.Client`

---

## 🚀 Quick Start

**Se vuoi solo installare la DLL rapidamente:**

→ Vedi [`QUICK_START_INSTALLAZIONE.md`](./QUICK_START_INSTALLAZIONE.md)

**Se vuoi integrare la DLL in un'applicazione esterna:**

→ Vedi [`INSTALLAZIONE_E_UTILIZZO.md`](./INSTALLAZIONE_E_UTILIZZO.md)

**Se vuoi capire l'architettura completa:**

→ Continua a leggere questo documento

---

## Architettura

```
ricetta_dematerializzata/
├── Models/
│   └── Enums.cs                              DigitalPrescriptionService (enum catalogo), ServiceEnvironment
├── Core/
│   ├── ServicesCatalog.cs                    Mappa enum → URL + SOAPAction
│   ├── ServiceConfiguration.cs               Credenziali, seriale certificato, ambiente
│   ├── InputMapperService.cs                 Normalizzazione e validazione input per servizio
│   ├── ParserKV.cs                           Serializzazione "CHIAVE=VALORE;..."
│   ├── SoapHelper.cs                         Build SOAP envelope, parse risposta XML
│   ├── SoapHttpClient.cs                     HTTP client con Basic Auth + SSL opzionale
│   ├── SoapDebugCapture.cs                   Cattura request/response SOAP per debug
│   └── SoapServiceInterceptor.cs             Interceptor SOAP (logging, hook pre/post chiamata)
├── Crypto/
│   └── OpenSSLEncoding.cs                    Cifratura CMS con certificato Sanitel
├── Services/
│   ├── RicettaDematerializzataBaseClient.cs  Client base servizi DEM
│   ├── Auth2FClient.cs                       Client servizi A2F
│   ├── ComInterop.cs                         IRicettaDematerializzataClient (interfaccia COM-visible)
│   └── ComRegistration.cs                    RicettaDematerializzataClient + RicettaDematerializzataFactory
├── certificates/
│   └── SanitelCF.cer                         Certificato Sanitel per cifratura CMS
├── docs/                                     Documentazione (vedere INDICE_DOCUMENTAZIONE.md)
└── RicettaDematerializzataHelper.pas         Helper Delphi con wrapper e utility KV
```

---

## Formato chiave=valore

Il contratto di I/O usa stringhe nel formato:

```
CHIAVE1=VALORE1;CHIAVE2=VALORE2;CHIAVE3=VALORE3
```

**Regole:**
- Separatore coppie: `;`
- Separatore chiave/valore: `=` (solo il primo `=` conta)
- Chiavi: case-insensitive, uppercase in output
- Valori con `;` embedded: escaped con `;;`
- Valori vuoti: ammessi (`CHIAVE=`)

**Esempi:**

```
Input:
  NRE=120000000000;cfMedico=RSSMRA80A01H501T;pinCode=123456

Output successo:
  CODICE_ESITO_INSERIMENTO=0000;NRE=120000000000;AVVISO=

Output errore:
  ERRORE_NUMERO=9999;ERRORE_DESCRIZIONE=Pincode non valido
```

**Codici esito standard:**

| Codice | Significato |
|--------|-------------|
| `0000` | Transazione avvenuta con successo |
| `0001` | Successo con avvisi non bloccanti |
| `9999` | Errore bloccante |

---

## Catalogo servizi (enum DigitalPrescriptionService)

### Prescrittore

| Valore | Nome | Descrizione |
|--------|------|-------------|
| 1 | `VisualizzaPrescritto` | Dettagli di una prescrizione |
| 2 | `InvioPrescritto` | Invio nuova prescrizione |
| 3 | `AnnullaPrescritto` | Annullamento prescrizione |
| 4 | `InterrogaNreUtilizzati` | Lista NRE già usati |
| 5 | `ServiceAnagPrescrittore` | Dati anagrafici assistito |
| 6 | `InvioDichiarazioneSostituzioneMedico` | Sostituzione medico |

### Erogatore

| Valore | Nome | Descrizione |
|--------|------|-------------|
| 10 | `InvioErogato` | Conferma erogazione prestazioni (dopo presa in carico) |
| 11 | `VisualizzaErogato` | Presa in carico / blocco ricetta |
| 12 | `SospendiErogato` | Sospensione erogato |
| 13 | `AnnullaErogato` | Annullamento erogato |
| 14 | `RicercaErogatore` | Ricerca farmacia/erogatore |
| 15 | `ReportErogatoMensile` | Report mensile |
| 16 | `ServiceAnagErogatore` | Anagrafica (lato erogatore) |
| 17 | `RicettaDifferita` | Ricetta differita |
| 18 | `AnnullaErogatoDiff` | Annulla erogato differito |
| 19 | `RicevuteSac` | Ricevute SAC |

**Flusso operativo erogatore raccomandato**

1. `VisualizzaErogato` con `tipoOperazione=1` per prendere in carico la ricetta.
2. `InvioErogato` per confermare l'erogazione delle prestazioni.

### Authorization 2D (A2F)

| Valore | Nome | Descrizione |
|--------|------|-------------|
| 20 | `CreateAuth` | Crea token di autenticazione |
| 21 | `RevokeAuth` | Revoca token |
| 22 | `CheckToken` | Verifica stato token |

---

## Configurazione

### Metodo principale

```csharp
// C# — con SerialeCertificatoSsl (se disponibile) o senza (solo Basic Auth)
var client = new RicettaDematerializzataBaseClient();
client.Configura("utente", "password", "F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D", ambiente: 1);
// Senza certificato (solo Basic Auth):
// client.Configura("utente", "password", "", ambiente: 0);
```

```pascal
{ Delphi }
client.Configura('utente', 'password', 'F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D', AMB_PRODUZIONE);
{ Senza certificato: }
{ client.Configura('utente', 'password', '', AMB_TEST); }
```

**Comportamento automatico per ambiente:**

| Ambiente | Validazione SSL server |
|----------|----------------------|
| Test (0) | Disabilitata automaticamente |
| Produzione (1) | Abilitata con CA in `certificates/` |

**Politica di autenticazione per servizio:**

| Tipo servizio | Autenticazione | Certificato client |
|---------------|---------------|-------------------|
| A2F (20/21/22) | Basic Auth | No |
| Prescrittore (1–6) | Basic Auth + SSL | Sì (se seriale impostato) |
| Erogatore (10–19) | Basic Auth + SSL | Sì (se seriale impostato) |

---

## Certificato Sanitel

`pinCode` e `cfAssistito` vengono **cifrati automaticamente** con il certificato Sanitel:
- **Posizione hardcodata**: `certificates/SanitelCF.cer` (relativo alla cartella di esecuzione)
- Il file deve essere distribuito accanto all'eseguibile
- Non è configurabile via API

---

## Compilazione DLL

```powershell
dotnet build .\ricetta_dematerializzata\ricetta_dematerializzata.csproj -c Release
```

Output:
- `ricetta_dematerializzata\bin\Release\net48\ricetta_dematerializzata.dll`

---

## Uso esterno via COM (Delphi, VB6, script, ecc.)

### 1) Registrare la DLL COM

```bat
cd /d C:\Repositories\ricetta-dematerializzata\ricetta_dematerializzata\bin\Release\net48
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm.exe" ricetta_dematerializzata.dll /tlb:ricetta_dematerializzata.tlb /codebase
```

Per deregistrare:

```bat
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm.exe" ricetta_dematerializzata.dll /unregister
```

### 2) API pubblica

- `Configura(username, password, seriale, ambiente)` — configurazione principale
- `ConfiguraAuthorization2F(tokenOrBearer)` — token A2F da usare sui servizi DEM (prescrittore/erogatore)
- `Chiama(servizio, parametriInputKV)` — chiamata servizio (inclusi A2F con codici `20/21/22`)
- `ChiamaJson(servizio, parametriInputJson)` — chiamata con I/O JSON
- `OttieniUrl(servizio)` — URL endpoint
- `TestConnessione(servizio)` — test raggiungibilità
- `CifraConSanitel(testoPiano)` — cifratura manuale con Sanitel

---

## Uso da Delphi

```pascal
uses RicettaDematerializzataHelper;

var
  client: TRicettaDematerializzataClient;
  output: string;
begin
  client := TRicettaDematerializzataClient.Create;
  try
    client.Configura('utente', 'password', 'F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D', AMB_PRODUZIONE);

    // A2F chiamato dall'esterno via Chiama(20/21/22)
    output := client.Chiama(SRV_CREATE_AUTH,
      'userId=PROVAX00X00X000Y;cfUtente=PROVAX00X00X000Y;contesto=RICETTA-DEM;applicazione=PRESCRITTORE');
    client.ConfiguraAuthorization2F(KVGet(output, 'MESSAGGIO'));

    // Prescrittore → SSL automatico
    output := client.Chiama(SRV_INVIO_PRESCRITTO,
      'pinCode=1234567890;nre=120000000000;cfMedico1=PROVAX00X00X000Y;' +
      'codRegione=190;codASLAo=201;codSpecializzazione=P;codiceAss=RSSMRA80A01H501T;' +
      'tipoPrescrizione=P;dataCompilazione=2026-01-01 10:00:00;tipoVisita=A;' +
      'ElencoDettagliPrescrizioni=ARRAY;' +
      'ElencoDettagliPrescrizioni_1_codProdPrest=891011;' +
      'ElencoDettagliPrescrizioni_1_quantita=1');

    if KVIsErrore(output) then
      ShowMessage(KVGetErroreNumero(output) + ' - ' + KVGetErroreDescrizione(output));
  finally
    client.Free;
  end;
end;
```

---

## Uso da .NET esterno (senza COM)

```csharp
var client = new RicettaDematerializzataBaseClient();
client.Configura("utente", "password", "F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D", ambiente: 1);

// A2F via Chiama con codici enum 20/21/22
var createOut = client.Chiama((int)DigitalPrescriptionService.CreateAuth,
    "userId=PROVAX00X00X000Y;cfUtente=PROVAX00X00X000Y;contesto=RICETTA-DEM;applicazione=PRESCRITTORE");

var token = ParserKV.Parse(createOut).TryGetValue("MESSAGGIO", out var t) ? t : string.Empty;
client.ConfiguraAuthorization2F(token);

// Prescrittore
var output = client.Chiama((int)DigitalPrescriptionService.InvioPrescritto,
    "pinCode=1234567890;nre=120000000000;cfMedico1=PROVAX00X00X000Y;" +
    "codRegione=190;codASLAo=201;codSpecializzazione=P;codiceAss=RSSMRA80A01H501T;" +
    "tipoPrescrizione=P;dataCompilazione=2026-01-01 10:00:00;tipoVisita=A;" +
    "ElencoDettagliPrescrizioni=ARRAY;" +
    "ElencoDettagliPrescrizioni_1_codProdPrest=891011;" +
    "ElencoDettagliPrescrizioni_1_quantita=1");
```

---

## Documentazione correlata

| File | Contenuto |
|------|-----------|
| `INTERFACCIAMENTO_ESTERNO.md` | Guida completa COM/.NET, registrazione, esempi Delphi |
| `PARAMETRI_SERVIZI.md` | Parametri di ogni servizio con alias e obbligatorietà |
| `POLITICA_AUTENTICAZIONE.md` | Autenticazione automatica per servizio |
| `CONFIGURAZIONE_CERTIFICATO_SSL_SERIALE.md` | Certificato client via Windows Certificate Store |
| `PROCESSO_DELPHI_TOKEN_MEDICO_E_INVIO_PRESCRITTO.md` | Flusso completo token + InvioPrescritto in Delphi |
| `A2F_COMPENDIO.md` | CreateAuth / RevokeAuth / CheckToken |

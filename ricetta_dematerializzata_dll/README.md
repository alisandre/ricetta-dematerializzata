# SanitaServiceLib

Libreria C# per l'accesso ai servizi web SAC/DEM Ricetta del Ministero della Salute  
(demservicetest.sanita.finanze.it / demservice.sanita.finanze.it).

Progettata per essere usata direttamente da **C#** o tramite **COM Interop da Delphi**.

---

## Architettura

```
SanitaServiceLib/
├── Models/
│   └── Enums.cs                  ServizioSanita (enum catalogo), AmbienteSanita
├── Core/
│   ├── CatalogoServizi.cs        Mappa enum → URL + SOAPAction
│   ├── ConfigurazioneServizio.cs Credenziali, certificati, ambiente
│   ├── ParserKV.cs               Serializzazione "CHIAVE=VALORE;..."
│   ├── SoapHelper.cs             Build SOAP envelope, parse risposta XML
│   └── SoapHttpClient.cs         HTTP client con Basic Auth preventiva + SSL
├── Crypto/
│   └── CifraturaOpenSsl.cs       Cifratura CMS con Sanitel.cer (CF e pincode)
├── Services/
│   └── SanitaServiceClient.cs    Client principale (C# e COM)
├── Interop/
│   ├── ComInterop.cs             ISanitaServiceClient (interfaccia COM-visible)
│   └── ComRegistration.cs        SanitaServiceClientCom + SanitaServiceFactory
└── DelphiHelper/
    └── SanitaServiceHelper.pas   Unit Delphi con wrapper e utility KV
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
  NRE=120000000000;CODICE_FISCALE_MEDICO=RSSMRA80A01H501T;PINCODE=123456

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

## Catalogo servizi (enum ServizioSanita)

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
| 10 | `InvioErogato` | Invio erogato farmacia |
| 11 | `VisualizzaErogato` | Dettagli erogato |
| 12 | `SospendiErogato` | Sospensione erogato |
| 13 | `AnnullaErogato` | Annullamento erogato |
| 14 | `RicercaErogatore` | Ricerca farmacia/erogatore |
| 15 | `ReportErogatoMensile` | Report mensile |
| 16 | `ServiceAnagErogatore` | Anagrafica (lato erogatore) |
| 17 | `RicettaDifferita` | Ricetta differita |
| 18 | `AnnullaErogatoDiff` | Annulla erogato differito |
| 19 | `RicevuteSac` | Ricevute SAC |

### Authorization 2F (A2F)

| Valore | Nome | Descrizione |
|--------|------|-------------|
| 20 | `CreateAuth` | Crea token di autenticazione |
| 21 | `RevokeAuth` | Revoca token |
| 22 | `CheckToken` | Verifica stato token |

---

## Esempi rapidi A2F (formato key=value)

```
CreateAuth:
  userId=PROVAX00X00X000Y;cfUtente=PROVAX00X00X000Y;codRegione=190;codAslAo=201;codSsa=201600;codiceStruttura=;contesto=DEM;applicazione=PRESCRITTO

RevokeAuth:
  userId=PROVAX00X00X000Y;cfUtente=PROVAX00X00X000Y;token=00000000-0000-0000-0000-000000000000;contesto=DEM;applicazione=PRESCRITTO

CheckToken:
  userId=PROVAX00X00X000Y;cfUtente=PROVAX00X00X000Y;token=00000000-0000-0000-0000-000000000000;contesto=DEM;applicazione=PRESCRITTO
```

---

## Compilazione

```bash
# .NET Framework 4.8
dotnet build SanitaServiceLib.csproj -c Release

# oppure con msbuild
msbuild SanitaServiceLib.csproj /p:Configuration=Release
```

**Output:** `bin/Release/net48/SanitaServiceLib.dll`

---

## Registrazione COM per Delphi

Da un prompt **amministratore**:

```bat
:: Registra e genera la Type Library
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm.exe" ^
    SanitaServiceLib.dll /tlb:SanitaServiceLib.tlb /codebase

:: Per rimuovere
regasm SanitaServiceLib.dll /unregister
```

In Delphi: **Project → Import Type Library → SanitaServiceLib.tlb**

---

## Uso da C#

```csharp
// Configurazione test (SSL ignorato)
var config = ConfigurazioneServizio.CreaTest("utente", "password");
var client = new SanitaServiceClient(config);

// Invio prescrizione
string output = client.Chiama(ServizioSanita.InvioPrescritto,
    "NRE=120000000000;CODICE_FISCALE_MEDICO=RSSMRA80A01H501T;" +
    "CODICE_FISCALE_ASSISTITO=BNCGPP70B12H501Q;PINCODE=123456");

if (ParserKV.IsErrore(output))
    Console.WriteLine("ERRORE: " + ParserKV.Get(output, "ERRORE_DESCRIZIONE"));
else
    Console.WriteLine("NRE: " + ParserKV.Get(output, "NRE"));
```

```csharp
// Configurazione produzione con certificati
var config = ConfigurazioneServizio.CreaProduzione(
    "utente", "password",
    pathCertSsl:  @"C:\cert\demservice.pem",
    pathCertCA:   @"C:\cert\CAEntrate.pem",
    pathSanitel:  @"C:\cert\Sanitel.cer");
```

---

## Uso da Delphi (con SanitaServiceHelper.pas)

```pascal
uses SanitaServiceHelper;

var
  client: TSanitaClient;
  output, nre: string;
begin
  client := TSanitaClient.Create;
  try
    client.Configura('utente', 'password', AMB_TEST, True);

    output := client.Chiama(SRV_INVIO_PRESCRITTO,
      KVBuild(
        ['NRE', 'CODICE_FISCALE_MEDICO', 'CODICE_FISCALE_ASSISTITO'],
        ['120000000000', 'RSSMRA80A01H501T', 'BNCGPP70B12H501Q']
      ));

    if KVIsErrore(output) then
      ShowMessage('Errore ' + KVGetErroreNumero(output) +
                  ': ' + KVGetErroreDescrizione(output))
    else
    begin
      nre := KVGet(output, 'NRE');
      ShowMessage('Prescrizione inviata! NRE: ' + nre);
    end;
  finally
    client.Free;
  end;
end;
```

---

## Certificati richiesti

| File | Ambiente | Scopo |
|------|----------|-------|
| `demservicetest.pem` | Test | Certificato SSL di trasporto |
| `CA Agenzia delle Entrate Test.pem` | Test | Certification Authority |
| `demservice.pem` | Produzione | Certificato SSL di trasporto |
| `CAEntrate.pem` | Produzione | Certification Authority |
| `Sanitel.cer` | Entrambi | Cifratura CF e pincode assistito |

I file `.pem` sono forniti da Sogei nel pacchetto `leggimi.txt`.  
Il file `Sanitel.cer` è nel pacchetto `demPrescritto.zip → SanitelCF.rar`.

---

## Autenticazione

Autenticazione **Basic HTTP preventiva** (senza attendere il challenge 401):

```
Authorization: Basic <Base64(username:password)>
Content-Type: text/xml;charset=UTF-8
SOAPAction: "http://invioprescritto.wsdl.dem.sanita.finanze.it/NomeOperazione"
Accept-Encoding: gzip,deflate
User-Agent: Apache-HttpClient/4.1.1 (java 1.5)
```

> **Nota:** Come da documentazione Sogei, per .NET è necessario forzare  
> l'autenticazione nell'override di `GetWebRequest` (qui implementato  
> direttamente nell'`HttpWebRequest` via header `Authorization`).

---

## Note implementative

- **SOAP 1.1** — gli endpoint usano SOAP 1.1 (non WCF/SOAP 1.2)
- **Cifratura dati sensibili** — CF e pincode vengono cifrati automaticamente  
  con `EnvelopedCms` (.NET) se `PathCertificatoSanitel` è configurato  
  (equivalente a `openssl cms -encrypt -recip Sanitel.cer -aes256`)
- **Parsing risposta** — i nodi foglia del Body SOAP vengono estratti  
  automaticamente; i SOAP Fault vengono mappati a `ERRORE_NUMERO/DESCRIZIONE`
- **TLS** — forzato TLS 1.2 minimo (richiesto dagli ambienti sanitari)

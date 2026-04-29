# Interfacciamento esterno DLL (COM/.NET)

Questa guida descrive come usare `ricetta_dematerializzata.dll` da applicazioni esterne.

Nomi pubblici aggiornati:
- **ProgID COM**: `RicettaDematerializzata.Client`
- **Interfaccia COM**: `IRicettaDematerializzataClient`
- **Classe COM**: `RicettaDematerializzataClient`
- **Factory statica**: `RicettaDematerializzataFactory`
- **Wrapper Delphi consigliato**: `TRicettaDematerializzataClient`

## 1) Build

```powershell
dotnet build .\ricetta_dematerializzata\ricetta_dematerializzata.csproj -c Release
```

Output:
- `ricetta_dematerializzata\bin\Release\net48\ricetta_dematerializzata.dll`

## 2) Registrazione COM

Aprire terminale amministratore nella cartella output:

```bat
cd /d C:\Repositories\ricetta-dematerializzata\ricetta_dematerializzata\bin\Release\net48
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm.exe" ricetta_dematerializzata.dll /tlb:ricetta_dematerializzata.tlb /codebase
```

Deregistrazione:

```bat
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm.exe" ricetta_dematerializzata.dll /unregister
```

ProgID COM esposto:
- `RicettaDematerializzata.Client`

## 3) API esposte verso esterno

### Metodo di configurazione principale:

- `Configura(username, password, seriale, ambiente)` — configura credenziali e certificato client SSL tramite seriale del Windows Certificate Store. Se il seriale è vuoto viene usata solo la Basic Auth.

### Metodi di configurazione:

- `ConfiguraAuthorization2F(tokenOrBearer)` — imposta il token Authorization2F (ID-SESSIONE o Bearer)

### Metodi di servizio:

- `Chiama(servizio, parametriInputKV)`
- `ChiamaJson(servizio, parametriInputJson)`
- `OttieniUrl(servizio)`
- `TestConnessione(servizio)`
- `CifraConSanitel(testoPiano)`

### Politica SSL automatica:

| Ambiente | SSL validation |
|----------|---------------|
| Test (0) | Disabilitata automaticamente |
| Produzione (1) | Abilitata automaticamente |

### Politica di autenticazione per servizio:

- **Servizi token (A2F)**: CreateAuth, RevokeAuth, CheckToken → sempre **Basic Auth** (username/password)
- **Servizi prescrittore/erogatore**: tutti gli altri → **SSL con certificato client** (se seriale configurato) + Basic Auth

## 4) Delphi (unit helper)

File helper: `RicettaDematerializzataHelper.pas`  
Nome unit Delphi: `RicettaDematerializzataHelper`

Include:
- costanti servizi (anche A2F `20/21/22`),
- classe `TRicettaDematerializzataClient`,
- utility KV (`KVGet`, `KVSet`, `KVBuild`, `KVSetArrayNode`, `KVSetNode`, ecc.)

### Esempio rapido:

```pascal
uses RicettaDematerializzataHelper;

var
  C: TRicettaDematerializzataClient;
  OutKv: string;
begin
  C := TRicettaDematerializzataClient.Create;
  try
    // Produzione con certificato client dal Certificate Store
    C.Configura('utente', 'password', 'F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D', AMB_PRODUZIONE);

    // Token A2F (Basic Auth automatica)
    OutKv := C.Chiama(SRV_CREATE_AUTH, 'identificativo_tipo=P;identificativo_valore=...');
    C.ConfiguraAuthorization2F(KVGet(OutKv, 'MESSAGGIO'));

    // Servizio prescrittore (SSL automatico)
    OutKv := C.Chiama(SRV_INVIO_PRESCRITTO,
      'pinCode=1234567890;nre=120000000000;cfMedico=PROVAX00X00X000Y');
  finally
    C.Free;
  end;
end;
```

### Esempio in ambiente di test (solo Basic Auth, no certificato):

```pascal
uses RicettaDematerializzataHelper;

var
  C: TRicettaDematerializzataClient;
  OutKv: string;
begin
  C := TRicettaDematerializzataClient.Create;
  try
    // Test senza certificato: seriale vuoto → solo Basic Auth, SSL validation disabilitata
    C.Configura('utente', 'password', '', AMB_TEST);
    OutKv := C.Chiama(SRV_VISUALIZZA_PRESCRITTO,
      'pinCode=1234567890;nre=120000000000;cfMedico=PROVAX00X00X000Y');
  finally
    C.Free;
  end;
end;
```

## 5) Convenzioni input strutturati (array SOAP)

Per nodi ripetibili usare chiavi KV strutturate:

- Prescrittore: `ElencoDettagliPrescrizioni_<indice>_<campo>`
- Erogatore: `ElencoDettagliPrescrInviiErogato_<indice>_<campo>`

Esempio:

```text
ElencoDettagliPrescrizioni=ARRAY;
ElencoDettagliPrescrizioni_1_codProdPrest=891011;
ElencoDettagliPrescrizioni_1_quantita=1
```

## 6) Nota sicurezza e certificato Sanitel

Nel progetto ricetta_dematerializzata:
- `pinCode` e `cfAssistito` vengono cifrati automaticamente con il certificato Sanitel
- Il certificato è hardcodato come: `certificates/SanitelCF.cer` (relativo alla cartella di esecuzione)
- Il certificato deve essere distribuito manualmente accanto all'eseguibile

Non è possibile configurare un percorso diverso per il certificato Sanitel.

## 7) Configurazione certificato SSL client

Il certificato client SSL viene specificato tramite il **seriale** nel Windows Certificate Store:

```csharp
// C#
var client = new RicettaDematerializzataBaseClient();
client.Configura("utente", "password", "F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D", ambiente: 1);
```

```pascal
// Delphi
client.Configura('utente', 'password', 'F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D', AMB_PRODUZIONE);
```

Se il seriale non è impostato (stringa vuota), viene usata solo la Basic Auth.

La validazione SSL del server è automatica:
- **Test**: sempre disabilitata
- **Produzione**: abilitata con il certificato CA nella cartella `certificates/`
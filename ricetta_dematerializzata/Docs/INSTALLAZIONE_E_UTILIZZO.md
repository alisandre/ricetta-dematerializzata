# Installazione e utilizzo della DLL in software esterno

Questo documento spiega come integrare `ricetta_dematerializzata.dll` (già compilata) in un'applicazione esterna (Delphi, Visual Basic, PowerShell, etc.) via **COM Interop**.

---

## Indice

1. [Prerequisiti](#prerequisiti)
2. [Passaggio 1: Preparazione dei file](#passaggio-1-preparazione-dei-file)
3. [Passaggio 2: Registrazione COM](#passaggio-2-registrazione-com)
4. [Passaggio 3: Integrazione nel progetto Delphi](#passaggio-3-integrazione-nel-progetto-delphi)
5. [Passaggio 4: Utilizzo da codice Delphi](#passaggio-4-utilizzo-da-codice-delphi)
6. [Distribuire l'applicazione](#distribuire-lapplicazione)
7. [Risoluzione problemi](#risoluzione-problemi)
8. [Disinstallazione](#disinstallazione)

---

## Prerequisiti

- **Sistema operativo:** Windows Vista o più recente (consigliato Windows 7+)
- **.NET Framework 4.8:** installato sul computer target
- **Permessi amministratore:** per la registrazione COM
- **Editor/IDE:** Delphi 2007+ oppure VB6+ oppure qualsiasi ambiente di sviluppo con supporto COM
- **File distribuito:** `ricetta_dematerializzata.dll` (già compilato)

---

## Passaggio 1: Preparazione dei file

### 1.1 Struttura cartella di installazione

La DLL viene distribuita **già compilata** accanto all'applicazione:

```
C:\Programmi\MiaSoftware\
  ├── MiaSoftware.exe                  (la tua applicazione)
  ├── ricetta_dematerializzata.dll     (DLL COM)
  ├── ricetta_dematerializzata.tlb     (Type Library, generato dopo registrazione)
  └── certificates/
      ├── SanitelCF-2024-2027.cer     (certificato per cifratura)
      └── (altri certificati se necessari)
```

### 1.2 Copia i file nella cartella dell'applicazione

1. Copia il file **`ricetta_dematerializzata.dll`** distribuito nella stessa cartella di `MiaSoftware.exe`

2. Copia la cartella **`certificates/`** con il certificato Sanitel:
   ```
   certificates/
   └── SanitelCF-2024-2027.cer
   ```

3. La **`ricetta_dematerializzata.tlb`** verrà generata automaticamente al momento della registrazione

### 1.3 Verifica i permessi

Assicurati che l'utente che esegue l'applicazione abbia permessi di lettura sulla cartella `certificates/`.

---

## Passaggio 2: Registrazione COM

La DLL deve essere registrata con `regasm.exe` (strumento .NET) affinché il sistema operativo possa trovarla via COM.

### 2.1 Apri il prompt dei comandi come Amministratore

Su Windows 10/11:
1. Premi `Win + S`
2. Scrivi `cmd`
3. Fai clic destro su **Prompt dei comandi** → **Esegui come amministratore**

### 2.2 Naviga alla cartella dell'applicazione

```cmd
cd C:\Programmi\MiaSoftware
```

### 2.3 Esegui la registrazione

```cmd
regasm ricetta_dematerializzata.dll /tlb:ricetta_dematerializzata.tlb /codebase
```

**Spiegazione opzioni:**
- `/tlb:ricetta_dematerializzata.tlb` – Crea la **Type Library** (necessaria per Delphi e VB)
- `/codebase` – Registra il percorso della DLL nel Registry (consigliato per sviluppo)

**Output atteso:**
```
Types exported to 'ricetta_dematerializzata.tlb'
Registry entry 'HKEY_CLASSES_ROOT\CLSID\{...}' created successfully.
Registry entry 'HKEY_CLASSES_ROOT\Interface\{...}' created successfully.
```

### 2.4 Verifica registrazione (facoltativo)

Apri **Registry Editor** (`regedit`) e naviga a:
```
HKEY_CLASSES_ROOT\RicettaDematerializzata.Client
```

Se la chiave esiste, la registrazione è corretta.

---

## Passaggio 3: Integrazione nel progetto Delphi

### 3.1 Importa la Type Library in Delphi

Assumendo **Delphi 2010+:**

1. Apri il tuo progetto Delphi
2. Menu **Project** → **Import Type Library**
3. Scegli il file **`ricetta_dematerializzata.tlb`** (nella cartella dell'applicazione)
4. Scegli **"Generate Unit"** con il nome `ricetta_dematerializzata_TLB`
5. Fai clic **Create Unit**

Delphi genera automaticamente l'unità wrapper con tutte le interfacce COM.

### 3.2 Aggiungi l'unità helper al tuo progetto

Il repository fornisce **`RicettaDematerializzataHelper.pas`** con funzioni utili per:
- Gestione formato chiave=valore
- Wrapper per il client COM
- Costanti servizi (SRV_INVIO_PRESCRITTO, SRV_CREATE_AUTH, etc.)

Copia il file nel tuo progetto:
```
C:\MioProgramma\Delphi\RicettaDematerializzataHelper.pas
```

Nel tuo file `.dpr`:
```pascal
uses
  ...
  ricetta_dematerializzata_TLB,        // Generato da Delphi
  RicettaDematerializzataHelper;       // Dal repository
```

### 3.3 Certificato Sanitel: configurazione automatica

La DLL cerca automaticamente il certificato Sanitel nella cartella **`certificates/`** della stessa directory dove si trova `ricetta_dematerializzata.dll`.

**Non è necessario configurarlo manualmente:**
- La DLL lo legge automaticamente al primo utilizzo
- Se manca, non sarà possibile eseguire operazioni che richiedono cifratura (come token A2F)
- Per test non-critici, puoi omettere il certificato

---

## Passaggio 4: Utilizzo da codice Delphi

### 4.1 Creazione e configurazione del client

```pascal
uses
  RicettaDematerializzataHelper;

var
  Client: TRicettaDematerializzataClient;
begin
  Client := TRicettaDematerializzataClient.Create;
  try
    // Configura: username, password, seriale certificato (opzionale), ambiente
    Client.Configura(
      'PROVAX00X00X000Y',          // Username
      'Salve123',                   // Password
      '',                           // Seriale certificato (vuoto = nessun SSL client)
      AMB_TEST                      // Ambiente (TEST = 1, PRODUZIONE = 0)
    );

    // Esegui un servizio
    var Input := 'nre=120000000000;cfMedico=PROVAX00X00X000Y;pinCode=1234567890';
    var Risultato := Client.Chiama(SRV_VISUALIZZA_PRESCRITTO, Input);

    ShowMessage(Risultato);

  finally
    Client.Free;
  end;
end;
```

### 4.2 Gestione token A2F (esempio)

```pascal
var
  Client: TRicettaDematerializzataClient;
  Token: string;
  CreateInput, CheckInput, InvioInput: string;
begin
  Client := TRicettaDematerializzataClient.Create;
  try
    Client.Configura('PROVAX00X00X000Y', 'Salve123', '', AMB_TEST);

    // 1. Crea token (usa il certificato Sanitel dalla cartella certificates/)
    CreateInput := 'userId=PROVAX00X00X000Y;identificativo=LsQi...;cfUtente=PROVAX00X00X000Y;' +
                   'codRegione=130;codAslAo=202;codSsa=000000;codiceStruttura=;' +
                   'contesto=RICETTA-DEM;applicazione=PRESCRITTORE';
    var CreateResult := Client.Chiama(SRV_CREATE_AUTH, CreateInput);

    // 2. Estrai token dalla risposta
    Token := KVGet(CreateResult, 'MESSAGGIO');  // Ricerca CODICE=token → MESSAGGIO

    // 3. Configura Authorization2F
    Client.ConfiguraAuthorization2F(Token);

    // 4. Chiama servizio con token
    InvioInput := 'pinCode=1234567890;cfMedico1=PROVAX00X00X000Y;...';
    var InvioResult := Client.Chiama(SRV_INVIO_PRESCRITTO, InvioInput);

    ShowMessage(InvioResult);

  finally
    Client.Free;
  end;
end;
```

### 4.3 Formato input/output (chiave=valore)

**Input (prescrittore):**
```
pinCode=1234567890
cfMedico=PROVAX00X00X000Y
codRegione=190
codASLAo=201
codStruttura=201600104
tipoPrescrizione=P
```

**Output (successo):**
```
CODICE_ESITO_INSERIMENTO=0000
NRE=120000000000
```

**Output (errore):**
```
ERRORE_NUMERO=9999
ERRORE_DESCRIZIONE=Pincode non valido
```

---

## Distribuire l'applicazione

### Opzione 1: Packaging con InnoSetup

Esempio di script InnoSetup:

```ini
[Setup]
AppName=Mia Software
AppVersion=1.0.0
DefaultDirName={pf}\MiaSoftware
OutputDir=.\Output

[Files]
Source: "ricetta_dematerializzata.dll"; DestDir: "{app}"
Source: "certificates\SanitelCF-2024-2027.cer"; DestDir: "{app}\certificates"
Source: "MiaSoftware.exe"; DestDir: "{app}"

[Run]
; Registra la DLL
Filename: "{dotnet4.8}\regasm.exe"; Parameters: """{app}\ricetta_dematerializzata.dll"" /tlb /codebase"; Flags: runascurrentuser

[UninstallRun]
; Deregistra la DLL
Filename: "{dotnet4.8}\regasm.exe"; Parameters: /unregister """{app}\ricetta_dematerializzata.dll"""; Flags: runascurrentuser
```

### Opzione 2: Installazione manuale per test

Distribuisci uno script `.bat` agli utenti:

```batch
@echo off
setlocal enabledelayedexpansion

:: Verifica se eseguito come amministratore
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo Questo script deve essere eseguito come AMMINISTRATORE.
    echo Fai clic destro e scegli "Esegui come amministratore".
    pause
    exit /b 1
)

:: Ottieni il percorso dello script
set APP_DIR=%~dp0

:: Verifica che la DLL esista
if not exist "%APP_DIR%ricetta_dematerializzata.dll" (
    echo ERRORE: ricetta_dematerializzata.dll non trovato in: %APP_DIR%
    pause
    exit /b 1
)

:: Verifica che la cartella certificates esista
if not exist "%APP_DIR%certificates" (
    echo ERRORE: cartella certificates non trovato in: %APP_DIR%
    pause
    exit /b 1
)

:: Registra la DLL
cd /d "%APP_DIR%"
for /f "tokens=*" %%i in ('dir /b /s "%WINDIR%\Microsoft.NET\Framework*\regasm.exe" 2^>nul') do (
    echo Registrazione con: %%i
    "%%i" ricetta_dematerializzata.dll /tlb:ricetta_dematerializzata.tlb /codebase
    if !errorlevel! equ 0 (
        echo ✓ Registrazione completata con successo.
        echo.
        echo La DLL è disponibile in: %APP_DIR%
        echo Certificato Sanitel in: %APP_DIR%certificates\
    ) else (
        echo ✗ Errore durante la registrazione.
        pause
        exit /b 1
    )
    goto :done
)
:done

echo.
echo Installazione completata!
echo Puoi ora eseguire la tua applicazione che utilizza la DLL.
pause
```

Salva come `Registra-RicettaDematerializzata.bat` nella stessa cartella della DLL e distribuiscilo agli utenti.

---

## Risoluzione problemi

### Problema 1: "RicettaDematerializzata.Client not found"

**Causa:** La DLL non è registrata correttamente.

**Soluzione:**
```cmd
cd C:\Programmi\MiaSoftware
regasm ricetta_dematerializzata.dll /tlb /codebase
```

### Problema 2: "Cannot find type library"

**Causa:** La TLB non esiste o è in percorso sbagliato.

**Soluzione:**
1. Assicurati che `ricetta_dematerializzata.dll` esista nella cartella
2. Registra di nuovo con:
   ```cmd
   regasm ricetta_dematerializzata.dll /tlb:ricetta_dematerializzata.tlb /codebase
   ```
3. Verifica che `ricetta_dematerializzata.tlb` sia stato generato nella stessa cartella

### Problema 3: ".NET Framework 4.8 not found"

**Causa:** .NET Framework 4.8 non è installato.

**Soluzione:**
1. Scarica da: https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48
2. Installa con privilegi amministratore
3. Riavvia il computer

### Problema 4: "Accesso negato durante registrazione"

**Causa:** Non hai permessi amministratore.

**Soluzione:** Apri il prompt dei comandi con **Esegui come amministratore** (fai clic destro su `cmd` → **Esegui come amministratore**)

### Problema 5: "Connection refused / Service not available"

**Causa:** Il server DEM non è raggiungibile.

**Soluzione:**
1. Verifica di avere internet
2. Controlla l'ambiente configurato (`AMB_TEST` vs `AMB_PRODUZIONE`)
3. Verifica le credenziali (username/password)
4. Se il server è a pagamento, assicurati di avere una sottoscrizione attiva

### Problema 6: "SSL certificate error"

**Causa:** Il certificato client (se configurato) non è valido o manca.

**Soluzione:**
1. Se **non** usi certificato client, passa stringa vuota:
   ```pascal
   Client.Configura(..., '', AMB_TEST);  // Seriale vuoto = Basic Auth solo
   ```
2. Se **usi** certificato client:
   - Importa nel Windows Certificate Store
   - Usa il seriale corretto (es. `S-1A2B3C4D5E6F7G8H`)
   - Assicurati che sia valido e non scaduto

### Problema 7: "Certificato Sanitel non trovato"

**Causa:** La cartella `certificates/` non è nel percorso corretto o il file manca.

**Soluzione:**
1. Verifica che la cartella sia nella stessa directory della DLL:
   ```
   C:\Programmi\MiaSoftware\
     ├── ricetta_dematerializzata.dll
     └── certificates/
         └── SanitelCF-2024-2027.cer
   ```
2. Se il file manca, ottienilo dal repository:
   ```
   ricetta_dematerializzata\certificates\SanitelCF-2024-2027.cer
   ```

---

## Disinstallazione

### Rimozione completa

1. **Deregistra la DLL:**
   ```cmd
   cd C:\Programmi\MiaSoftware
   regasm /unregister ricetta_dematerializzata.dll
   ```

2. **Elimina la cartella dell'applicazione** (se la DLL era locale):
   ```cmd
   rmdir /s /q "C:\Programmi\MiaSoftware"
   ```

3. **Riavvia il computer** (consigliato)

---

## Note di sicurezza

1. **Certificato Sanitel:** non condividere il file `.cer` pubblicamente; contiene la chiave di cifratura
2. **Credenziali:** non hardcodare username/password nel codice sorgente; usa variabili d'ambiente o file di configurazione crittati
3. **HTTPS:** tutte le comunicazioni con il server SAC/DEM devono avvenire su HTTPS (certificato server validato automaticamente)
4. **Token A2F:** salva i token in modo sicuro (non in chiaro su disco); considerare l'uso di Windows Data Protection API (DPAPI)
5. **Cartella certificates:** assicurati che gli utenti non abbiano permessi di scrittura su questa cartella

---

## Contatti e supporto

Per domande o segnalazioni di bug:
- Repository GitHub: https://github.com/alisandre/ricetta-dematerializzata
- Issues: https://github.com/alisandre/ricetta-dematerializzata/issues

---

## Versioni testate

| Sistema | Versione | Stato |
|---------|----------|-------|
| Windows 10 21H2 | .NET 4.8 | ✅ OK |
| Windows 11 | .NET 4.8 | ✅ OK |
| Delphi 2010 | COM Interop | ✅ OK |
| Delphi XE8 | COM Interop | ✅ OK |
| VB6 SP6 | COM Interop | ✅ OK |
| PowerShell 5.0 | COM Interop | ✅ OK |

# Quick Start — Installazione DLL in 5 minuti

La DLL viene distribuita **già compilata**. Se sei fretta e vuoi solo installarla, segui questi 5 passaggi:

---

## ✅ Passo 1: Ottieni i file distribuiti

Assicurati di avere:
- ✅ `ricetta_dematerializzata.dll` (già compilato)
- ✅ `certificates/` con `SanitelCF-2024-2027.cer` dentro

---

## ✅ Passo 2: Copia nella cartella dell'applicazione

```
C:\Programmi\MiaSoftware\
  ├── MiaSoftware.exe
  ├── ricetta_dematerializzata.dll     ← copia qui
  └── certificates/                   ← copia qui
      └── SanitelCF-2024-2027.cer
```

Copia i file nella stessa cartella dove c'è il tuo eseguibile.

---

## ✅ Passo 3: Registra con regasm

Apri **Prompt dei comandi come Amministratore** e digita:

```cmd
cd C:\Programmi\MiaSoftware
regasm ricetta_dematerializzata.dll /tlb /codebase
```

**Output atteso:**
```
Types exported to 'ricetta_dematerializzata.tlb'
Registry entry 'HKEY_CLASSES_ROOT\CLSID\{...}' created successfully.
```

✅ **Fatto!** La DLL è ora disponibile via COM.

---

## ✅ Passo 4: Verifica (opzionale)

In PowerShell, digita:

```powershell
[activator]::CreateInstance([type]::GetTypeFromProgID("RicettaDematerializzata.Client"))
```

Se non genera errore, la registrazione è OK.

---

## ✅ Passo 5: Usa da Delphi (o altro linguaggio)

### Delphi

```pascal
uses RicettaDematerializzataHelper;

var Client: TRicettaDematerializzataClient;
begin
  Client := TRicettaDematerializzataClient.Create;
  try
    Client.Configura('PROVAX00X00X000Y', 'Salve123', '', 1);  // 1=Test
    var Risultato := Client.Chiama(SRV_VISUALIZZA_PRESCRITTO, 'nre=120000000000;cfMedico=PROVAX00X00X000Y;pinCode=1234567890');
    ShowMessage(Risultato);
  finally
    Client.Free;
  end;
end;
```

### VB6

```vb
Dim Client As Object
Set Client = CreateObject("RicettaDematerializzata.Client")
Client.Configura "PROVAX00X00X000Y", "Salve123", "", 1
MsgBox Client.Chiama(1, "nre=120000000000;cfMedico=PROVAX00X00X000Y;pinCode=1234567890")
Set Client = Nothing
```

### PowerShell

```powershell
$client = New-Object -ComObject RicettaDematerializzata.Client
$client.Configura("PROVAX00X00X000Y", "Salve123", "", 1)
$result = $client.Chiama(1, "nre=120000000000;cfMedico=PROVAX00X00X000Y;pinCode=1234567890")
Write-Host $result
```

---

## ⚠️ Errori comuni

| Errore | Causa | Soluzione |
|--------|-------|-----------|
| "RicettaDematerializzata.Client not found" | DLL non registrata | Esegui `regasm` come Amministratore nella cartella giusta |
| "Cannot find type library" | TLB non esiste | Registra di nuovo con `/tlb` |
| ".NET Framework 4.8 not found" | .NET non installato | Installa da https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48 |
| "Accesso negato" | Non hai permessi admin | Apri cmd con **Esegui come amministratore** |
| "Certificato Sanitel non trovato" | Cartella `certificates/` manca | Copia `certificates/` nella stessa cartella della DLL |

---

## 📖 Per approfondire

- **Installazione dettagliata**: vedi `INSTALLAZIONE_E_UTILIZZO.md`
- **API completa**: vedi `INTERFACCIAMENTO_ESTERNO.md`
- **Parametri servizi**: vedi `PARAMETRI_SERVIZI.md`
- **Token A2F**: vedi `A2F_COMPENDIO.md`

---

## 🔐 Nota di sicurezza

**Non** hardcodare le credenziali nel codice! Usa variabili d'ambiente:

```pascal
// ❌ Sbagliato
Client.Configura('PROVAX00X00X000Y', 'Salve123', '', 1);

// ✅ Giusto
var Username := GetEnvironmentVariable('RIC_USERNAME');
var Password := GetEnvironmentVariable('RIC_PASSWORD');
Client.Configura(Username, Password, '', 1);
```

E nel `PATH` o in un file `.env` protetto:
```
RIC_USERNAME=PROVAX00X00X000Y
RIC_PASSWORD=Salve123
```

---

**Fine! La DLL è pronta all'uso.** 🎉

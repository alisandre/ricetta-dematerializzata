# Configurazione certificato SSL per seriale

Questo documento descrive come configurare l'autenticazione SSL del client usando il **seriale del certificato** nel Windows Certificate Store.

## Panoramica

La libreria usa esclusivamente la modalità **serial-based** per il certificato SSL di trasporto:

- Si specifica il seriale esadecimale per recuperare il certificato dal Windows Certificate Store
- Se il seriale non è impostato, viene usata solo la **Basic Auth**
- La validazione SSL del server è **automatica per ambiente**: disabilitata in Test, abilitata in Produzione
- Non è necessario (né supportato) passare il path di un file certificato

**Vantaggi**:
- Nessun file certificato client su disco
- Supporta rotazione automatica dei certificati
- Funziona sia in Test che in Produzione

---

## ⚠️ Nota sulla posizione del certificato Sanitel

Il certificato Sanitel (`SanitelCF.cer`) è hardcodato nel codice della libreria:
- **Posizione**: `certificates/SanitelCF.cer` (relativo alla cartella di esecuzione)
- **Non è configurabile**
- **Necessario per**: cifrare `cfAssistito` e `pinCode` negli input

Assicurati di distribuire il file `SanitelCF.cer` nella cartella `certificates` accanto all'eseguibile.

---

## Uso da C#

### Configurazione con seriale

```csharp
// Metodo diretto con ServiceConfiguration
var config = new ServiceConfiguration
{
    Username              = "utente",
    Password              = "password",
    Ambiente              = ServiceEnvironment.Produzione,
    SerialeCertificatoSsl = "F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D"
};
var client = new RicettaDematerializzataBaseClient(config);
```

```csharp
// Oppure con metodo Configura() dopo istanziazione (es. uso COM)
var client = new RicettaDematerializzataBaseClient();
client.Configura("utente", "password", "F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D", ambiente: 1);
string risultato = client.Chiama((int)DigitalPrescriptionService.InvioPrescritto, "...");
```

### Senza certificato (solo Basic Auth)

```csharp
var client = new RicettaDematerializzataBaseClient();
client.Configura("utente", "password", seriale: "", ambiente: 0); // Test, Basic Auth only
```

---

## Uso da Delphi

### Configurazione con seriale

```pascal
uses RicettaDematerializzataHelper;

var
  client: TRicettaDematerializzataClient;
begin
  client := TRicettaDematerializzataClient.Create;
  try
    client.Configura('utente', 'password', 'F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D', AMB_PRODUZIONE);
    ShowMessage(client.Chiama(SRV_INVIO_PRESCRITTO, 'NRE=123456789;...'));
  finally
    client.Free;
  end;
end;
```

### Senza certificato (solo Basic Auth, test)

```pascal
client.Configura('utente', 'password', '', AMB_TEST);
```

---

## Come trovare il seriale nel Windows Certificate Store

1. Aprire `certmgr.msc` oppure `mmc` → Certificati → Locale → Personali
2. Fare doppio clic sul certificato
3. Scheda **Dettagli** → campo **Numero di serie**
4. Copiare il valore esadecimale (rimuovere gli spazi se presenti)

Esempio seriale: `F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D`

# Configurazione certificato SSL per seriale

Questo documento descrive come configurare l'autenticazione SSL del client usando il **seriale del certificato** nel Windows Certificate Store.

## Panoramica

La libreria usa esclusivamente la modalità **serial-based** per il certificato SSL di trasporto:

- Si specifica il seriale esadecimale per recuperare il certificato dal Windows Certificate Store
- Se il seriale non è impostato, viene usata solo la **Basic Auth**
- La validazione SSL del server è **automatica per ambiente**: disabilitata in Test, abilitata in Produzione

---

## ⚠️ Nota sulla posizione del certificato Sanitel

Il certificato Sanitel (`SanitelCF.cer`) è hardcodato:
- **Posizione**: `certificates/SanitelCF.cer` (relativo alla cartella di esecuzione)
- **Non è configurabile**
- **Necessario per**: cifrare `cfAssistito` e `pinCode`

---

## Uso da C#

```csharp
var client = new RicettaDematerializzataBaseClient();
client.Configura("utente", "password", "F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D", ambiente: 1);
string risultato = client.Chiama((int)DigitalPrescriptionService.InvioPrescritto, "...");
```

Senza certificato (solo Basic Auth):

```csharp
client.Configura("utente", "password", seriale: "", ambiente: 0);
```

---

## Uso da Delphi

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

---

## Come trovare il seriale nel Windows Certificate Store

1. Aprire `certmgr.msc` → Certificati → Locale → Personali
2. Doppio clic sul certificato → Scheda **Dettagli** → **Numero di serie**
3. Copiare il valore esadecimale (rimuovere gli spazi)

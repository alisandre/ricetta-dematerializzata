# Politica di Autenticazione per Servizio

## Sommario

La libreria seleziona automaticamente il metodo di autenticazione in base al servizio chiamato. Non è necessaria alcuna configurazione manuale oltre a `Configura()`.

## Regole di autenticazione

### 1. Servizi di Gestione Token (Authorization 2F)

| Servizio | ID | Autenticazione | SSL client |
|----------|----|----------------|------------|
| CreateAuth | 20 | Basic Auth | No |
| RevokeAuth | 21 | Basic Auth | No |
| CheckToken | 22 | Basic Auth | No |

- Usa username e password forniti in `Configura()`
- Non usa il certificato client SSL

### 2. Servizi Prescrittore

| Servizio | ID | Autenticazione | SSL client | Validazione CA |
|----------|----|----------------|------------|----------------|
| VisualizzaPrescritto | 1 | Basic Auth + SSL | Sì (se seriale) | Sì (solo prod) |
| InvioPrescritto | 2 | Basic Auth + SSL | Sì (se seriale) | Sì (solo prod) |
| AnnullaPrescritto | 3 | Basic Auth + SSL | Sì (se seriale) | Sì (solo prod) |
| InterrogaNreUtilizzati | 4 | Basic Auth + SSL | Sì (se seriale) | Sì (solo prod) |
| ServiceAnagPrescrittore | 5 | Basic Auth + SSL | Sì (se seriale) | Sì (solo prod) |
| InvioDichiarazioneSostituzioneMedico | 6 | Basic Auth + SSL | Sì (se seriale) | Sì (solo prod) |

### 3. Servizi Erogatore

| Servizio | ID | Autenticazione | SSL client | Validazione CA |
|----------|----|----------------|------------|----------------|
| InvioErogato | 10 | Basic Auth + SSL | Sì (se seriale) | No |
| VisualizzaErogato | 11 | Basic Auth + SSL | Sì (se seriale) | No |
| SospendiErogato | 12 | Basic Auth + SSL | Sì (se seriale) | No |
| AnnullaErogato | 13 | Basic Auth + SSL | Sì (se seriale) | No |
| RicercaErogatore | 14 | Basic Auth + SSL | Sì (se seriale) | No |
| ReportErogatoMensile | 15 | Basic Auth + SSL | Sì (se seriale) | No |
| ServiceAnagErogatore | 16 | Basic Auth + SSL | Sì (se seriale) | No |
| RicettaDifferita | 17 | Basic Auth + SSL | Sì (se seriale) | No |
| AnnullaErogatoDiff | 18 | Basic Auth + SSL | Sì (se seriale) | No |
| RicevuteSac | 19 | Basic Auth + SSL | Sì (se seriale) | No |

---

## Configurazione

```csharp
// C# — metodo principale
client.Configura(
    username: "MIO_UTENTE",
    password: "MIA_PASSWORD",
    seriale: "F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D",  // vuoto = solo Basic Auth
    ambiente: 1   // 0=Test, 1=Produzione
);
```

```pascal
// Delphi — metodo principale
client.Configura('MIO_UTENTE', 'MIA_PASSWORD', 'F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D', AMB_PRODUZIONE);
```

**SSL automatico per ambiente**:
- **Test**: validazione SSL server disabilitata automaticamente
- **Produzione**: validazione SSL server abilitata con CA in `certificates/`

---

## Cifratura dei dati sensibili

Indipendentemente dal tipo di autenticazione, i campi sensibili vengono cifrati automaticamente:

| Campo | Metodo |
|-------|--------|
| `cfAssistito` | CMS/PKCS#7 con certificato Sanitel |
| `pinCode` | CMS/PKCS#7 con certificato Sanitel |

Il certificato Sanitel è hardcodato in `certificates/SanitelCF.cer`.

---

## Flusso di una chiamata

```text
client.Chiama(SRV_INVIO_PRESCRITTO, "pinCode=...;nre=...")
  1. Identifica servizio → prescrittore
  2. Cifra campi sensibili (pinCode, cfAssistito) con Sanitel
  3. Carica certificato client da Windows Certificate Store (se seriale impostato)
  4. Prepara richiesta HTTPS con Basic Auth + eventuale certificato client
  5. In produzione: valida CA del server (solo prescrittore)
  6. Invia SOAP → restituisce risposta key=value


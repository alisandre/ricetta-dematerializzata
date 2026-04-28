# 💡 ESEMPIO PRATICO - Come Usare CreateAuth 2.0

## Scenario: Autenticazione 2FA per Prescrittore

### Step 1: Preparare i Dati

```csharp
// Dati dell'utente prescrittore
string userId = "PROVAX00X00X000Y";
string cfUtente = "PROVAX00X00X000Y";
string pinCode = "1234";  // ← Verrà cifrato automaticamente
string codRegione = "130";
string codAslAo = "202";
string codSsa = "000000";
string contesto = "RICETTA-DEM";
string applicazione = "PRESCRITTORE";
```

### Step 2: Configurare il Client

```csharp
using ricetta_dematerializzata_dll.Services;
using ricetta_dematerializzata_dll.Models;

// Configurazione per ambiente Test
var config = new ServiceConfiguration
{
    Username = "utente_test",
    Password = "password_test",
    Ambiente = ServiceEnvironment.Test,
    PathCertificatoSanitel = @"C:\Certificati\SanitelCF-2024-2027.cer"
};

// Creare il client
var client = new PrescriptionClient(config);
```

### Step 3: Preparare i Parametri di Input

```csharp
// Metodo 1: Forma ESPLICITA (consigliata per chiarezza)
string input = $@"
userId={userId};
identificativo_tipo=P;
identificativo_valore={pinCode};
cfUtente={cfUtente};
codRegione={codRegione};
codAslAo={codAslAo};
codSsa={codSsa};
codiceStruttura=;
contesto={contesto};
applicazione={applicazione}";

// Metodo 2: Forma SEMPLIFICATA (con alias)
string inputAlternativo = $@"
USERID={userId};
IDENTIFICATIVO_TIPO=P;
IDENTIFICATIVO_VALORE={pinCode};
CFUTENTE={cfUtente};
CODREGIONE={codRegione};
CODASLAO={codAslAo};
CODSSA={codSsa};
CONTESTO={contesto};
APPLICAZIONE={applicazione}";

// Metodo 3: Forma MINIMA (identificativo semplice, espanso automaticamente)
string inputMinimo = $@"
userId={userId};
identificativo={pinCode};
cfUtente={cfUtente};
codRegione={codRegione};
codAslAo={codAslAo};
codSsa={codSsa};
contesto={contesto};
applicazione={applicazione}";
```

### Step 4: Chiamare il Servizio

```csharp
try
{
    // Chiama CreateAuth
    string output = client.Chiama(DigitalPrescriptionService.CreateAuth, input);

    // Analizza il risultato (formato KV)
    // Output format: "CHIAVE=VALORE;CHIAVE2=VALORE2;..."
    var result = ParserKV.Parse(output);

    if (result.TryGetValue("codEsito", out var codEsito) && codEsito == "0000")
    {
        // ✅ Successo
        result.TryGetValue("token", out var token);
        result.TryGetValue("info", out var info);

        Console.WriteLine($"✅ Autenticazione riuscita!");
        Console.WriteLine($"Token: {token}");
        Console.WriteLine($"Info: {info}");
    }
    else
    {
        // ❌ Errore
        result.TryGetValue("ERRORE_NUMERO", out var errore);
        result.TryGetValue("ERRORE_DESCRIZIONE", out var descrizione);

        Console.WriteLine($"❌ Errore: {errore} - {descrizione}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Eccezione: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}
```

### Step 5: Consultare i Log SOAP

```
📁 bin/Debug/Logs/SOAP/

01_Request_ChiamaServizio_20260428_143522_123.xml
   ↓
   <?xml version="1.0" encoding="UTF-8"?>
   <soapenv:Envelope ...>
     <soapenv:Body>
       <tns:CreateAuthReq>
         <userId>PROVAX00X00X000Y</userId>
         <identificativo>
           <tipo>P</tipo>
           <valore>LsQiYtf7FcpMYV...</valore>
         </identificativo>
         ...
       </tns:CreateAuthReq>
     </soapenv:Body>
   </soapenv:Envelope>

02_Response_ChiamaServizio_20260428_143522_245.xml
   ↓
   <?xml version="1.0" encoding="UTF-8"?>
   <soapenv:Envelope ...>
     <soapenv:Body>
       <tns:CreateAuthRes>
         <codEsito>0000</codEsito>
         <token>...</token>
       </tns:CreateAuthRes>
     </soapenv:Body>
   </soapenv:Envelope>
```

---

## Flusso Completo Passo per Passo

```
┌─────────────────────────────────────────────────────────────┐
│ 1. INPUT PARAMETRI (KV)                                      │
│    userId=PROVAX00X00X000Y;contesto=RICETTA-DEM;...          │
└─────────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────────┐
│ 2. PARSING (ParserKV.Parse)                                  │
│    Dictionary<string, string> = { "userId": "...", ... }     │
└─────────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────────┐
│ 3. NORMALIZZAZIONE (InputMapperService.NormalizeAndValidate) │
│    - Alias mappati a nomi canonici                           │
│    - Validazione parametri obbligatori (contesto)            │
└─────────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────────┐
│ 4. ESPANSIONE (EspandiIdentificativo) - Solo CreateAuth     │
│    identificativo → identificativo_tipo + identificativo_valore │
└─────────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────────┐
│ 5. CIFRATURA (CifraParametriSensibili)                       │
│    - PIN cifrato con certificato Sanitel (opzionale)         │
│    - CF cifrato con certificato Sanitel (opzionale)          │
└─────────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────────┐
│ 6. COSTRUZIONE SOAP (SoapHelper.BuildSoapEnvelope)           │
│    <soapenv:Envelope>                                        │
│      <soapenv:Body>                                          │
│        <tns:CreateAuthReq>                                   │
│          <userId>...</userId>                                │
│          <identificativo>                                    │
│            <tipo>P</tipo>                                    │
│            <valore>...</valore>                              │
│          </identificativo>                                   │
│          ...                                                 │
│        </tns:CreateAuthReq>                                  │
│      </soapenv:Body>                                         │
│    </soapenv:Envelope>                                       │
└─────────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────────┐
│ 7. LOGGING (SoapServiceInterceptor.LogSoapRequest)           │
│    📝 Console output                                         │
│    📁 File XML in Logs/SOAP/                                 │
│    ⏱️ Timestamp                                              │
└─────────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────────┐
│ 8. HTTP POST (SoapHttpClient.ChiamaServizio)                 │
│    URL: https://...                                          │
│    Headers: Authorization, SOAPAction, Content-Type          │
│    Body: SOAP Envelope                                       │
└─────────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────────┐
│ 9. RESPONSE LOGGING (SoapServiceInterceptor.LogSoapResponse) │
│    📝 Console output                                         │
│    📁 File XML in Logs/SOAP/                                 │
│    ⏱️ Tempo elaborazione (ms)                               │
└─────────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────────┐
│ 10. PARSING RISPOSTA (SoapHelper.ParseSoapResponse)          │
│    XML → Dictionary<string, string>                          │
│    Estrae noduli foglia (ricorsivamente)                     │
└─────────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────────┐
│ 11. OUTPUT (ParserKV.Build)                                  │
│    "codEsito=0000;token=...;info=..." oppure                 │
│    "ERRORE_NUMERO=9999;ERRORE_DESCRIZIONE=..."              │
└─────────────────────────────────────────────────────────────┘
```

---

## Codice Completo (Copia-Incolla)

```csharp
using System;
using ricetta_dematerializzata_dll.Services;
using ricetta_dematerializzata_dll.Models;
using ricetta_dematerializzata_dll.Core;

class Program
{
    static void Main()
    {
        try
        {
            // 1. Configurazione
            var config = new ServiceConfiguration
            {
                Username = "utente_test",
                Password = "password_test",
                Ambiente = ServiceEnvironment.Test,
                PathCertificatoSanitel = @"C:\Certificati\SanitelCF-2024-2027.cer"
            };

            var client = new PrescriptionClient(config);

            // 2. Parametri
            string input = @"
userId=PROVAX00X00X000Y;
identificativo_tipo=P;
identificativo_valore=LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=;
cfUtente=PROVAX00X00X000Y;
codRegione=130;
codAslAo=202;
codSsa=000000;
codiceStruttura=;
contesto=RICETTA-DEM;
applicazione=PRESCRITTORE";

            // 3. Chiamata
            Console.WriteLine("🚀 Calling CreateAuth...\n");
            string output = client.Chiama(DigitalPrescriptionService.CreateAuth, input);

            // 4. Analisi risultato
            Console.WriteLine("\n📋 Output (KV format):");
            Console.WriteLine(output);

            var result = ParserKV.Parse(output);

            if (result.TryGetValue("codEsito", out var codEsito) && codEsito == "0000")
            {
                Console.WriteLine("\n✅ SUCCESS!");
                if (result.TryGetValue("token", out var token))
                    Console.WriteLine($"   Token: {token}");
            }
            else
            {
                Console.WriteLine("\n❌ ERROR");
                if (result.TryGetValue("ERRORE_NUMERO", out var num))
                    Console.WriteLine($"   Code: {num}");
                if (result.TryGetValue("ERRORE_DESCRIZIONE", out var desc))
                    Console.WriteLine($"   Message: {desc}");
            }

            // 5. SOAP files
            Console.WriteLine("\n📁 SOAP files saved in: bin/Debug/Logs/SOAP/");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}
```

---

## 🎯 Verifiche Pre-Test

Checklist prima di testare su SoapUI:

- [ ] Certificato `SanitelCF-2024-2027.cer` disponibile in `bin/Debug/certificates/`
- [ ] Credenziali Basic (username/password) valide
- [ ] PIN e CF cifrati correttamente con OpenSSL/CMS
- [ ] Contesto = "RICETTA-DEM" impostato
- [ ] URL endpoint configurato correttamente (Test vs Prod)
- [ ] Authorization2F header impostato (obbligatorio in Prod)
- [ ] TLS 1.2+ abilitato

---

## 📞 Troubleshooting

### Errore: "Parametri obbligatori mancanti: contesto"
```
Soluzione: Aggiungere contesto=RICETTA-DEM ai parametri di input
```

### Errore: "Certificato Sanitel non trovato"
```
Soluzione: Distribuire SanitelCF-2024-2027.cer in bin/Debug/certificates/
           oppure configurare PathCertificatoSanitel
```

### SOAP file non generato
```
Soluzione: Verificare che la directory Logs/SOAP/ sia scrivibile
           Controllare i log della console per errori I/O
```

### Risposta "000" (Errore generico)
```
Soluzione: Controllare il file Logs/SOAP/02_Response_*.xml
           Verificare i dettagli dell'errore nella risposta SOAP
```

---

**🎉 Pronto per testare CreateAuth 2.0!**

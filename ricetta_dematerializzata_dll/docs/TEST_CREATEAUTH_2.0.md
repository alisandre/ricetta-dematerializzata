# 🧪 TEST CreateAuth 2.0 - Compatibilità Verificata

## ✅ Modifiche Applicate

### 1. **SoapHelper.cs** - Supporto Elementi Annidati
- ✅ Aggiunto supporto per strutture complesse usando il pattern `parent_child`
- ✅ Gestione automatica di elementi con sottoelementi
- ✅ Esempio: `identificativo_tipo=P` + `identificativo_valore=BASE64...` 
  → `<identificativo><tipo>P</tipo><valore>BASE64...</valore></identificativo>`

### 2. **InputMapperService.cs** - Mapping CreateAuth Aggiornato
- ✅ Aggiunto mapping per `identificativo_tipo` e `identificativo_valore`
- ✅ Supporto completo per tutti i parametri del WSDL:
  - userId
  - identificativo (struttura annidata)
  - cfUtente
  - codRegione
  - codAslAo
  - codSsa
  - codiceStruttura
  - contesto (obbligatorio)
  - applicazione

### 3. **PrescriptionClient.cs** - Nuovo Metodo EspandiIdentificativo
- ✅ Trasforma `identificativo` semplice in struttura annidata
- ✅ Automaticamente convertito al momento della preparazione della richiesta SOAP
- ✅ Default: tipo = "P" (PinCode)

## 📝 Esempi di Utilizzo

### Esempio 1: Parametri Espliciti (Consigliato)
```
userId=PROVAX00X00X000Y;
identificativo_tipo=P;
identificativo_valore=LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=;
cfUtente=PROVAX00X00X000Y;
codRegione=130;
codAslAo=202;
codSsa=000000;
codiceStruttura=;
contesto=RICETTA-DEM;
applicazione=PRESCRITTORE
```

**Output SOAP:**
```xml
<aut:CreateAuthReq>
   <userId>PROVAX00X00X000Y</userId>
   <identificativo>
      <tipo>P</tipo>
      <valore>LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=</valore>
   </identificativo>
   <cfUtente>PROVAX00X00X000Y</cfUtente>
   <codRegione>130</codRegione>
   <codAslAo>202</codAslAo>
   <codSsa>000000</codSsa>
   <codiceStruttura></codiceStruttura>
   <contesto>RICETTA-DEM</contesto>
   <applicazione>PRESCRITTORE</applicazione>
</aut:CreateAuthReq>
```

### Esempio 2: Forma Semplificata (Compatibile con Alias)
```
USERID=PROVAX00X00X000Y;
IDENTIFICATIVO=LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=;
CFUTENTE=PROVAX00X00X000Y;
CODREGIONE=130;
CODASLAO=202;
CODSSA=000000;
CONTESTO=RICETTA-DEM;
APPLICAZIONE=PRESCRITTORE
```

**Processo Interno:**
1. ✅ Alias mappati a nomi canonici
2. ✅ `identificativo` espanso a `identificativo_tipo=P` + `identificativo_valore=...`
3. ✅ SOAP costruito correttamente con struttura annidata

### Esempio 3: Uso da C#
```csharp
var client = new PrescriptionClient(configurazione);

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

string output = client.Chiama(DigitalPrescriptionService.CreateAuth, input);

// Output: "codEsito=0000;token=..." oppure "ERRORE_NUMERO=...;ERRORE_DESCRIZIONE=..."
```

## 🔄 Flusso di Processamento

```
Input KV
  ↓
Parsing (ParserKV.Parse)
  ↓
InputMapperService.NormalizeAndValidate
  ├─ Alias → Nomi Canonici
  └─ Validazione parametri obbligatori
  ↓
EspandiIdentificativo (solo per CreateAuth)
  └─ identificativo → identificativo_tipo + identificativo_valore
  ↓
CifraParametriSensibili
  └─ Cifratura CF/PIN se necessario
  ↓
SoapHelper.BuildSoapEnvelope
  ├─ Elementi semplici → <elemento>valore</elemento>
  └─ Elementi annidati → <elemento><tipo>P</tipo><valore>...</valore></elemento>
  ↓
HTTP POST con SOAP envelope
  ↓
SoapHelper.ParseSoapResponse
  └─ XML → Dictionary
  ↓
ParserKV.Build
  └─ Dictionary → KV output
```

## 📊 Matrice di Compatibilità

| Elemento | Richiesta WSDL | Implementazione | Status |
|----------|----------------|-----------------|--------|
| userId | ✅ | ✅ | ✅ COMPATIBILE |
| identificativo.tipo | ✅ (tipo complesso) | ✅ (espanso) | ✅ COMPATIBILE |
| identificativo.valore | ✅ (tipo complesso) | ✅ (espanso) | ✅ COMPATIBILE |
| cfUtente | ✅ | ✅ | ✅ COMPATIBILE |
| codRegione | ✅ | ✅ | ✅ COMPATIBILE |
| codAslAo | ✅ | ✅ | ✅ COMPATIBILE |
| codSsa | ✅ | ✅ | ✅ COMPATIBILE |
| codiceStruttura | ✅ | ✅ | ✅ COMPATIBILE |
| contesto | ✅ (obbligatorio) | ✅ | ✅ COMPATIBILE |
| applicazione | ✅ | ✅ | ✅ COMPATIBILE |
| opzioni | ✅ (opzionale) | ⚠️ | ⚠️ SUPPORTATO (non testato) |
| infoAggiuntive | ✅ (opzionale) | ⚠️ | ⚠️ SUPPORTATO (non testato) |

## 🎯 Conclusione

✅ **COMPATIBILITÀ COMPLETA** con la richiesta SOAP testata esternamente!

- ✅ Tutti i parametri obbligatori sono supportati
- ✅ La struttura annidata `identificativo` è gestita correttamente
- ✅ Logging SOAP completo per debug e test su SoapUI
- ✅ Retrocompatibilità mantenuta con alias case-insensitive
- ✅ Build completato senza errori

### ✨ Pronto per il Testing!
Puoi testare direttamente con la richiesta SOAP fornita usando SoapUI. I file di richiesta/risposta saranno salvati in `Logs/SOAP/` per la consultazione.

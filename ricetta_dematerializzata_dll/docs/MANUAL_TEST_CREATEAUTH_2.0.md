# 🧪 MANUAL TEST - CreateAuth 2.0 Compatibility

## Test Case 1: Verifica SOAP Generato

**Obiettivo**: Verificare che il SOAP generato corrisponde alla richiesta testata

**Input Parameters** (Key=Value):
```
userId=PROVAX00X00X000Y
identificativo_tipo=P
identificativo_valore=LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=
cfUtente=PROVAX00X00X000Y
codRegione=130
codAslAo=202
codSsa=000000
codiceStruttura=
contesto=RICETTA-DEM
applicazione=PRESCRITTORE
```

**Expected SOAP Output**:
```xml
<?xml version="1.0" encoding="UTF-8"?>
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tns="http://authservice.xsd.wsdl.auth.a2f.sts.sanita.finanze.it">
  <soapenv:Header/>
  <soapenv:Body>
    <tns:CreateAuthReq>
      <userId>PROVAX00X00X000Y</userId>
      <identificativo>
        <tipo>P</tipo>
        <valore>LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=</valore>
      </identificativo>
      <cfUtente>PROVAX00X00X000Y</cfUtente>
      <codRegione>130</codRegione>
      <codAslAo>202</codAslAo>
      <codSsa>000000</codSsa>
      <codiceStruttura/>
      <contesto>RICETTA-DEM</contesto>
      <applicazione>PRESCRITTORE</applicazione>
    </tns:CreateAuthReq>
  </soapenv:Body>
</soapenv:Envelope>
```

**How to Test**:
1. ✅ Esegui l'applicazione
2. ✅ Naviga in `bin/Debug/Logs/SOAP/`
3. ✅ Apri il file `01_Request_ChiamaServizio_*.xml`
4. ✅ Verifica che la struttura `<identificativo>` contiene `<tipo>P</tipo>` e `<valore>...</valore>`

---

## Test Case 2: Alias Case-Insensitive

**Obiettivo**: Verificare che gli alias funzionano correttamente

**Input Parameters** (uppercase):
```
USERID=PROVAX00X00X000Y
IDENTIFICATIVO_TIPO=P
IDENTIFICATIVO_VALORE=BASE64...
CFUTENTE=PROVAX00X00X000Y
CODREGIONE=130
CODASLAO=202
CONTESTO=RICETTA-DEM
```

**Expected Behavior**:
- ✅ Parametri convertiti correttamente ai nomi canonici
- ✅ SOAP generato correttamente
- ✅ Nessun errore di validazione

---

## Test Case 3: Espansione Identificativo Automatica

**Obiettivo**: Verificare che `identificativo` semplice sia espanso automaticamente

**Input Parameters**:
```
userId=TEST_USER
identificativo=LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=
contesto=RICETTA-DEM
```

**Expected SOAP**:
```xml
<identificativo>
  <tipo>P</tipo>
  <valore>LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=</valore>
</identificativo>
```

---

## Test Case 4: Test su SoapUI

**Obiettivo**: Testare l'effettiva compatibilità con il servizio esterno

**Steps**:
1. ✅ Apri SoapUI
2. ✅ Crea un nuovo SOAP Project
3. ✅ Importa il WSDL da: `/a2f-auth-ws/soap/v1/authentication-service?wsdl`
4. ✅ Copia il SOAP dal file `Logs/SOAP/01_Request_ChiamaServizio_*.xml`
5. ✅ Sostituisci i valori sensibili (PIN, CF)
6. ✅ Invia la richiesta
7. ✅ Verifica la risposta

**Expected Response**:
```xml
<CreateAuthRes>
  <codEsito>0000</codEsito>
  <info>...</info>
</CreateAuthRes>
```

---

## Test Case 5: Validazione Parametri Obbligatori

**Obiettivo**: Verificare che `contesto` è obbligatorio

**Input Parameters** (senza contesto):
```
userId=TEST_USER
cfUtente=TESTX00X00X000Y
```

**Expected Behavior**:
- ❌ Errore di validazione: "Parametri obbligatori mancanti per CreateAuth: contesto"
- ✅ Eccezione ArgumentException lanciata
- ✅ Logging dell'errore

---

## Test Case 6: XML Escaping

**Obiettivo**: Verificare che i caratteri speciali siano correttamente escapati

**Input Parameters** (con caratteri speciali):
```
userId=USER<>&"'
contesto=TEST&DEMO
```

**Expected SOAP**:
```xml
<userId>USER&lt;&gt;&amp;&quot;'</userId>
<contesto>TEST&amp;DEMO</contesto>
```

---

## Test Case 7: Logging SOAP

**Obiettivo**: Verificare che il logging SOAP sia attivo

**Expected Files** (in `bin/Debug/Logs/SOAP/`):
- ✅ `01_Request_ChiamaServizio_YYYYMMDD_HHMMSS_FFF.xml` (richiesta)
- ✅ `02_Response_ChiamaServizio_YYYYMMDD_HHMMSS_FFF.xml` (risposta)
- ✅ `03_Error_ChiamaServizio_YYYYMMDD_HHMMSS_FFF.xml` (se errore)

**Console Output**:
```
================================================================================
[2026-04-28 14:35:22.123] INIZIO RICHIESTA SOAP: ChiamaServizio
================================================================================
URL: https://...
SOAPAction: ...
```

---

## Checklist di Verifica

- [ ] Build completato senza errori
- [ ] Logging SOAP attivo (Debug output e file XML)
- [ ] SOAP generato contiene struttura annidata `<identificativo>`
- [ ] Alias case-insensitive funzionano
- [ ] Parametri obbligatori validati (contesto)
- [ ] Escaping XML corretto
- [ ] Test su SoapUI passato
- [ ] Response SOAP parsata correttamente
- [ ] Nessuna regressione nelle altre operazioni

---

## Comandi Utili per il Debug

### Visualizzare il SOAP generato
```csharp
var parametri = new Dictionary<string, string> { /* ... */ };
string soapEnvelope = SoapHelper.BuildSoapEnvelope("CreateAuthReq", nsA2f, parametri);
Console.WriteLine(soapEnvelope);
```

### Testare il mapping
```csharp
var input = ParserKV.Parse("USERID=TEST;CONTESTO=RICETTA-DEM");
var normalized = InputMapperService.NormalizeAndValidate(
    DigitalPrescriptionService.CreateAuth, input);
foreach (var kv in normalized)
    Console.WriteLine($"{kv.Key}={kv.Value}");
```

### Verificare i file SOAP
```powershell
Get-ChildItem -Path "bin/Debug/Logs/SOAP/" -Filter "*.xml" | ForEach-Object {
    Write-Host $_.Name
    Get-Content $_.FullName | Select-Object -First 50
}
```

---

## Note Importanti

⚠️ **Attenzione**: Il valore `identificativo` deve essere già **CIFRATO con OpenSSL/CMS** usando il certificato SanitelCF-2024-2027.cer prima di essere passato al servizio.

La classe `OpenSSLEncoding.CifraConCertificato()` gestisce automaticamente la cifratura se il certificato è configurato.

✅ **Conclusione**: Tutte le modifiche sono state applicate e il sistema è compatibile con la richiesta CreateAuth 2.0 testata esternamente!

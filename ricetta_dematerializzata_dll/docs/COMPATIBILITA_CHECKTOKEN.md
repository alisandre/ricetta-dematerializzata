# ⚠️ REPORT COMPATIBILITÀ - CheckTokenReq

## 🔍 Richiesta SOAP Fornita

```xml
<aut:CheckTokenReq>
   <aut:userId>PROVAX00X00X000Y</aut:userId>
   <aut:identificativo>
      <dat:tipo>P</dat:tipo>
      <dat:valore>LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=</dat:valore>
   </aut:identificativo>
   <aut:cfUtente>PROVAX00X00X000Y</aut:cfUtente>
   <aut:token>5315d61c-9abc-49d2-b957-e61ad6aa8b3a</aut:token>
   <aut:contesto>RICETTA-DEM</aut:contesto>
   <aut:applicazione>PRESCRITTORE</aut:applicazione>
</aut:CheckTokenReq>
```

---

## ⚠️ RISULTATO: **PARZIALMENTE COMPATIBILE**

### 🔴 PROBLEMI IDENTIFICATI

#### 1. **Mapping Incomplete**
- ❌ `identificativo_tipo` NON è mappato per CheckToken
- ❌ `identificativo_valore` NON è mappato per CheckToken
- ⚠️ Solo CreateAuth e RevokeAuth hanno il mapping completo

**File**: `InputMapperService.cs` Line 153-160
```csharp
[DigitalPrescriptionService.CheckToken] = new(StringComparer.OrdinalIgnoreCase)
{
    ["USERID"] = "userId",
    ["CFUTENTE"] = "cfUtente",
    ["TOKEN"] = "token",                    // ← Presente
    ["CONTESTO"] = "contesto",
    ["APPLICAZIONE"] = "applicazione"
    // ❌ MANCANO: identificativo_tipo, identificativo_valore
}
```

#### 2. **WSDL vs Implementazione**
Il WSDL di CheckTokenReq prevede:
```xml
<xs:complexType name="CheckTokenReq">
    <xs:element name="userId" ... />
    <xs:element name="identificativo" type="dt:identificativo" ... />  ← PRESENTE nel WSDL
    <xs:element name="cfUtente" ... />
    <xs:element name="token" ... />  ← OBBLIGATORIO
    <xs:element name="contesto" ... />
    <xs:element name="applicazione" ... />
</xs:complexType>
```

**Richiesta fornita** include `identificativo` con struttura annidata ✅
**Mapping del codice** NON supporta `identificativo` per CheckToken ❌

---

## 📊 Tabella Compatibilità

| Parametro | WSDL | Richiesta | Mapping | Status |
|-----------|------|-----------|---------|--------|
| userId | ✅ | ✅ | ✅ | ✅ OK |
| identificativo (tipo) | ✅ | ✅ | ❌ | 🔴 MANCANTE |
| identificativo (valore) | ✅ | ✅ | ❌ | 🔴 MANCANTE |
| cfUtente | ✅ | ✅ | ✅ | ✅ OK |
| token | ✅ | ✅ | ✅ | ✅ OK |
| contesto | ✅ | ✅ | ✅ | ✅ OK |
| applicazione | ✅ | ✅ | ✅ | ✅ OK |

**Compatibilità Globale**: 57% (5/7 parametri)

---

## 🔧 Soluzione Richiesta

Aggiornare il mapping di `CheckToken` in `InputMapperService.cs`:

```csharp
[DigitalPrescriptionService.CheckToken] = new(StringComparer.OrdinalIgnoreCase)
{
    ["USERID"] = "userId",
    ["IDENTIFICATIVO_TIPO"] = "identificativo_tipo",          // ← NUOVO
    ["IDENTIFICATIVO_VALORE"] = "identificativo_valore",      // ← NUOVO
    ["CFUTENTE"] = "cfUtente",
    ["TOKEN"] = "token",
    ["CONTESTO"] = "contesto",
    ["APPLICAZIONE"] = "applicazione"
}
```

---

## 🚨 Impatto

Se si tenta di usare CheckToken con `identificativo_tipo` e `identificativo_valore`:

1. ❌ **Errore**: Input normalizzato non conterrà i parametri
2. ❌ **Risultato**: `identificativo_tipo` e `identificativo_valore` non saranno nel SOAP
3. ❌ **SOAP generato**: Mancante `<identificativo>` annidato
4. ❌ **Risposta**: Errore SOAP dal servizio

---

## 💡 Raccomandazione

✅ **Applicare la stessa modifica fatta per CreateAuth anche a CheckToken**:

1. Aggiungere mapping `identificativo_tipo` e `identificativo_valore`
2. ✅ CheckToken supporterà la struttura annidata
3. ✅ RevokeAuth sarà verificato (dovrebbe avere gli stessi parametri)

---

## ✅ Note Positive

- ✅ SoapHelper supporta già elementi annidati (parent_child pattern)
- ✅ EspandiIdentificativo potrebbe essere richiamato anche per CheckToken
- ✅ Nessuna modifica a core logic necessaria
- ✅ Modifica minima e localizzata

---

## 🎯 Azione Richiesta

**File da modificare**: `InputMapperService.cs`
**Linee**: 153-160
**Tipo modifica**: Aggiunta mapping per CheckToken
**Complessità**: Molto bassa
**Tempo stimato**: < 2 minuti


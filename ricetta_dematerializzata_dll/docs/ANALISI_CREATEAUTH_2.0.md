# 🔍 ANALISI COMPATIBILITÀ CreateAuth 2.0

## 📋 Richiesta SOAP Testata (Funzionante)

```xml
<aut:CreateAuthReq>
   <aut:userId>PROVAX00X00X000Y</aut:userId>
   <aut:identificativo>
      <dat:tipo>P</dat:tipo>
      <dat:valore>LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=</dat:valore>
   </aut:identificativo>
   <aut:cfUtente>PROVAX00X00X000Y</aut:cfUtente>
   <aut:codRegione>130</aut:codRegione>
   <aut:codAslAo>202</aut:codAslAo>
   <aut:codSsa>000000</aut:codSsa>
   <aut:codiceStruttura></aut:codiceStruttura>
   <aut:contesto>RICETTA-DEM</aut:contesto>
   <aut:applicazione>PRESCRITTORE</aut:applicazione>
</aut:CreateAuthReq>
```

## 🏗️ Struttura WSDL (da sts-a2f-service-data-type.v0.1.xsd)

```xml
<xs:complexType name="identificativo">
   <xs:sequence>
      <xs:element name="tipo" type="stringType2" minOccurs="1">
         <!-- P=pincode, I=id inviante sogei -->
      </xs:element>
      <xs:element name="valore" type="stringTypeMax256" minOccurs="1">
         <!-- Identificativo cifrato e codificato in base64 -->
      </xs:element>
   </xs:sequence>
</xs:complexType>
```

## ⚠️ PROBLEMI IDENTIFICATI

### 1. **Mapping dell'identificativo**
- ❌ **Attuale**: Non supporta il tipo complesso `identificativo`
- ✅ **Richiesto**: Struttura con `tipo` e `valore` come sottoelementi

### 2. **SoapHelper.BuildSoapEnvelope**
- ❌ **Limitazione**: Attualmente trasforma solo `Dictionary<string, string>` in elementi XML semplici
- ✅ **Necessario**: Supportare elementi con sottoelementi (nested)

### 3. **InputMapperService**
- ⚠️ **Mapping parziale**: Il parametro `identificativo` non è mappato correttamente
- Mappa solo: userId, cfUtente, codRegione, codAslAo, codSsa, codiceStruttura, contesto, applicazione
- **Manca**: Il parametro `identificativo` con la sua struttura annidata

## 📊 Compatibilità Attuale

| Elemento | Status | Nota |
|----------|--------|------|
| userId | ✅ | Mappato correttamente |
| identificativo | ❌ | Tipo complesso non supportato |
| cfUtente | ✅ | Mappato correttamente |
| codRegione | ✅ | Mappato correttamente |
| codAslAo | ✅ | Mappato correttamente |
| codSsa | ✅ | Mappato correttamente |
| codiceStruttura | ✅ | Mappato correttamente |
| contesto | ✅ | Mappato correttamente (obbligatorio) |
| applicazione | ✅ | Mappato correttamente |
| opzioni | ⚠️ | Supportato ma non utilizzato |
| infoAggiuntive | ⚠️ | Supportato ma non utilizzato |

## 🎯 RACCOMANDAZIONI

### Opzione 1: Aggiungere supporto per oggetti complessi nel SoapHelper ✅ CONSIGLIATO
- Modificare `BuildSoapEnvelope` per gestire strutture annidate
- Supportare `Dictionary<string, object>` dove object può essere `Dictionary<string, string>`
- Questo aumenta la flessibilità per futuri servizi

### Opzione 2: Gestire identificativo come caso speciale
- Aggiungere logica ad-hoc in `InputMapperService` 
- Meno elegante ma meno complesso

### Opzione 3: Pre-costruire il SOAP a mano
- Non scalabile e manutenibile

## ✅ CONCLUSIONE

**Il sistema è PARZIALMENTE compatibile** con CreateAuth 2.0:
- ✅ I campi semplici sono supportati
- ❌ Il campo complesso `identificativo` richiede modifiche al `SoapHelper`

**Azione consigliata**: Implementare l'Opzione 1 per una soluzione robusta e riutilizzabile.

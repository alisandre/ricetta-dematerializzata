# 📚 Indice documentazione (consolidato)

Questo indice raccoglie i documenti realmente utili dopo il consolidamento della cartella `docs`.

## File principali

1. **README.md**
   - guida generale libreria,
   - build,
   - note di sicurezza e certificati,
   - utilizzo base.

2. **PARAMETRI_SERVIZI.md**
   - elenco parametri di input di ogni servizio,
   - campi obbligatori,
   - alias supportati,
   - campi strutturati per array SOAP.

3. **INTERFACCIAMENTO_ESTERNO.md**
   - uso esterno DLL da COM/.NET,
   - registrazione `regasm`, ProgID,
   - API esposte (`Configura`, `Chiama`, `ChiamaJson`, ecc.),
   - uso da Delphi e convenzioni input strutturati.

4. **POLITICA_AUTENTICAZIONE.md**
   - selezione automatica autenticazione per servizio,
   - tabelle Basic Auth vs SSL per prescrittore/erogatore/A2F,
   - comportamento SSL automatico per ambiente (Test/Produzione).

5. **CONFIGURAZIONE_CERTIFICATO_SSL_SERIALE.md**
   - configurazione certificato client via seriale nel Windows Certificate Store,
   - comportamento senza certificato (solo Basic Auth),
   - istruzioni per trovare il seriale.

6. **PROCESSO_DELPHI_TOKEN_MEDICO_E_INVIO_PRESCRITTO.md**
   - simulazione processo applicativo Delphi esterno,
   - riuso token medico in memoria,
   - `CheckToken` → `CreateAuth` se necessario,
   - `InvioPrescritto` finale.

7. **A2F_COMPENDIO.md**
   - documento unico per CreateAuth / RevokeAuth / CheckToken,
   - esempi KV,
   - checklist test minima.

8. **INDICE_DOCUMENTAZIONE.md**
   - questo file.

---

## Percorso consigliato per ruolo

### Integrazione esterna DLL
Leggere in ordine:
1. `INTERFACCIAMENTO_ESTERNO.md`
2. `PROCESSO_DELPHI_TOKEN_MEDICO_E_INVIO_PRESCRITTO.md`
3. `README.md`

### Servizi A2F
Leggere:
1. `A2F_COMPENDIO.md`
2. `INTERFACCIAMENTO_ESTERNO.md`

### Parametri input per servizio
Leggere:
1. `PARAMETRI_SERVIZI.md`
2. `README.md`

### Visione generale progetto
Leggere:
1. `README.md`
2. `INDICE_DOCUMENTAZIONE.md`

---

## Temi coperti

- Build .NET Framework 4.8
- Registrazione COM della DLL
- Interfacciamento Delphi esterno (`RicettaDematerializzata.Client`, `IRicettaDematerializzataClient`, `TRicettaDematerializzataClient`)
- Gestione token A2F medico
- Invio prescrizione con `InvioPrescritto`
- Input KV strutturati per nodi array SOAP
- Parametri obbligatori e alias di ogni servizio

---

## Nota

I contenuti precedentemente dispersi in più file A2F sono stati accorpati per ridurre la frammentazione.

---

## Esempi di utilizzo rapido

### Per sviluppatore .NET/C# che vuole integrare la DLL

1. README.md → Architettura generale
2. INTERFACCIAMENTO_ESTERNO.md → API e metodo `Configura()`
3. POLITICA_AUTENTICAZIONE.md → Capire come funziona l'autenticazione
4. CONFIGURAZIONE_CERTIFICATO_SSL_SERIALE.md → Dettagli certificato seriale

**Esempio di utilizzo rapido**:

```csharp
var client = new RicettaDematerializzataBaseClient();
client.Configura("user", "pass", "SERIALE_CERT", 1);  // 1=Produzione
var result = client.Chiama((int)DigitalPrescriptionService.InvioPrescritto, "pinCode=...;nre=...");
```

### Per sviluppatore Delphi che vuole integrare via COM

1. README.md → Architettura generale
2. INTERFACCIAMENTO_ESTERNO.md → Sezione "Delphi (unit helper)"
3. POLITICA_AUTENTICAZIONE.md → Capire autenticazione automatica
4. CONFIGURAZIONE_CERTIFICATO_SSL_SERIALE.md → Come ottenere il seriale

**Esempio di utilizzo rapido**:

```pascal
uses RicettaDematerializzataHelper;
var client: TRicettaDematerializzataClient;
begin
  client := TRicettaDematerializzataClient.Create;
  client.Configura('user', 'pass', 'SERIALE_CERT', AMB_PRODUZIONE);
  ShowMessage(client.Chiama(SRV_INVIO_PRESCRITTO, 'pinCode=...;nre=...'));
  client.Free;
end;

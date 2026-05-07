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

4. **INSTALLAZIONE_E_UTILIZZO.md**
   - installazione della DLL in cartelle software esterno,
   - registrazione COM step-by-step,
   - integrazione in Delphi e altri linguaggi,
   - distribuzione via MSI, InnoSetup, script batch,
   - risoluzione problemi comuni,
   - disinstallazione.

5. **POLITICA_AUTENTICAZIONE.md**
   - selezione automatica autenticazione per servizio,
   - tabelle Basic Auth vs SSL per prescrittore/erogatore/A2F,
   - comportamento SSL automatico per ambiente (Test/Produzione).

6. **CONFIGURAZIONE_CERTIFICATO_SSL_SERIALE.md**
   - configurazione certificato client via seriale nel Windows Certificate Store,
   - comportamento senza certificato (solo Basic Auth),
   - istruzioni per trovare il seriale.

7. **PROCESSO_DELPHI_TOKEN_MEDICO_E_INVIO_PRESCRITTO.md**
   - simulazione processo applicativo Delphi esterno,
   - riuso token medico in memoria,
   - `CheckToken` → `CreateAuth` se necessario,
   - chiamate A2F via `Chiama(20/21/22)`,
   - `InvioPrescritto` finale,
   - flusso erogatore: `VisualizzaErogato` (presa in carico) → `InvioErogato` (conferma erogazione).

8. **A2F_COMPENDIO.md**
   - documento unico per CreateAuth / RevokeAuth / CheckToken,
   - invocazione esterna via `Chiama(20/21/22)`,
   - esempi KV,
   - checklist test minima.

9. **INDICE_DOCUMENTAZIONE.md**
   - questo file.

---

## Percorso consigliato per ruolo

### Integrazione esterna DLL
Leggere in ordine:
1. `INSTALLAZIONE_E_UTILIZZO.md` (installiamo la DLL)
2. `INTERFACCIAMENTO_ESTERNO.md` (API e registrazione COM)
3. `PROCESSO_DELPHI_TOKEN_MEDICO_E_INVIO_PRESCRITTO.md` (flusso applicativo)
4. `README.md` (riferimento generale)

### Servizi A2F
Leggere:
1. `A2F_COMPENDIO.md`
2. `INTERFACCIAMENTO_ESTERNO.md`
3. `INSTALLAZIONE_E_UTILIZZO.md` (per distribuzione dell'app che usa A2F)

### Parametri input per servizio
Leggere:
1. `PARAMETRI_SERVIZI.md`
2. `README.md`

### Distribuire l'applicazione
Leggere:
1. `INSTALLAZIONE_E_UTILIZZO.md` (sezione "Distribuire l'applicazione")
2. `POLITICA_AUTENTICAZIONE.md` (capire autenticazione da documentare agli utenti)

### Visione generale progetto
Leggere:
1. `README.md`
2. `INDICE_DOCUMENTAZIONE.md` (questo file)

---

## Temi coperti

- Build .NET Framework 4.8
- Registrazione COM della DLL via `regasm`
- Installazione in `Program Files` o cartelle locali
- Distribuzione via MSI, InnoSetup o script batch
- Integrazione Delphi esterna (`RicettaDematerializzata.Client`, `IRicettaDematerializzataClient`, `TRicettaDematerializzataClient`)
- Gestione token A2F medico
- Invio prescrizione con `InvioPrescritto`
- Flusso erogatore con presa in carico (`VisualizzaErogato`) e conferma (`InvioErogato`)
- Input KV strutturati per nodi array SOAP
- Parametri obbligatori e alias di ogni servizio
- Risoluzione problemi comuni (certificati, permessi, connessione)

---

## Nota

I contenuti precedentemente dispersi in più file A2F sono stati accorpati per ridurre la frammentazione.

---

## Esempi di utilizzo rapido

### Per sviluppatore .NET/C# che vuole integrare la DLL

0. **PRIMA**: INSTALLAZIONE_E_UTILIZZO.md → Installare e registrare la DLL
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

0. **PRIMA**: INSTALLAZIONE_E_UTILIZZO.md → Installare, registrare la DLL e importare la TLB
1. README.md → Architettura generale
2. INTERFACCIAMENTO_ESTERNO.md → Sezione "Delphi (unit helper)"
3. PROCESSO_DELPHI_TOKEN_MEDICO_E_INVIO_PRESCRITTO.md → Flusso applicativo Delphi
4. POLITICA_AUTENTICAZIONE.md → Capire autenticazione automatica
5. CONFIGURAZIONE_CERTIFICATO_SSL_SERIALE.md → Come ottenere il seriale

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
















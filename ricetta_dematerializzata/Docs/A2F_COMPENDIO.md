# A2F Compendio (CreateAuth / RevokeAuth / CheckToken)

Documento unificato per uso, compatibilità e test dei servizi A2F.

## Stato

- Compatibilità A2F: **verificata**
- Servizi coperti:
  - `CreateAuth` (20)
  - `RevokeAuth` (21)
  - `CheckToken` (22)
- Invocazione esterna supportata:
  - `Chiama(20, ...)`, `Chiama(21, ...)`, `Chiama(22, ...)` (COM/.NET)

## Formato input supportato

La libreria accetta input KV (`chiave=valore;...`) con alias case-insensitive.

Per `identificativo` sono supportate entrambe le forme:

1. **Esplicita**

```text
identificativo_tipo=P;identificativo_valore=<BASE64>
```

2. **Compatta** (auto-espansione)

```text
identificativo=<BASE64>
```

## Esempi rapidi KV

### CreateAuth (20)

```text
userId=PROVAX00X00X000Y;identificativo_tipo=P;identificativo_valore=BASE64;cfUtente=PROVAX00X00X000Y;codRegione=130;codAslAo=202;codSsa=000000;contesto=RICETTA-DEM;applicazione=PRESCRITTORE
```

### RevokeAuth (21)

```text
userId=PROVAX00X00X000Y;identificativo_tipo=P;identificativo_valore=BASE64;cfUtente=PROVAX00X00X000Y;token=<GUID>;contesto=RICETTA-DEM;applicazione=PRESCRITTORE
```

### CheckToken (22)

```text
userId=PROVAX00X00X000Y;identificativo_tipo=P;identificativo_valore=BASE64;cfUtente=PROVAX00X00X000Y;token=<GUID>;contesto=RICETTA-DEM;applicazione=PRESCRITTORE
```

## Esempio invocazione esterna

### Delphi (COM)

```pascal
CreateOut := Client.Chiama(SRV_CREATE_AUTH, BuildCreateAuthInput);
CheckOut  := Client.Chiama(SRV_CHECK_TOKEN, BuildCheckTokenInput(Token));
RevokeOut := Client.Chiama(SRV_REVOKE_AUTH, BuildCheckTokenInput(Token));
```

### .NET

```csharp
var createOut = client.Chiama((int)DigitalPrescriptionService.CreateAuth, inputCreate);
var checkOut  = client.Chiama((int)DigitalPrescriptionService.CheckToken, inputCheck);
var revokeOut = client.Chiama((int)DigitalPrescriptionService.RevokeAuth, inputRevoke);
```

## Output

- Successo: KV con codici/esiti del servizio
- Errore: `ERRORE_NUMERO=...;ERRORE_DESCRIZIONE=...`

## Logging SOAP

Le richieste/risposte possono essere tracciate nei log SOAP della libreria per debug e confronto con SoapUI.

## Checklist test minima

1. Build release eseguita
2. Richiesta `Chiama(20, ...)` con `identificativo_tipo/valore`
3. Richiesta `Chiama(20, ...)` con `identificativo` compatto
4. `Chiama(22, ...)` con token valido/non valido
5. `Chiama(21, ...)` revoke token
6. Verifica output KV e fault mapping

## Note

- In produzione è necessario gestire correttamente `Authorization2F` e certificati.
- Per guida COM/.NET esterna vedi `INTERFACCIAMENTO_ESTERNO.md`.

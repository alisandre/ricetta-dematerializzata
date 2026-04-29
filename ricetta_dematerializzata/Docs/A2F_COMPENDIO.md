# A2F Compendio (CreateAuth / RevokeAuth / CheckToken)

Documento unificato per uso, compatibilità e test dei servizi A2F.

## Stato

- Compatibilità A2F: **verificata**
- Servizi coperti:
  - `CreateAuth` (20)
  - `RevokeAuth` (21)
  - `CheckToken` (22)

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

## Esempi rapidi

### CreateAuth

```text
userId=PROVAX00X00X000Y;identificativo_tipo=P;identificativo_valore=BASE64;cfUtente=PROVAX00X00X000Y;codRegione=130;codAslAo=202;codSsa=000000;contesto=RICETTA-DEM;applicazione=PRESCRITTORE
```

### RevokeAuth

```text
userId=PROVAX00X00X000Y;identificativo_tipo=P;identificativo_valore=BASE64;cfUtente=PROVAX00X00X000Y;token=<GUID>;contesto=RICETTA-DEM;applicazione=PRESCRITTORE
```

### CheckToken

```text
userId=PROVAX00X00X000Y;identificativo_tipo=P;identificativo_valore=BASE64;cfUtente=PROVAX00X00X000Y;token=<GUID>;contesto=RICETTA-DEM;applicazione=PRESCRITTORE
```

## Output

- Successo: KV con codici/esiti del servizio
- Errore: `ERRORE_NUMERO=...;ERRORE_DESCRIZIONE=...`

## Logging SOAP

Le richieste/risposte possono essere tracciate nei log SOAP della libreria per debug e confronto con SoapUI.

## Checklist test minima

1. Build release eseguita
2. Richiesta CreateAuth con `identificativo_tipo/valore`
3. Richiesta CreateAuth con `identificativo` compatto
4. CheckToken con token valido/non valido
5. RevokeAuth token
6. Verifica output KV e fault mapping

## Note

- In produzione è necessario gestire correttamente `Authorization2F` e certificati.
- Per guida COM/.NET esterna vedi `INTERFACCIAMENTO_ESTERNO.md`.

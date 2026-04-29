# Parametri dei servizi

Questo documento elenca i parametri di input supportati dalla libreria per ogni servizio esposto, aggiungendo ove possibile la **descrizione recuperata dagli XSD locali**.

Fonti:
- `InputMapperService.cs`
- XSD in `ricetta_dematerializzata_dll/wsdl/dematerializzata/`
- XSD A2F in `ricetta_dematerializzata_dll/wsdl/a2f/`

Legenda:
- **Obbl.** = richiesto dalla validazione libreria
- **Alias** = chiavi alternative accettate in input KV
- **Descrizione XSD** = testo recuperato dalla documentazione XSD/WSDL locale

---

## Prescrittore

### 1. VisualizzaPrescritto
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `nre` | sì | `NRE` | Numero Ricetta Elettronica |
| `cfMedico` | sì | `CFMEDICO` | CF del medico a cui è associato l'NRE |

---

### 2. InvioPrescritto
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `cfMedico1` | sì | `CFMEDICO1` | Codice fiscale del medico associato all'NRE inviato o attribuito in fase di registrazione |
| `cfMedico2` | no | `CFMEDICO2` | Codice fiscale del medico che compila la ricetta elettronica |
| `codRegione` | sì | `CODREGIONE` | Codice Regione / Provincia Autonoma del medico a cui si vuole attribuire un NRE |
| `codASLAo` | sì | `CODASLAO` | Codice ASL del medico a cui si vuole attribuire un NRE |
| `codStruttura` | no | `CODSTRUTTURA` | Codice struttura del medico a cui si vuole attribuire un NRE |
| `codSpecializzazione` | sì | `CODSPECIALIZZAZIONE` | Specializzazione del medico a cui si vuole attribuire un NRE |
| `codiceAss` | sì | `CODICEASS` | Codice Fiscale/STP/ENI/altro che rappresenta l'assistito per cui viene compilata la ricetta |
| `provAssistito` | no | `PROVASSISTITO`, `PROVINCIAASSISTITO` | Sigla della provincia di residenza dell'assistito |
| `aslAssistito` | no | `ASLASSISTITO` | Codice ASL di residenza dell'assistito |
| `codEsenzione` | no | `CODESENZIONE` | Codice esenzione dell'assistito |
| `nonEsente` | no | `NONESENTE` | Indica se l'assistito è non esente (es. `1`) |
| `reddito` | no | `REDDITO` | Indica se l'assistito è esente per reddito |
| `ricettaInterna` | no | `RICETTAINTERNA` | Indica se si tratta di una ricetta interna |
| `codDiagnosi` | no | `CODDIAGNOSI` | Codice diagnosi ICD |
| `descrizioneDiagnosi` | no | `DESCRDIAGNOSI`, `DESCRIZIONEDIAGNOSI` | Descrizione testuale della diagnosi |
| `tipoPrescrizione` | sì | `TIPOPRESCRIZIONE` | F = Farmaceutica, P = Specialistica |
| `dataCompilazione` | sì | `DATACOMPILAZIONE` | Data compilazione della ricetta, formato aaaa-mm-gg hh24:mm:ss |
| `tipoVisita` | sì | `TIPOVISITA` | A = Ambulatoriale, D = Domiciliare |
| `classePriorita` | no | `CLASSEPRIORITA`, `PRIORITA` | Classe di priorità: U, B, D, P (obbligatoria per primo accesso specialistica) |
| `ElencoDettagliPrescrizioni` | sì | `ELENCODETTAGLIPRESCRIZIONI` | Elenco singole prescrizioni |
| `nre` | no | `NRE` | Numero di ricetta elettronica (NRE) |

**Campi figli supportati in `ElencoDettagliPrescrizioni`**

| Campo figlio | Descrizione XSD |
|---|---|
| `codProdPrest` | Codice prodotto farmaceutico (AIC) o codice prestazione specialistica |
| `descrProdPrest` | Descrizione testuale del prodotto farmaceutico o della prestazione specialistica |
| `codGruppoEquival` | Codice del gruppo equivalente |
| `descrGruppoEquival` | Descrizione testuale del gruppo equivalente |
| `testoLibero` | Campo che indica di tenere conto solamente della descrizione inserita in descrTestoLibero |
| `descrTestoLiberoNote` | Descrizione testuale della prestazione specialistica |
| `nonSost` | Non sostituibilità del farmaco |
| `motivazNote` | Motivazione della non sostituibilità |
| `codMotivazione` | Codici di motivazione di non sostituibilità di un farmaco |
| `notaProd` | Nota AIFA |
| `quantita` | Quantità di confezioni o di prestazioni specialistiche prescritte |
| `prescrizione1` | Campo destinato a informazioni aggiuntive |
| `prescrizione2` | Campo destinato a informazioni aggiuntive |
| `codCatalogoPrescr` | Codice del catalogo regionale della prestazione prescritta |
| `tipoAccesso` | Tipologia di accesso |
| `numeroNota` | DPCM 12 gen 2017 |
| `condErogabilita` | DPCM 12 gen 2017 |
| `approprPrescrittiva` | DPCM 12 gen 2017 |
| `patologia` | DPCM 12 gen 2017 |
| `numsedute` | DPCM 12 gen 2017 |

**Altri campi presenti nell'XSD ma non normalizzati esplicitamente in `InputMapperService`**
- `testata1`, `testata2`, `tipoRic`, `cognNome`, `indirizzo`, `oscuramDati`, `numTessSasn`, `socNavigaz`, `dispReg`, `indicazionePrescr`, `altro`, `statoEstero`, `istituzCompetente`, `numIdentPers`, `numIdentTess`, `dataNascitaEstero`, `dataScadTessera`.

---

### 3. AnnullaPrescritto
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `nre` | sì | `NRE` | Numero Ricetta Elettronica |
| `cfMedico` | sì | `CFMEDICO` | CF del medico a cui è associato l'NRE |

---

### 4. InterrogaNreUtilizzati
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `codRegione` | sì | `CODREGIONE` | Codice della Regione/P.A. |
| `cfMedico` | sì | `CFMEDICO` | Codice fiscale di un medico prescrittore |
| `nre` | no | `NRE` | Singolo Numero Ricetta Elettronica |

**Altri campi presenti nell'XSD ma non normalizzati esplicitamente**
- `codLotto` = Numero lotto
- `cfAssistito` = Codice fiscale dell'assistito
- `tipoPrescr` = documentazione XSD presente ma descrizione generica
- `dataCompilazioneRicettaDal` = Data compilazione ricetta “DAL” nel formato aaaa-mm-gg
- `dataCompilazioneRicettaAl` = Data compilazione ricetta “AL” nel formato aaaa-mm-gg

---

### 5. ServiceAnagPrescrittore
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `tipoOperazione` | sì | `TIPOOPERAZIONE` | Operazione da eseguire |

**Altri campi presenti nell'XSD ma non normalizzati esplicitamente**
- `codiceRegione` = Codice regione
- `nomeSWH` = Nome della software house
- `mailSWH` = Email della software house
- `opzione1`..`opzione5` = Campo opzionale

---

### 6. InvioDichiarazioneSostituzioneMedico
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `pwd` | sì | `PWD` | Campo a disposizione dei SAR |
| `cfMedicoTitolare` | sì | `CFMEDICOTITOLARE` | Codice fiscale del medico titolare |
| `codRegione` | sì | `CODREGIONE` | Codice Regione / Provincia Autonoma del medico titolare |
| `codASLAo` | sì | `CODASLAO` | Codice ASL del medico titolare |
| `codSpecializzazione` | sì | `CODSPECIALIZZAZIONE` | Specializzazione del medico titolare |
| `cfMedicoSostituto` | sì | `CFMEDICOSOSTITUTO` | Codice fiscale del medico sostituto |
| `dataInizioSostituzione` | sì | `DATAINIZIOSOSTITUZIONE` | Data di inizio sostituzione |
| `dataFineSostituzione` | sì | `DATAFINESOSTITUZIONE` | Data di fine sostituzione |

**Altri campi presenti nell'XSD ma non normalizzati esplicitamente**
- `codStruttura` = Codice struttura del medico titolare
- `comunicazioneAsl` = Indica se la sostituzione è stata comunicata alla ASL: SI/NO
- `nota` = Note
- `opzioni` = Elenco campi opzionali

---

## Erogatore

### 10. InvioErogato
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `codiceRegioneErogatore` | sì | `CODICEREGIONEEROGATORE` | Codice regione della struttura erogatrice |
| `codiceAslErogatore` | sì | `CODICEASLEROGATORE` | Codice ASL della struttura erogatrice |
| `codiceSsaErogatore` | sì | `CODICESSAEROGATORE` | Codice SSA della struttura erogatrice |
| `nre` | sì | `NRE` | Numero Ricetta Elettronica |
| `tipoOperazione` | sì | `TIPOOPERAZIONE` | 1 = Erogazione totale, 2 = Erogazione parziale, 3 = Erogazione parziale con chiusura forzata, 4 = Erogazione differita totale, 5 = Erogazione differita con chiusura forzata |
| `dataSpedizione` | sì | `DATASPEDIZIONE` | Data di spedizione (erogazione) della ricetta |

**Campi figli supportati in `ElencoDettagliPrescrInviiErogato`**

| Campo figlio | Descrizione XSD |
|---|---|
| `codProdPrest` | Codice prodotto farmaceutico (AIC) o codice prestazione specialistica |
| `codGruppoEquival` | Codice del gruppo equivalente |
| `descrTestoLiberoNote` | Descrizione testuale della prestazione specialistica |
| `codProdPrestErog` | Codice AIC del farmaco o codice della prestazione effettivamente erogata |
| `descrProdPrestErog` | Descrizione del farmaco o della prestazione effettivamente erogata |
| `flagErog` | A = codice AIC aggiornato, S = sostituzione AIC, V = variazione codice prestazione |
| `motivazSostProd` | Motivazione in caso di valore S in flagErog |
| `targa` | Numero identificativo univoco della singola confezione |
| `dichTargaDoppia` | Dichiarazione del farmacista di erogazione di un farmaco il cui codice targatura è già esistente presso il SAC |
| `codBranca` | Codice della branca specialistica della prestazione |
| `tipoErogazioneFarm` | Tipologia di erogazione dei farmaci |
| `prezzo` | Prezzo al pubblico del prodotto farmaceutico o tariffa della prestazione specialistica |
| `ticketConfezione` | Ticket della confezione farmaceutica |
| `diffGenerico` | Differenza con generico della confezione farmaceutica |
| `quantitaErogata` | Quantità effettivamente erogata |
| `dataIniErog` | Data di inizio erogazione |
| `dataFineErog` | Data di fine erogazione |
| `prezzoRimborso` | Prezzo di rimborso al laboratorio |
| `onereProd` | Onere di distribuzione del prodotto farmaceutico erogato in DPC |
| `scontoSSN` | Sconto riconosciuto al SSN |
| `extraScontoIndustria` | Sconto obbligatorio dello 0,6% sul prezzo al pubblico lordo con IVA a carico dell'industria produttrice |
| `extraScontoPayback` | Sconto trattenuto dal SSN per importo corrispondente allo 0,6% del prezzo al pubblico comprensivo IVA |
| `extraScontoDL31052010` | Sconto introdotto nel DL 31.05.2010 |
| `codPresidio` | Codice del presidio di erogazione della prestazione |
| `codReparto` | Codice del reparto di erogazione della prestazione |
| `dispFust1` | Campo per futuro utilizzo |
| `dispFust2` | Campo per futuro utilizzo |
| `dispFust3` | Campo per futuro utilizzo |
| `codCatalogoPrescr` | Codice del catalogo regionale della prestazione prescritta |
| `codCatalogoErog` | Codice del catalogo regionale erogato |
| `garanziaTempiMax` | Garanzia dei tempi massimi di attesa |
| `dataPrenotazione` | Data di prenotazione |

**Altri campi presenti nell'XSD ma non normalizzati esplicitamente**
- `pwd` = identificativo dell'utente che ha effettuato l'operazione
- `cfAssistito` = Codice fiscale dell’assistito prelevato dalla Tessera Sanitaria
- `prescrizioneFruita` = Dichiarazione di effettiva fruizione della prestazione specialistica
- `tipoErogazioneSpec` = Tipo erogazione della prestazione specialistica
- `ticket` = Ammontare della quota di compartecipazione alla spesa sanitaria per farmaceutica
- `quotaFissa` = Quota fissa del ticket per prestazione specialistica
- `franchigia` = Franchigia del ticket per prestazione specialistica
- `galDirChiamAltro` = Ammontare del prezzo del galenico / diritto di chiamata notturna
- `reddito` = Indica se l’assistito è esente per reddito oppure no
- `dispRic1`, `dispRic2`, `dispRic3` = Campo per futuro utilizzo
- `ElencoDettagliPrescrInviiErogato` = Elenco singole prescrizioni invio erogato

---

### 11. VisualizzaErogato
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `codiceRegioneErogatore` | sì | `CODICEREGIONEEROGATORE` | Codice regione della struttura erogatrice |
| `codiceAslErogatore` | sì | `CODICEASLEROGATORE` | Codice ASL della struttura erogatrice |
| `codiceSsaErogatore` | sì | `CODICESSAEROGATORE` | Codice SSA della struttura erogatrice |
| `nre` | sì | `NRE` | Numero Ricetta Elettronica |
| `tipoOperazione` | sì | `TIPOOPERAZIONE` | 1 = Blocco esclusivo con restituzione dati ricetta, 2 = Blocco esclusivo senza restituzione dati, 3 = Rilascio ricetta, 4 = Visualizza dati oscurati |
| `cfAssistito` | no | `CFASSISTITO` | Codice fiscale dell’assistito prelevato dalla Tessera Sanitaria |

---

### 12. SospendiErogato
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `codiceRegioneErogatore` | sì | `CODICEREGIONEEROGATORE` | Codice regione della struttura erogatrice |
| `codiceAslErogatore` | sì | `CODICEASLEROGATORE` | Codice ASL della struttura erogatrice |
| `codiceSsaErogatore` | sì | `CODICESSAEROGATORE` | Codice SSA della struttura erogatrice |
| `nre` | sì | `NRE` | Numero Ricetta Elettronica |
| `tipoOperazione` | sì | `TIPOOPERAZIONE` | 1 = Inizio sospensione, 2 = Revoca sospensione |

---

### 13. AnnullaErogato
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `codiceRegioneErogatore` | sì | `CODICEREGIONEEROGATORE` | Codice regione della struttura erogatrice |
| `codiceAslErogatore` | sì | `CODICEASLEROGATORE` | Codice ASL della struttura erogatrice |
| `codiceSsaErogatore` | sì | `CODICESSAEROGATORE` | Codice SSA della struttura erogatrice |
| `nre` | sì | `NRE` | Numero Ricetta Elettronica |
| `codAnnullamento` | sì | `CODANNULLAMENTO` | Codice annullamento erogato |

---

### 14. RicercaErogatore
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `codiceRegioneErogatore` | sì | `CODICEREGIONEEROGATORE` | Codice regione della struttura erogatrice |
| `codiceAslErogatore` | sì | `CODICEASLEROGATORE` | Codice ASL della struttura erogatrice |
| `codiceSsaErogatore` | sì | `CODICESSAEROGATORE` | Codice SSA della struttura erogatrice |

**Altri campi presenti nell'XSD ma non normalizzati esplicitamente**
- `pwd` = identificativo dell'utente che ha effettuato l'operazione
- `nre` = Numero Ricetta Elettronica
- `cfAssistito` = Codice fiscale dell’assistito prelevato dalla Tessera Sanitaria
- `dataPeriodoDal` = Data periodo DAL nel formato aaaa-mm-gg
- `dataPeriodoAl` = Data periodo AL nel formato aaaa-mm-gg
- `tipoData` = Tipo di data utilizzata per la ricerca: P o E
- `statoRicetta` = Stato della ricetta
- `disp1`, `disp2`, `disp3` = Campo a disposizione

---

### 15. ReportErogatoMensile
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `codiceRegioneErogatore` | sì | `CODICEREGIONEEROGATORE` | Codice regione della struttura erogatrice |
| `codiceAslErogatore` | sì | `CODICEASLEROGATORE` | Codice ASL della struttura erogatrice |
| `codiceSsaErogatore` | sì | `CODICESSAEROGATORE` | Codice SSA della struttura erogatrice |
| `annoMese` | sì | `ANNOMESE` | Anno mese della richiesta nel formato AAAAMM |

---

### 16. ServiceAnagErogatore
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `tipoOperazione` | sì | `TIPOOPERAZIONE` | Operazione da eseguire |

**Altri campi presenti nell'XSD ma non normalizzati esplicitamente**
- `codiceRegione` = Codice regione
- `nomeSWH` = Nome della software house
- `mailSWH` = Email della software house
- `opzione1`..`opzione5` = Campo opzionale

---

### 17. RicettaDifferita
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `codiceRegioneErogatore` | sì | `CODICEREGIONEEROGATORE` | Codice regione della struttura erogatrice |
| `codiceAslErogatore` | sì | `CODICEASLEROGATORE` | Codice ASL della struttura erogatrice |
| `codiceSsaErogatore` | sì | `CODICESSAEROGATORE` | Codice SSA della struttura erogatrice |
| `codMotivazione` | sì | `CODMOTIVAZIONE` | Codice motivazione del guasto |
| `dataDal` | sì | `DATADAL` | Data di inizio del guasto-anomalia |

**Altri campi presenti nell'XSD ma non normalizzati esplicitamente**
- `pwd` = identificativo dell'utente che ha effettuato l'operazione
- `note` = Note aggiuntive
- `dispRic1`, `dispRic2`, `dispRic3` = Campo per futuro utilizzo

---

### 18. AnnullaErogatoDiff
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `pinCode` | sì | `PINCODE` | PinCode inviante |
| `codiceRegioneErogatore` | sì | `CODICEREGIONEEROGATORE` | Codice regione della struttura erogatrice |
| `codiceAslErogatore` | sì | `CODICEASLEROGATORE` | Codice ASL della struttura erogatrice |
| `codiceSsaErogatore` | sì | `CODICESSAEROGATORE` | Codice SSA della struttura erogatrice |
| `idRicetta` | sì | `IDRICETTA` | Identificativo univoco ricette differite |
| `nre` | sì | `NRE` | Numero Ricetta Elettronica |

**Altri campi presenti nell'XSD ma non normalizzati esplicitamente**
- `pwd` = identificativo dell'utente che ha effettuato l'operazione
- `cfAssistito` = Codice fiscale dell’assistito prelevato dalla Tessera Sanitaria
- `codAnnullamentoDiff` = Codice motivazione annullamento
- `disp1`, `disp2`, `disp3` = Campo a disposizione

---

### 19. RicevuteSac
**Stato documentazione**
- Nessun parametro obbligatorio definito in `InputMapperService`.
- Per questo servizio il documento non ha ancora una descrizione XSD consolidata nel workspace.

---

## Authorization 2F (A2F)

### 20. CreateAuth
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `userId` | no* | `USERID` | UserID di SistemaTS per l'utente. Obbligatorio oppure opzionale a seconda del parametro contesto |
| `identificativo` | no* | `IDENTIFICATIVO` | Codice PIN o altro codice cifrato. Obbligatorio oppure opzionale a seconda del parametro contesto |
| `identificativo_tipo` | no | `IDENTIFICATIVO_TIPO` | Tipologia identificativo (attualmente P = pincode, I = id inviante sogei) |
| `identificativo_valore` | no | `IDENTIFICATIVO_VALORE` | Identificativo cifrato e codificato in base64 |
| `cfUtente` | no* | `CFUTENTE` | Codice Fiscale dell'utente. Obbligatorio oppure opzionale a seconda del parametro contesto |
| `codRegione` | no | `CODREGIONE` | Codice Regione / Provincia Autonoma o Codice Amministrazione di appartenenza del soggetto indicato in cfUtente |
| `codAslAo` | no | `CODASLAO` | Codice ASL o Subcodice Amministrazione di appartenenza del soggetto indicato in cfUtente |
| `codSsa` | no | `CODSSA` | Codice SSA di appartenenza del soggetto |
| `codiceStruttura` | no | `CODICESTRUTTURA` | Codice della struttura di appartenenza del medico |
| `contesto` | sì | `CONTESTO` | Contesto di validità del token |
| `applicazione` | no | `APPLICAZIONE` | Applicazione dove il token viene consumato se il contesto è suddiviso |

**Ulteriori strutture A2F presenti nell'XSD**
- `opzioni` = Eventuali campi opzionali
- `infoAggiuntive` = Elenco di informazioni in input aggiuntive, tipicamente usate in test

---

### 21. RevokeAuth
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `userId` | no | `USERID` | UserID di SistemaTS per l'utente |
| `identificativo` | no | `IDENTIFICATIVO` | Codice PIN o altro codice cifrato |
| `identificativo_tipo` | no | `IDENTIFICATIVO_TIPO` | Tipologia identificativo |
| `identificativo_valore` | no | `IDENTIFICATIVO_VALORE` | Identificativo cifrato e codificato in base64 |
| `cfUtente` | no | `CFUTENTE` | Codice Fiscale dell'utente |
| `token` | sì | `TOKEN` | Token ricevuto tramite l'operazione create |
| `contesto` | no | `CONTESTO` | Contesto di validità per il quale il token è stato richiesto |
| `applicazione` | no | `APPLICAZIONE` | Applicazione per la quale il token è stato richiesto |
| `codRegione` | no | `CODREGIONE` | Non descritto nella request RevokeAuth ma accettato dalla libreria |
| `codAslAo` | no | `CODASLAO` | Non descritto nella request RevokeAuth ma accettato dalla libreria |
| `codSsa` | no | `CODSSA` | Non descritto nella request RevokeAuth ma accettato dalla libreria |
| `codiceStruttura` | no | `CODICESTRUTTURA` | Non descritto nella request RevokeAuth ma accettato dalla libreria |

---

### 22. CheckToken
| Parametro | Obbl. | Alias | Descrizione XSD |
|---|---|---|---|
| `userId` | no | `USERID` | UserID di SistemaTS per l'utente |
| `identificativo` | no | `IDENTIFICATIVO` | Codice PIN o altro codice cifrato |
| `identificativo_tipo` | no | `IDENTIFICATIVO_TIPO` | Tipologia identificativo |
| `identificativo_valore` | no | `IDENTIFICATIVO_VALORE` | Identificativo cifrato e codificato in base64 |
| `cfUtente` | no | `CFUTENTE` | Codice Fiscale dell'utente |
| `token` | sì | `TOKEN` | Token ricevuto tramite l'operazione create |
| `contesto` | no | `CONTESTO` | Contesto di validità per il quale il token è stato richiesto |
| `applicazione` | no | `APPLICAZIONE` | Applicazione per la quale il token è stato richiesto |
| `codRegione` | no | `CODREGIONE` | Non descritto nella request CheckToken ma accettato dalla libreria |
| `codAslAo` | no | `CODASLAO` | Non descritto nella request CheckToken ma accettato dalla libreria |
| `codSsa` | no | `CODSSA` | Non descritto nella request CheckToken ma accettato dalla libreria |
| `codiceStruttura` | no | `CODICESTRUTTURA` | Non descritto nella request CheckToken ma accettato dalla libreria |

---

## Note finali

- Le descrizioni riportate sono recuperate dagli XSD presenti nel repository, quando disponibili.
- Il documento resta focalizzato sui **parametri accettati dalla libreria**; alcuni XSD contengono campi aggiuntivi non ancora normalizzati in `InputMapperService`.
- Dove la libreria accetta un parametro ma la relativa request XSD non lo documenta esplicitamente, il documento lo segnala.
- Per integrazione esterna COM/.NET vedere anche:
  - `INTERFACCIAMENTO_ESTERNO.md`
  - `PROCESSO_DELPHI_TOKEN_MEDICO_E_INVIO_PRESCRITTO.md`
  - `A2F_COMPENDIO.md`

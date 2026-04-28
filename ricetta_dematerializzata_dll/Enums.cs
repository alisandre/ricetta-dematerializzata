namespace ricetta_dematerializzata_dll.Models
{
    /// <summary>
    /// Catalogo di tutti i servizi disponibili per il sistema SAC / DEM Ricetta.
    /// Usare questo enum come primo parametro di ISanitaServiceClient.Chiama().
    /// </summary>
    public enum DigitalPrescriptionService
    {
        // ─── PRESCRITTORE ────────────────────────────────────────────────────────

        /// <summary>
        /// Visualizza i dettagli di una prescrizione già inviata.
        /// Input:  NRE, CODICE_FISCALE_ASSISTITO (opzionale)
        /// Output: dati della prescrizione
        /// </summary>
        VisualizzaPrescritto = 1,

        /// <summary>
        /// Invia una nuova prescrizione dematerializzata.
        /// Input:  tutti i campi della ricetta (medico, assistito, farmaci…)
        /// Output: NRE, CODICE_ESITO_INSERIMENTO (0000/0001/9999)
        /// </summary>
        InvioPrescritto = 2,

        /// <summary>
        /// Annulla una prescrizione precedentemente inviata.
        /// Input:  NRE, CODICE_FISCALE_MEDICO, MOTIVO_ANNULLAMENTO
        /// Output: CODICE_ESITO
        /// </summary>
        AnnullaPrescritto = 3,

        /// <summary>
        /// Interroga i Numeri Ricetta Elettronica (NRE) già utilizzati.
        /// Input:  CODICE_REGIONE, CODICE_ASL, MESE, ANNO
        /// Output: lista NRE
        /// </summary>
        InterrogaNreUtilizzati = 4,

        /// <summary>
        /// Servizio anagrafico condiviso (Prescrittore).
        /// Input:  CODICE_FISCALE_ASSISTITO o TESSERA_STP
        /// Output: dati anagrafici assistito
        /// </summary>
        ServiceAnagPrescrittore = 5,

        /// <summary>
        /// Invia la dichiarazione di sostituzione del medico.
        /// Input:  dati medico titolare, medico sostituto, periodo
        /// Output: CODICE_ESITO
        /// </summary>
        InvioDichiarazioneSostituzioneMedico = 6,

        // ─── EROGATORE ────────────────────────────────────────────────────────────

        /// <summary>
        /// Invia l'erogato (farmacia/dispensatore) per una prescrizione.
        /// Input:  NRE, dati farmaci erogati, importi, erogatore
        /// Output: CODICE_ESITO_INSERIMENTO
        /// </summary>
        InvioErogato = 10,

        /// <summary>
        /// Visualizza i dati di un erogato già trasmesso.
        /// Input:  NRE, CODICE_EROGATORE
        /// Output: dettagli erogato
        /// </summary>
        VisualizzaErogato = 11,

        /// <summary>
        /// Sospende temporaneamente un erogato (es. farmaco non disponibile).
        /// Input:  NRE, CODICE_EROGATORE, MOTIVO_SOSPENSIONE
        /// Output: CODICE_ESITO
        /// </summary>
        SospendiErogato = 12,

        /// <summary>
        /// Annulla un erogato trasmesso in precedenza.
        /// Input:  NRE, CODICE_EROGATORE, MOTIVO_ANNULLAMENTO
        /// Output: CODICE_ESITO
        /// </summary>
        AnnullaErogato = 13,

        /// <summary>
        /// Ricerca erogatore per codice o denominazione.
        /// Input:  CODICE_EROGATORE o DENOMINAZIONE (almeno uno)
        /// Output: lista erogatori corrispondenti
        /// </summary>
        RicercaErogatore = 14,

        /// <summary>
        /// Report mensile degli erogati trasmessi.
        /// Input:  CODICE_EROGATORE, MESE, ANNO
        /// Output: riepilogo erogati del mese
        /// </summary>
        ReportErogatoMensile = 15,

        /// <summary>
        /// Servizio anagrafico condiviso (Erogatore).
        /// Input:  CODICE_FISCALE_ASSISTITO o TESSERA_STP
        /// Output: dati anagrafici assistito
        /// </summary>
        ServiceAnagErogatore = 16,

        /// <summary>
        /// Gestione ricetta differita (dematerializzazione posticipata).
        /// Input:  dati ricetta cartacea da dematerializzare
        /// Output: CODICE_ESITO, NRE assegnato
        /// </summary>
        RicettaDifferita = 17,

        /// <summary>
        /// Annulla un erogato in modalità ricetta differita.
        /// Input:  NRE_DIFFERITA, CODICE_EROGATORE, MOTIVO
        /// Output: CODICE_ESITO
        /// </summary>
        AnnullaErogatoDiff = 18,

        /// <summary>
        /// Recupera le ricevute SAC (Sistema di Accoglienza Centrale).
        /// Input:  CODICE_EROGATORE, DATA_INIZIO, DATA_FINE
        /// Output: lista ricevute
        /// </summary>
        RicevuteSac = 19,

        // ─── AUTHORIZATION 2F (A2F) ───────────────────────────────────────────────

        /// <summary>
        /// Crea un token di autenticazione 2FA.
        /// Operazione WSDL: create
        /// </summary>
        CreateAuth = 20,

        /// <summary>
        /// Revoca un token di autenticazione 2FA.
        /// Operazione WSDL: revoke
        /// </summary>
        RevokeAuth = 21,

        /// <summary>
        /// Verifica lo stato/validità di un token 2FA.
        /// Operazione WSDL: checkToken
        /// </summary>
        CheckToken = 22,
    }

    /// <summary>
    /// Ambiente di destinazione per le chiamate al servizio.
    /// </summary>
    public enum ServiceEnvironment
    {
        /// <summary>Ambiente di test/collaudo (demservicetest.sanita.finanze.it)</summary>
        Test = 0,

        /// <summary>Ambiente di produzione (demservice.sanita.finanze.it)</summary>
        Produzione = 1
    }
}

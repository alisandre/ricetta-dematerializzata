using System;
using System.Collections.Generic;
using System.Text;

namespace ricetta_dematerializzata.Core
{
    /// <summary>
    /// Utility per serializzare/deserializzare il formato chiave=valore usato
    /// come contratto di input/output verso Delphi.
    ///
    /// Formato:  CHIAVE1=VALORE1;CHIAVE2=VALORE2
    /// Regole:
    ///   - separatore di coppia: ';'
    ///   - separatore chiave/valore: '='  (solo il primo '=' conta)
    ///   - chiavi: case-insensitive, uppercase in output
    ///   - valori che contengono ';' devono essere escaped con ';;'
    ///   - valori vuoti ammessi: CHIAVE=;CHIAVE2=VAL
    /// </summary>
    public static class ParserKV
    {
        // ── Parse ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Converte la stringa "K=V;K2=V2" in un dizionario (chiavi uppercase).
        /// </summary>
        public static Dictionary<string, string> Parse(string? input)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(input)) return dict;

            // Gestisce escape ';;' → placeholder → split → ripristina
            const string placeholder = "\x01\x01";
            var lavorazione = input.Replace(";;", placeholder);
            var parti = lavorazione.Split(';');

            foreach (var parte in parti)
            {
                if (string.IsNullOrWhiteSpace(parte)) continue;
                var idx = parte.IndexOf('=');
                if (idx < 0) continue;

                var chiave = parte.Substring(0, idx).Trim().ToUpperInvariant();
                var valore = parte.Substring(idx + 1).Replace(placeholder, ";");
                dict[chiave] = valore;
            }
            return dict;
        }

        /// <summary>
        /// Restituisce il valore di una chiave dalla stringa input, o stringa vuota.
        /// </summary>
        public static string Get(string input, string chiave)
        {
            var dict = Parse(input);
            return dict.TryGetValue(chiave.ToUpperInvariant(), out var v) ? v : string.Empty;
        }

        // ── Build ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Costruisce la stringa "K=V;K2=V2" da un dizionario.
        /// </summary>
        public static string Build(Dictionary<string, string> dict)
        {
            var sb = new StringBuilder();
            foreach (var kv in dict)
            {
                if (sb.Length > 0) sb.Append(';');
                var key   = kv.Key.ToUpperInvariant();
                var value = kv.Value?.Replace(";", ";;") ?? string.Empty;
                sb.Append(key).Append('=').Append(value);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Costruisce la stringa da coppie (key, value) passate come params.
        /// Esempio: Build("NRE", "12345", "CF", "RSSMRA80A01H501T")
        /// </summary>
        public static string Build(params string[] coppie)
        {
            if (coppie.Length % 2 != 0)
                throw new ArgumentException("Devono essere passate coppie chiave/valore.", nameof(coppie));

            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < coppie.Length; i += 2)
                dict[coppie[i]] = coppie[i + 1];
            return Build(dict);
        }

        // ── Esito standard ────────────────────────────────────────────────────────

        /// <summary>
        /// Costruisce la stringa di errore standard.
        /// es. "ERRORE_NUMERO=9999;ERRORE_DESCRIZIONE=Timeout di connessione"
        /// </summary>
        public static string BuildErrore(int numero, string descrizione)
            => Build("ERRORE_NUMERO", numero.ToString(), "ERRORE_DESCRIZIONE", descrizione);

        /// <summary>
        /// Restituisce true se la stringa output contiene un errore (ERRORE_NUMERO presente).
        /// </summary>
        public static bool IsErrore(string output)
            => !string.IsNullOrEmpty(Get(output, "ERRORE_NUMERO"));

        /// <summary>
        /// Verifica se il codice esito corrisponde a successo (0000 o 0001).
        /// </summary>
        public static bool IsSuccesso(string codiceEsito)
            => codiceEsito == "0000" || codiceEsito == "0001";
    }
}

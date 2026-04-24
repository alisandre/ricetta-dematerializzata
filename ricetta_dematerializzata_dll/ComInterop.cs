using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ricetta_dematerializzata_dll.Services
{
    /// <summary>
    /// Interfaccia COM-visible per l'uso da Delphi.
    ///
    /// REGISTRAZIONE COM:
    ///   regasm SanitaServiceLib.dll /tlb:SanitaServiceLib.tlb /codebase
    ///
    /// USO DA DELPHI:
    ///   1. Importare SanitaServiceLib.tlb in Delphi
    ///   2. uses SanitaServiceLib_TLB;
    ///   3. var client: ISanitaServiceClient;
    ///      client := CoSanitaServiceClient.Create;
    ///      client.Configura('user', 'pass', 0, True);
    ///      result := client.Chiama(2, 'NRE=12345;CF=RSSMRA...');
    /// </summary>
    [ComVisible(true)]
    [Guid("A1B2C3D4-E5F6-7890-ABCD-EF1234567890")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ISanitaServiceClient
    {
        /// <summary>Configura credenziali e ambiente (0=Test, 1=Produzione)</summary>
        void Configura(string username, string password, int ambiente = 0, bool ignoraSsl = false);

        /// <summary>Imposta il valore Authorization2F (ID-SESSIONE o Bearer completo)</summary>
        void ConfiguraAuthorization2F(string? authorization2F);

        /// <summary>Configura i certificati SSL e Sanitel</summary>
        void ConfiguraCertificati(string? pathSsl, string? pathCA, string? pathSanitel);

        /// <summary>
        /// Chiama il servizio identificato dal codice enum.
        /// Input/Output nel formato "CHIAVE=VALORE;CHIAVE2=VALORE2"
        /// </summary>
        string Chiama(int servizio, string parametriInput);

        /// <summary>Chiama il servizio con input/output JSON</summary>
        string ChiamaJson(int servizio, string parametriInputJson);

        /// <summary>Restituisce l'URL dell'endpoint per un servizio</summary>
        string OttieniUrl(int servizio);

        /// <summary>Testa la configurazione dell'endpoint</summary>
        string TestConnessione(int servizio);

        /// <summary>Cifra un valore con il certificato Sanitel</summary>
        string CifraConSanitel(string testoPiano);
    }
}

namespace ricetta_dematerializzata_dll.Core
{
    /// <summary>
    /// Helper minimalista per serializzazione/deserializzazione JSON
    /// senza dipendenze esterne pesanti.
    /// Supporta solo dizionari string→string (sufficiente per il contratto KV).
    /// </summary>
    internal static class JsonHelper
    {
        public static string ToJson(Dictionary<string, string> dict)
        {
            var sb = new System.Text.StringBuilder("{");
            bool first = true;
            foreach (var kv in dict)
            {
                if (!first) sb.Append(',');
                sb.Append('"').Append(Esc(kv.Key)).Append("\":\"")
                  .Append(Esc(kv.Value)).Append('"');
                first = false;
            }
            sb.Append('}');
            return sb.ToString();
        }

        public static Dictionary<string, string> FromJson(string json)
        {
            // Parser JSON minimale per dizionari piatti string:string
            // Per input complessi usare System.Text.Json o Newtonsoft
            var result = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(json)) return result;

            json = json.Trim();
            if (json.StartsWith("{")) json = json.Substring(1);
            if (json.EndsWith("}")) json = json.Substring(0, json.Length - 1);

            // Regex semplice per coppie "key":"value"
            var regex = new System.Text.RegularExpressions.Regex(
                @"""([^""\\]*(?:\\.[^""\\]*)*)""\s*:\s*""([^""\\]*(?:\\.[^""\\]*)*)""");

            foreach (System.Text.RegularExpressions.Match m in regex.Matches(json))
                result[Unescape(m.Groups[1].Value)] = Unescape(m.Groups[2].Value);

            return result;
        }

        private static string Esc(string s)
            => s.Replace("\\", "\\\\").Replace("\"", "\\\"")
                .Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");

        private static string Unescape(string s)
            => s.Replace("\\\"", "\"").Replace("\\\\", "\\")
                .Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t");
    }
}

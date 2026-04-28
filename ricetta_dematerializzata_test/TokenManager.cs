using System;
using System.IO;

namespace ricetta_dematerializzata_test_ui
{
    /// <summary>
    /// Gestisce la persistenza dell'ultimo token A2F generato.
    /// Il token viene salvato in un file locale e ricaricato al riavvio dell'applicazione.
    /// </summary>
    public static class TokenManager
    {
        private static string TokenFilePath(string ruolo) => System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ricetta-dematerializzata",
            $"token_{ruolo.ToLowerInvariant()}.txt"
        );

        /// <summary>
        /// Carica l'ultimo token salvato, se esiste.
        /// </summary>
        public static string? LoadToken(string ruolo)
        {
            try
            {
                var path = TokenFilePath(ruolo);
                if (File.Exists(path))
                {
                    var content = File.ReadAllText(path).Trim();
                    return string.IsNullOrWhiteSpace(content) ? null : content;
                }
            }
            catch
            {
                // Se fallisce la lettura, restituisci null
            }
            return null;
        }

        /// <summary>
        /// Salva un nuovo token persistentemente.
        /// </summary>
        public static void SaveToken(string token, string ruolo)
        {
            try
            {
                var path = TokenFilePath(ruolo);
                var dir  = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);
                File.WriteAllText(path, token);
            }
            catch
            {
                // Silenzioso se la scrittura fallisce
            }
        }

        /// <summary>
        /// Cancella il token salvato.
        /// </summary>
        public static void ClearToken(string ruolo)
        {
            try
            {
                var path = TokenFilePath(ruolo);
                if (File.Exists(path)) File.Delete(path);
            }
            catch
            {
                // Silenzioso se la cancellazione fallisce
            }
        }

        /// <summary>
        /// Verifica se esiste un token salvato.
        /// </summary>
        public static bool HasToken(string ruolo)
        {
            return File.Exists(TokenFilePath(ruolo)) && 
                   !string.IsNullOrWhiteSpace(LoadToken(ruolo));
        }
    }
}

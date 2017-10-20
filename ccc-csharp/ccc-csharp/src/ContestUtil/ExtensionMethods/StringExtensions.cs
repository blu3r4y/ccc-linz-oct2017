using System;
using System.Security.Cryptography;
using System.Text;

namespace CCC.ContestUtil.ExtensionMethods
{
    public static class StringExtensions
    {
        /// <summary>
        /// Beschneidet einen String, sofern möglich
        /// </summary>
        /// <param name="value">Zu beschneidender String</param>
        /// <param name="maxLength">Maximale Länge des Ergebnisstrings</param>
        /// <returns>Beschnittener String</returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (maxLength <= 0) return string.Empty;
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        /// Gibt einen MD5 Hash als String zurück
        /// </summary>
        /// <param name="textToHash">string der Gehasht werden soll.</param>
        /// <returns>Hash als string.</returns>
        public static string GetMd5Hash(this string textToHash)
        {
            //Prüfen ob Daten übergeben wurden.
            if (string.IsNullOrEmpty(textToHash))
            {
                return string.Empty;
            }

            // MD5 Hash aus dem String berechnen. Dazu muss der string in ein Byte[]
            // zerlegt werden. Danach muss das Resultat wieder zurück in ein string.
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] hashArr = Encoding.Default.GetBytes(textToHash);
            byte[] result = md5.ComputeHash(hashArr);

            return BitConverter.ToString(result);
        }


        /// <summary>
        /// Entfernt mehrfache Zeilenumbrüche im angegebenen String
        /// </summary>
        /// <param name="text">Der zu untersuchende String</param>
        /// <returns>Ein String ohne mehrfache Zeilenumbrüche</returns>
        public static string TrimMultipleLineBreaks(this string text)
        {
            const string winBreak = "\r\n";
            const string linuxBreak = "\n";

            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            // search duplicate line breaks
            while (text.Contains(winBreak + winBreak)
                || text.Contains(linuxBreak + linuxBreak))
            {
                text = text.Replace(winBreak + winBreak, winBreak);
                text = text.Replace(linuxBreak + linuxBreak, linuxBreak);
            }

            return text;
        }
    }
}

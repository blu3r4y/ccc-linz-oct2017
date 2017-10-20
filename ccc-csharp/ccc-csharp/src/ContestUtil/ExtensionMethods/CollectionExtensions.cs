using System.Collections.Generic;

namespace CCC.ContestUtil.ExtensionMethods
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Versucht den Wert zum gewünschten Key aus dem Dictionary zu holen und gibt bei Fehlschlag den Standardwert (z.B. null) zurück.
        /// </summary>
        /// <typeparam name="TKey">Datentyp des Schlüssels</typeparam>
        /// <typeparam name="TValue">Datentyp des Werts</typeparam>
        /// <param name="dictionary">Zu durchsuchendes Dictionary</param>
        /// <param name="key">Zu suchender Schlüssel</param>
        /// <returns>Den Wert (Value) an dieser Stelle oder den Standardwert (z.B. null) sollte der Wert nicht gefunden worden sein.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            // Standardwert
            TValue getValue = default(TValue);

            // Versuchen den Wert zu holen
            if (key != null) dictionary.TryGetValue(key, out getValue);

            return getValue;
        }

        /// <summary>
        /// Versucht ein Element in die Liste einzufügen, sofern es nicht schon existiert
        /// </summary>
        /// <typeparam name="T">Datentyp des Elements</typeparam>
        /// <param name="list">Anzusprechende Liste</param>
        /// <param name="value">Einzufügendes Element</param>
        /// <returns>TRUE, wenn das Element eingefügt werden konnte</returns>
        public static bool AddIfNotExists<T>(this List<T> list, T value)
        {
            if (list.Contains(value)) return false;

            list.Add(value);
            return true;
        }

        /// <summary>
        /// Versucht ein Element in das Wörterbuch einzufügen, sofern es nicht schon existiert
        /// </summary>
        /// <typeparam name="T">Datentyp des Schlüssels</typeparam>
        /// <typeparam name="V">Datentyp des Werts</typeparam>
        /// <param name="dict">Anzusprechende Liste</param>
        /// <param name="key">Einzufügender Schlüssel</param>
        /// <param name="value">Einzufügender Wert</param>
        /// <returns>TRUE, wenn das Element eingefügt werden konnte</returns>
        public static bool AddIfNotExists<T, V>(this Dictionary<T, V> dict, T key, V value)
        {
            if (dict.ContainsKey(key)) return false;

            dict.Add(key, value);
            return true;
        }
    }
}

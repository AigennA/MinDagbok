using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MinDagbok
{
    public class FileHandler
    {
        private readonly string _filePath;
        private readonly string _loggfil = "error.log";

        public FileHandler(string filePath)
        {
            _filePath = filePath;
        }

        // Läser anteckningar från fil och returnerar en lista
        public List<DiaryEntry> LoadEntries()
        {
            List<DiaryEntry> lista = new List<DiaryEntry>();

            try
            {
                if (!File.Exists(_filePath))
                    return lista;

                string innehåll = File.ReadAllText(_filePath);
                lista = JsonSerializer.Deserialize<List<DiaryEntry>>(innehåll);

                if (lista == null)
                    lista = new List<DiaryEntry>();
            }
            catch (Exception fel)
            {
                LoggaFel("Fel vid läsning från fil: " + fel.Message);
                Console.WriteLine("Kunde inte läsa filen. Se error.log för detaljer.");
            }

            return lista;
        }

        // Sparar anteckningar till fil i JSON-format
        public void SaveEntries(List<DiaryEntry> lista)
        {
            try
            {
                string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception fel)
            {
                LoggaFel("Fel vid skrivning till fil: " + fel.Message);
                Console.WriteLine("Kunde inte spara filen. Se error.log för detaljer.");
            }
        }

        // Exporterar anteckningar till CSV-format
        public void ExportToCsv(List<DiaryEntry> lista, string csvFil)
        {
            try
            {
                List<string> rader = new List<string>();

                for (int i = 0; i < lista.Count; i++)
                {
                    DiaryEntry post = lista[i];
                    string rad = post.Date.ToString("yyyy-MM-dd") + ";\"" + post.Text.Replace("\"", "\"\"") + "\"";
                    rader.Add(rad);
                }

                File.WriteAllLines(csvFil, rader);
                Console.WriteLine("Exporterat till " + csvFil);
            }
            catch (Exception fel)
            {
                LoggaFel("Fel vid CSV-export: " + fel.Message);
                Console.WriteLine("Kunde inte exportera till CSV. Se error.log.");
            }
        }

        // Loggar fel till separat textfil
        private void LoggaFel(string meddelande)
        {
            string rad = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + meddelande;
            File.AppendAllText(_loggfil, rad + Environment.NewLine);
        }
    }
}

using System.Text.Json;

namespace MinDagbok
{
    public class FileHandler
    {
        private readonly string _filePath;
        private readonly string _errorLogPath = "error.log";

        public FileHandler(string filePath)
        {
            _filePath = filePath;
        }

        public List<DiaryEntry> LoadEntries()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<DiaryEntry>();

                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<DiaryEntry>>(json) ?? new List<DiaryEntry>();
            }
            catch (Exception ex)
            {
                LogError($"Fel vid läsning: {ex.Message}");
                Console.WriteLine("Kunde inte läsa filen.");
                return new List<DiaryEntry>();
            }
        }

        public void SaveEntries(List<DiaryEntry> entries)
        {
            try
            {
                string json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                LogError($"Fel vid skrivning: {ex.Message}");
                Console.WriteLine("Kunde inte spara filen.");
            }
        }

        private void LogError(string message)
        {
            File.AppendAllText(_errorLogPath, $"{DateTime.Now}: {message}\n");
        }
    }
}

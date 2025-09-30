using System;
using MinDagbok;

namespace MinDagbok
{
    public class UIMenu
    {
        private readonly DiaryService _diaryService;
        private readonly FileHandler _fileHandler;

        public UIMenu(DiaryService diaryService, FileHandler fileHandler)
        {
            _diaryService = diaryService;
            _fileHandler = fileHandler;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n--- Min DAGBOK ---");
                Console.WriteLine("1. Skriv ny anteckning");
                Console.WriteLine("2. Lista alla anteckningar");
                Console.WriteLine("3. Sök anteckning på datum");
                Console.WriteLine("4. Uppdatera anteckning");
                Console.WriteLine("5. Ta bort anteckning");
                Console.WriteLine("6. Spara till fil");
                Console.WriteLine("7. Läs från fil");
                Console.WriteLine("8. Avsluta");
                Console.Write("Val: ");

                switch (Console.ReadLine())
                {
                    case "1": AddEntry(); break;
                    case "2": ListEntries(); break;
                    case "3": SearchEntry(); break;
                    case "4": UpdateEntry(); break;
                    case "5": RemoveEntry(); break;
                    case "6": _fileHandler.SaveEntries(_diaryService.GetAllEntries().ToList()); break;
                    case "7": _diaryService.LoadFromFile(_fileHandler.LoadEntries()); break;
                    case "8": return;
                    default: Console.WriteLine("Ogiltigt val."); break;
                }
            }
        }

        private void AddEntry()
        {
            DateTime date = PromptForDate("Datum (yyyy-MM-dd): ");
            if (date == DateTime.MinValue) return;

            Console.Write("Text: ");
            string text = Console.ReadLine();

            if (_diaryService.AddEntry(date, text))
                Console.WriteLine("Anteckning tillagd.");
            else
                Console.WriteLine("Misslyckades med att lägga till anteckning.");
        }

        private void ListEntries()
        {
            var entries = _diaryService.GetAllEntries();
            if (!entries.Any())
            {
                Console.WriteLine("Inga anteckningar.");
                return;
            }

            foreach (var entry in entries.OrderBy(e => e.Date))
                Console.WriteLine(entry);
        }

        private void SearchEntry()
        {
            DateTime date = PromptForDate("Datum att söka (yyyy-MM-dd): ");
            if (date == DateTime.MinValue) return;

            var entry = _diaryService.GetEntryByDate(date);
            Console.WriteLine(entry != null ? entry.ToString() : "Ingen anteckning hittades.");
        }

        private void UpdateEntry()
        {
            DateTime date = PromptForDate("Datum att uppdatera (yyyy-MM-dd): ");
            if (date == DateTime.MinValue) return;

            Console.Write("Ny text: ");
            string newText = Console.ReadLine();

            if (_diaryService.UpdateEntry(date, newText))
                Console.WriteLine("Anteckning uppdaterad.");
            else
                Console.WriteLine("Ingen anteckning hittades eller texten var tom.");
        }

        private void RemoveEntry()
        {
            DateTime date = PromptForDate("Datum att ta bort (yyyy-MM-dd): ");
            if (date == DateTime.MinValue) return;

            if (_diaryService.RemoveEntry(date))
                Console.WriteLine("Anteckning borttagen.");
            else
                Console.WriteLine("Ingen anteckning hittades.");
        }

        private DateTime PromptForDate(string prompt)
        {
            Console.Write(prompt);
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("Ogiltigt datum.");
                return DateTime.MinValue;
            }
            return date;
        }
    }
}

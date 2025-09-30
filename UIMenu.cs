using System;
using System.Collections.Generic;
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
            bool kör = true;

            while (kör)
            {
                VisaMeny();
                Console.Write("Val: ");
                string? val = Console.ReadLine();
                Console.WriteLine();

                switch (val)
                {
                    case "1": SkrivAnteckning(); break;
                    case "2": ListaAnteckningar(); break;
                    case "3": SökAnteckning(); break;
                    case "4": UppdateraAnteckning(); break;
                    case "5": TaBortAnteckning(); break;
                    case "6": SparaTillFil(); break;
                    case "7": LäsFrånFil(); break;
                    case "8":
                        SparaTillFil();
                        Console.WriteLine("Programmet avslutas.");
                        kör = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val.");
                        break;
                }

                if (kör)
                {
                    Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private void VisaMeny()
        {
            Console.WriteLine("=== Min Dagbok ===");
            Console.WriteLine("1. Skriv ny anteckning");
            Console.WriteLine("2. Lista alla anteckningar");
            Console.WriteLine("3. Sök anteckning på datum");
            Console.WriteLine("4. Uppdatera anteckning");
            Console.WriteLine("5. Ta bort anteckning");
            Console.WriteLine("6. Spara till fil");
            Console.WriteLine("7. Läs från fil");
            Console.WriteLine("8. Avsluta");
            Console.WriteLine("==================");
        }

        private void SkrivAnteckning()
        {
            DateTime datum = BegärDatum("Datum (yyyy-MM-dd): ");
            if (datum == DateTime.MinValue) return;

            Console.Write("Text: ");
            string? text = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Texten får inte vara tom.");
                return;
            }

            bool lyckades = _diaryService.AddEntry(datum, text);
            if (lyckades)
            {
                Console.WriteLine("Anteckning tillagd.");
                SparaTillFil();
            }
            else
            {
                Console.WriteLine("Kunde inte lägga till anteckning.");
            }
        }

        private void ListaAnteckningar()
        {
            List<DiaryEntry> lista = new List<DiaryEntry>(_diaryService.GetAllEntries());

            if (lista.Count == 0)
            {
                Console.WriteLine("Inga anteckningar.");
                return;
            }

            lista.Sort((a, b) => a.Date.CompareTo(b.Date));

            for (int i = 0; i < lista.Count; i++)
            {
                Console.WriteLine(lista[i].Date.ToString("yyyy-MM-dd") + " - " + lista[i].Text);
            }
        }

        private void SökAnteckning()
        {
            DateTime datum = BegärDatum("Datum att söka (yyyy-MM-dd): ");
            if (datum == DateTime.MinValue) return;

            DiaryEntry? post = _diaryService.GetEntryByDate(datum);
            if (post != null)
                Console.WriteLine(post.Date.ToString("yyyy-MM-dd") + " - " + post.Text);
            else
                Console.WriteLine("Ingen anteckning hittades.");
        }

        private void UppdateraAnteckning()
        {
            DateTime datum = BegärDatum("Datum att uppdatera (yyyy-MM-dd): ");
            if (datum == DateTime.MinValue) return;

            Console.Write("Ny text: ");
            string? nyText = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nyText))
            {
                Console.WriteLine("Texten får inte vara tom.");
                return;
            }

            bool lyckades = _diaryService.UpdateEntry(datum, nyText);
            if (lyckades)
            {
                Console.WriteLine("Anteckning uppdaterad.");
                SparaTillFil();
            }
            else
            {
                Console.WriteLine("Ingen anteckning hittades.");
            }
        }

        private void TaBortAnteckning()
        {
            DateTime datum = BegärDatum("Datum att ta bort (yyyy-MM-dd): ");
            if (datum == DateTime.MinValue) return;

            bool lyckades = _diaryService.RemoveEntry(datum);
            if (lyckades)
            {
                Console.WriteLine("Anteckning borttagen.");
                SparaTillFil();
            }
            else
            {
                Console.WriteLine("Ingen anteckning hittades.");
            }
        }

        private void SparaTillFil()
        {
            List<DiaryEntry> lista = new List<DiaryEntry>(_diaryService.GetAllEntries());
            _fileHandler.SaveEntries(lista);
            Console.WriteLine("Anteckningar sparade.");
        }

        private void LäsFrånFil()
        {
            List<DiaryEntry> lista = _fileHandler.LoadEntries();
            _diaryService.LoadFromFile(lista);
            Console.WriteLine("Anteckningar laddade från fil.");
        }


        private DateTime BegärDatum(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            DateTime datum;
            bool giltigt = DateTime.TryParse(input, out datum);

            if (!giltigt)
            {
                Console.WriteLine("Ogiltigt datumformat. Använd t.ex. 2025-09-30.");
                return DateTime.MinValue;
            }

            return datum;
        }
    }
}

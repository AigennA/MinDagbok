using System;
using System.Collections.Generic;
using System.Linq;

namespace MinDagbok
{
    public class DiaryService
    {
        private readonly List<DiaryEntry> _entries = new();
        private readonly Dictionary<DateTime, DiaryEntry> _entryDict = new();

        public IReadOnlyList<DiaryEntry> GetAllEntries() => _entries;

        public bool AddEntry(DateTime date, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            var entry = new DiaryEntry { Date = date, Text = text };
            _entries.Add(entry);
            _entryDict[date] = entry;
            return true;
        }

        public DiaryEntry? GetEntryByDate(DateTime date)
        {
            return _entryDict.TryGetValue(date, out var entry) ? entry : null;
        }

        public bool UpdateEntry(DateTime date, string newText)
        {
            if (_entryDict.ContainsKey(date) && !string.IsNullOrWhiteSpace(newText))
            {
                _entryDict[date].Text = newText;
                return true;
            }
            return false;
        }

        public bool RemoveEntry(DateTime date) //Ta bort anteckning
        {
            if (_entryDict.ContainsKey(date))
            {
                var entry = _entryDict[date];
                _entries.Remove(entry);
                _entryDict.Remove(date);
                return true;
            }
            return false;
        }

        public void LoadFromFile(List<DiaryEntry> loadedEntries)
        {
            _entries.Clear();
            _entryDict.Clear();
            foreach (var entry in loadedEntries)
            {
                _entries.Add(entry);
                _entryDict[entry.Date] = entry;
            }
        }
    }
}

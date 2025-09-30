namespace MinDagbok
{
    public class DiaryEntry
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd} - {Text}";
        }
    }
}

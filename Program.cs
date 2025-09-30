using MinDagbok;

class Program
{
    static void Main()
    {
        var diaryService = new DiaryService();
        var fileHandler = new FileHandler("diary.json");
        diaryService.LoadFromFile(fileHandler.LoadEntries());

        var uiMenu = new UIMenu(diaryService, fileHandler);
        uiMenu.Run();
    }
}

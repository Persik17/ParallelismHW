using ParallelismHW;

RandomFileGenerator.RandomTextFilesRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RandomTextFiles");
Console.WriteLine($"Root установлен в: {RandomFileGenerator.RandomTextFilesRoot}");

StopwatchHelper _stopwatchHelper = new();
RandomFileGenerator _randomFileGenerator = new(true);
TextFileReaderTask _reader = new(RandomFileGenerator.RandomTextFilesRoot);

List<string> fileNames = [];

//Указываем кол-во файлов для генерации
for (var i = 0; i < 3; i++)
{
    string fileName = await _randomFileGenerator.GenerateRandomTextFile(linesPerFile: 100);
    fileNames.Add(fileName);
    Console.WriteLine($"Файл {fileName} создан.");
}

Console.WriteLine();

List<Task> tasks = [];
foreach (var fileName in fileNames)
{
    tasks.Add(Task.Run(async () =>
    {
        string description = $"Чтение и подсчет пробелов в {fileName}.txt";

        await _stopwatchHelper.MeasureAndLog(async () =>
        {
            try
            {
                int whiteSpaceCount = await _reader.ReadTextFileAndGetWhiteSpaceCount(fileName);
                Console.WriteLine($"Количество пробелов в {fileName} - {whiteSpaceCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке файла {fileName}: {ex.Message}");
            }
        }, fileName);
    }));
}

await Task.WhenAll(tasks);
Console.WriteLine();
Console.WriteLine("Все задачи завершены.");

Console.ReadLine();
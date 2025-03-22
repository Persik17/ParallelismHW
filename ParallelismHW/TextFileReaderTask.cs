using System.Text;

namespace ParallelismHW;

public class TextFileReaderTask(string rootForRead)
{
    private readonly string _rootForRead = rootForRead;

    public async Task<int> ReadTextFileAndGetWhiteSpaceCount(string fileName)
    {
        string filePath = Path.Combine(_rootForRead, fileName);

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Ошибка: Файл не найден по пути {filePath}");
            return 0;
        }

        try
        {
            using StreamReader reader = new(filePath, Encoding.UTF8);
            string text = await reader.ReadToEndAsync();

            int whiteSpaceCount = CountWhiteSpacesInTextFile(text);
            return whiteSpaceCount;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении {fileName}: {ex.Message}");
            return 0;
        }
    }

    private static int CountWhiteSpacesInTextFile(string text) => text.Count(x => x == ' ');
}


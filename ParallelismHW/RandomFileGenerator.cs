using System.Text;

namespace ParallelismHW;

public class RandomFileGenerator
{
    private readonly Random _random = new();
    private const string chars = " A B C D E F G H I J K L M N O P Q R S T U V W X Y Z a b c d e f g h i j k l m n o p q r s t u v w x y z 0 1 2 3 4 5 6 7 8 9 ";
    public static string RandomTextFilesRoot { get; set; }

    public RandomFileGenerator(bool needClearDirectory = true)
    {
        if (string.IsNullOrEmpty(RandomTextFilesRoot))
        {
            throw new InvalidOperationException("Не задан корневой путь.");
        }

        if (!Directory.Exists(RandomTextFilesRoot))
        {
            Directory.CreateDirectory(RandomTextFilesRoot);
        }

        if (needClearDirectory)
        {
            CleanDirectory(RandomTextFilesRoot);
        }
    }

    public async Task<string> GenerateRandomTextFile(string? fileName = null, int fileSizeInKB = 10, int linesPerFile = 50)
    {
        fileName ??= $"random_{Guid.NewGuid()}.txt";

        string filePath = Path.Combine(RandomTextFilesRoot, fileName);

        try
        {
            if (linesPerFile <= 0)
            {
                throw new ArgumentException("linesPerFile должно быть больше нуля.");
            }

            int lineSizeBytes = fileSizeInKB * 1024 / linesPerFile;

            using (StreamWriter writer = new(filePath, false, Encoding.UTF8, bufferSize: 4096))
            {
                for (int i = 0; i < linesPerFile; i++)
                {
                    string randomLine = GenerateRandomLine(lineSizeBytes);
                    await writer.WriteLineAsync(randomLine);
                }
            }

            Console.WriteLine($"{fileName} создан в {RandomTextFilesRoot}");
            return fileName;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании {fileName}: {ex.Message}");
            return string.Empty;
        }
    }

    private static void CleanDirectory(string path)
    {
        try
        {
            DirectoryInfo directory = new(path);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                subDirectory.Delete(true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при очистке директории {path}: {ex.Message}");
        }
    }


    private string GenerateRandomLine(int lineLength)
    {
        StringBuilder stringBuilder = new();

        for (var i = 0; i < lineLength; i++)
        {
            stringBuilder.Append(chars[_random.Next(chars.Length)]);
        }

        return stringBuilder.ToString();
    }
}


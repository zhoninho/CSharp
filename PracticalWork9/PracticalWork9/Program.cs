// Program.cs
using PracticalWork9;
using System;
using System.IO; // Для DirectoryInfo, FileInfo, Path
using System.Linq; // Может понадобиться для работы с коллекциями

// enum TimeFrame { Year, TwoYears, Long } // Убедитесь, что он определен (возможно, в отдельном файле или здесь)

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("--- Практическая работа №9: Система ввода-вывода ---");

        // 1. Создать объект типа T (ResearchTeam) с непустым списком элементов.
        ResearchTeam team = new ResearchTeam("Исследование графена", "НИИ Физики", 777, TimeFrame.Long);
        team.AddMembers(
            new Person("Алексей", "Волков", new DateTime(1985, 3, 12)),
            new Person("Мария", "Сидорова", new DateTime(1990, 7, 22))
        );
        team.AddPapers(
            new Paper("Свойства графена при низких температурах", team.Members[0], new DateTime(2023, 5, 10)),
            new Paper("Применение графена в электронике", team.Members[1], new DateTime(2024, 1, 15))
        );

        Console.WriteLine("\n--- 1. Исходный объект ---");
        Console.WriteLine(team.ToString());

        // Создать полную копию объекта с помощью метода DeepCopy()
        ResearchTeam teamCopy = team.DeepCopy();
        // Модифицируем копию, чтобы убедиться, что она независима
        teamCopy.ResearchTopic = "Копия: Исследование графена (изменено)";
        if (teamCopy.Publications.Count > 0)
        {
            teamCopy.Publications[0].Title = "Копия: Свойства графена (изменено)";
        }

        Console.WriteLine("\n--- 1. Копия объекта (после модификации копии) ---");
        Console.WriteLine(teamCopy.ToString());
        Console.WriteLine("\n--- 1. Исходный объект (после модификации копии - должен остаться без изменений) ---");
        Console.WriteLine(team.ToString());


        // 2. Предложить пользователю ввести имя файла
        Console.Write("\n--- 2. Введите имя файла для сохранения/загрузки (например, team_data.json): ");
        string filename = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(filename))
        {
            filename = "default_team_data.json";
            Console.WriteLine($"Имя файла не введено, используется имя по умолчанию: {filename}");
        }

        string directoryPath = "SavedTeamsData"; // Папка для сохранения файлов
        Directory.CreateDirectory(directoryPath); // Создаем папку, если ее нет
        string fullPath = Path.Combine(directoryPath, filename);


        if (!File.Exists(fullPath))
        {
            Console.WriteLine($"Файла '{fullPath}' не существует. Он будет создан при первой операции сохранения.");
            // Попытка создать пустой файл для демонстрации, хотя Save его все равно создаст/перезапишет
            try
            {
                File.Create(fullPath).Close(); // Создаем и сразу закрываем
                Console.WriteLine($"Пустой файл '{fullPath}' создан.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось создать файл '{fullPath}': {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Файл '{fullPath}' существует. Попытка загрузки данных...");
            if (team.Load(fullPath))
            {
                Console.WriteLine("Данные успешно загружены из файла.");
            }
            else
            {
                Console.WriteLine("Не удалось загрузить данные из файла. Объект остался без изменений.");
            }
        }

        // 3. Вывести объект T.
        Console.WriteLine("\n--- 3. Объект после шага 2 (загрузки или инициализации) ---");
        Console.WriteLine(team.ToString());

        // 4. Для этого же объекта T сначала вызвать метод AddFromConsole(), затем метод Save(string filename).
        Console.WriteLine("\n--- 4. Добавление элемента с консоли и сохранение ---");
        if (team.AddFromConsole())
        {
            Console.WriteLine("Элемент добавлен с консоли.");
        }
        else
        {
            Console.WriteLine("Не удалось добавить элемент с консоли.");
        }

        if (team.Save(fullPath))
        {
            Console.WriteLine($"Объект успешно сохранен в файл '{fullPath}'.");
        }
        else
        {
            Console.WriteLine($"Не удалось сохранить объект в файл '{fullPath}'.");
        }
        Console.WriteLine("\nОбъект T после AddFromConsole и Save:");
        Console.WriteLine(team.ToString());

        // 5. Вызвать последовательно статический метод Load, AddFromConsole, статический метод Save.
        Console.WriteLine("\n--- 5. Статическая загрузка, добавление с консоли, статическое сохранение ---");
        Console.WriteLine($"Попытка статической загрузки из '{fullPath}'...");
        if (ResearchTeam.Load(fullPath, team)) // Передаем тот же объект team
        {
            Console.WriteLine("Статическая загрузка успешно выполнена.");
        }
        else
        {
            Console.WriteLine("Не удалось выполнить статическую загрузку.");
        }
        Console.WriteLine("Объект T после статической загрузки:");
        Console.WriteLine(team.ToString());

        Console.WriteLine("\nДобавление еще одного элемента с консоли...");
        if (team.AddFromConsole())
        {
            Console.WriteLine("Элемент добавлен с консоли.");
        }
        else
        {
            Console.WriteLine("Не удалось добавить элемент с консоли.");
        }

        Console.WriteLine($"Попытка статического сохранения в '{fullPath}'...");
        if (ResearchTeam.Save(fullPath, team))
        {
            Console.WriteLine($"Объект успешно сохранен статическим методом в файл '{fullPath}'.");
        }
        else
        {
            Console.WriteLine($"Не удалось сохранить объект статическим методом в файл '{fullPath}'.");
        }

        // 6. Вывести объект T.
        Console.WriteLine("\n--- 6. Объект T после всех операций ---");
        Console.WriteLine(team.ToString());

        // 7. Для папки с сохраненными файлами рекурсивно выдать список её файлов и подпапок.
        Console.WriteLine($"\n--- 7. Содержимое папки '{directoryPath}' ---");
        ListDirectoryContents(directoryPath, "");


        Console.WriteLine("\nНажмите любую клавишу для завершения...");
        Console.ReadKey();
    }

    /// <summary>
    /// Рекурсивно выводит содержимое указанной директории.
    /// </summary>
    /// <param name="path">Путь к директории.</param>
    /// <param name="indent">Отступ для форматирования вывода.</param>
    static void ListDirectoryContents(string path, string indent)
    {
        try
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                Console.WriteLine($"{indent}Папка '{path}' не найдена.");
                return;
            }

            Console.WriteLine($"{indent}[{dirInfo.Name}]");

            // Список подпапок
            foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
            {
                ListDirectoryContents(subDir.FullName, indent + "  ");
            }

            // Список файлов
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                Console.WriteLine($"{indent}  - {file.Name} ({file.Length} байт, {file.LastWriteTime})");
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"{indent}Ошибка: нет доступа к папке '{path}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{indent}Ошибка при чтении содержимого папки '{path}': {ex.Message}");
        }
    }
}

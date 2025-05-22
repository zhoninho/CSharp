using PracticalWork7; // Используем пространство имен, где определены все классы
using System;
using System.Collections.Generic;

// Определение перечисления TimeFrame на уровне пространства имен или внутри класса Program
// В данном случае, оно уже было в файле Program.cs из предыдущей работы
enum TimeFrame { Year, TwoYears, Long }


class Program
{
    // Метод для получения валидного числового ввода от пользователя
    static int GetValidInput()
    {
        int count;
        while (true)
        {
            Console.Write("Введите количество элементов для тестовых коллекций (TestCollections): ");
            try
            {
                count = int.Parse(Console.ReadLine());
                if (count >= 0)
                    return count;
                Console.WriteLine("Ошибка: Число должно быть неотрицательным.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Пожалуйста, введите корректное целое число.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
            }
        }
    }

    static void Main()
    {
        // Установка кодировки вывода консоли для корректного отображения кириллицы
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        // --- ЗАДАНИЕ 7: РАБОТА С СОБЫТИЯМИ ---

        // 1. Создать две коллекции ResearchTeamCollection.
        ResearchTeamCollection collection1 = new ResearchTeamCollection("Коллекция Alpha");
        ResearchTeamCollection collection2 = new ResearchTeamCollection("Коллекция Beta");

        // 2. Создать два объекта типа TeamsJournal.
        TeamsJournal journal1 = new TeamsJournal();
        TeamsJournal journal2 = new TeamsJournal();

        // Подписка journal1 на события ResearchTeamAdded и ResearchTeamInserted из первой коллекции ResearchTeamCollection (collection1).
        collection1.ResearchTeamAdded += journal1.HandleTeamEvent;
        collection1.ResearchTeamInserted += journal1.HandleTeamEvent;

        // Подписка journal2 на события ResearchTeamInserted из обеих коллекций ResearchTeamCollection.
        collection1.ResearchTeamInserted += journal2.HandleTeamEvent; // journal2 слушает Inserted из collection1
        collection2.ResearchTeamInserted += journal2.HandleTeamEvent; // journal2 слушает Inserted из collection2

        Console.WriteLine("--- Начало демонстрации событий из Задания 7 ---");

        // 3. Внести изменения в коллекции ResearchTeamCollection

        // === Работа с collection1 ===
        Console.WriteLine($"\n--- Манипуляции с '{collection1.CollectionName}' ---");

        // Добавить элементы в коллекцию (AddDefaults)
        Console.WriteLine($"\nВызов AddDefaults() для '{collection1.CollectionName}':");
        collection1.AddDefaults(); // Ожидается 3 события ResearchTeamAdded для journal1

        // Добавить элементы (AddResearchTeams)
        Person authorRT1 = new Person("Сергей", "Петров", new DateTime(1990, 5, 20));
        ResearchTeam rtToAdd1 = new ResearchTeam("Квантовые вычисления", "НИИ Физики", 1010, TimeFrame.Long);
        rtToAdd1.AddMembers(authorRT1);
        rtToAdd1.AddPapers(new Paper("Основы квантов", authorRT1, DateTime.Now.AddMonths(-6)));

        ResearchTeam rtToAdd2 = new ResearchTeam("Нейросети", "Лаборатория ИИ", 1011, TimeFrame.TwoYears);
        Console.WriteLine($"\nВызов AddResearchTeams() для '{collection1.CollectionName}':");
        collection1.AddResearchTeams(rtToAdd1, rtToAdd2); // Ожидается 2 события ResearchTeamAdded для journal1

        // С помощью метода InsertAt (int j, ResearchTeam rt) перед элементом с номером j, который есть в коллекции, вставить новый элемент.
        ResearchTeam rtToInsertExisting = new ResearchTeam("Вставка перед существующим", "Институт Инноваций", 1012, TimeFrame.Year);
        Console.WriteLine($"\nВызов InsertAt(1, ...) для '{collection1.CollectionName}':");
        // Допустим, в collection1 сейчас есть элементы [0], [1], [2], ...
        // Вставляем rtToInsertExisting перед элементом с текущим индексом 1.
        collection1.InsertAt(1, rtToInsertExisting); // Ожидается 1 событие ResearchTeamInserted для journal1 и journal2

        // Вызвать метод InsertAt (int j, ResearchTeam rt) с номером j, которого нет в коллекции (например, j слишком большое).
        ResearchTeam rtToInsertNonExistingLarge = new ResearchTeam("Вставка по несуществующему (большой индекс)", "Удаленная Лаборатория", 1013, TimeFrame.Year);
        Console.WriteLine($"\nВызов InsertAt(99, ...) для '{collection1.CollectionName}': (индекс 99 не существует, добавление в конец)");
        collection1.InsertAt(99, rtToInsertNonExistingLarge); // Ожидается 1 событие ResearchTeamAdded для journal1

        // Вызвать метод InsertAt (int j, ResearchTeam rt) с номером j, который указывает на позицию "в конец списка".
        ResearchTeam rtToInsertAtEnd = new ResearchTeam("Вставка в конец по индексу Count", "Финальный Проект", 1014, TimeFrame.TwoYears);
        Console.WriteLine($"\nВызов InsertAt(collection1.GetTeamsList().Count, ...) для '{collection1.CollectionName}': (вставка в самый конец)");
        collection1.InsertAt(collection1.GetTeamsList().Count, rtToInsertAtEnd); // Ожидается 1 событие ResearchTeamAdded для journal1

        // === Работа с collection2 ===
        Console.WriteLine($"\n--- Манипуляции с '{collection2.CollectionName}' ---");

        // Добавим пару элементов в collection2, чтобы было с чем работать для InsertAt
        // Эти события ResearchTeamAdded не будут отловлены journal2, т.к. он не подписан на них.
        ResearchTeam rtBeta1 = new ResearchTeam("Биоинформатика", "Центр БиоТех", 2020, TimeFrame.Long);
        collection2.AddResearchTeams(rtBeta1);
        Console.WriteLine($"\nВызов AddResearchTeams() для '{collection2.CollectionName}' (journal2 не слушает Added).");


        ResearchTeam rtToInsertBeta = new ResearchTeam("Вставка в Beta", "Институт BetaТест", 2021, TimeFrame.Year);
        Console.WriteLine($"\nВызов InsertAt(0, ...) для '{collection2.CollectionName}': (вставка перед существующим элементом)");
        collection2.InsertAt(0, rtToInsertBeta); // Ожидается 1 событие ResearchTeamInserted для journal2

        ResearchTeam rtToInsertBetaNonExisting = new ResearchTeam("Вставка в Beta (несущ. индекс)", "Лаб BetaКрай", 2022, TimeFrame.Year);
        Console.WriteLine($"\nВызов InsertAt(-5, ...) для '{collection2.CollectionName}': (индекс -5 не существует, добавление в конец)");
        collection2.InsertAt(-5, rtToInsertBetaNonExisting); // Событие ResearchTeamAdded, journal2 его не слушает.

        Console.WriteLine("\n--- Содержимое журналов после всех операций ---");

        // 4. Вывести данные обоих объектов TeamsJournal.
        Console.WriteLine("\n--- Журнал 1 (слушает Added и Inserted из 'Коллекция Alpha') ---");
        Console.WriteLine(journal1.ToString());

        Console.WriteLine("\n--- Журнал 2 (слушает Inserted из 'Коллекция Alpha' и 'Коллекция Beta') ---");
        Console.WriteLine(journal2.ToString());

        // --- Демонстрация остального функционала (из предыдущей работы) ---
        Console.WriteLine("\n\n--- Демонстрация работы коллекций (содержимое и базовые операции) ---");
        Console.WriteLine($"\nСодержимое '{collection1.CollectionName}':");
        Console.WriteLine(collection1.ToString());

        Console.WriteLine($"\nСодержимое '{collection2.CollectionName}':");
        Console.WriteLine(collection2.ToString());

        if (collection1.GetTeamsList().Count > 0)
        {
            Console.WriteLine($"\nЭлемент по индексу 0 в '{collection1.CollectionName}':");
            Console.WriteLine(collection1[0].ToShortString());

            Console.WriteLine($"\nЗамена элемента по индексу 0 в '{collection1.CollectionName}' (событие НЕ вызывается):");
            ResearchTeam rtReplace = new ResearchTeam("Замещенная Тема", "Организация Замены", 9999, TimeFrame.Long);
            collection1[0] = rtReplace;
            Console.WriteLine($"Новый элемент по индексу 0 в '{collection1.CollectionName}':");
            Console.WriteLine(collection1[0].ToShortString());
        }

        // Пример сортировки для collection1
        if (collection1.GetTeamsList().Count > 1)
        {
            Console.WriteLine($"\nСортировка '{collection1.CollectionName}' по Регистрационному Номеру:");
            collection1.SortByRegistrationNumber();
            Console.WriteLine(collection1.ToShortString());
        }

        // TestCollections (если нужно)
        // Console.WriteLine("\n--- TestCollections (измерение времени поиска) ---");
        // int numElementsForTest = GetValidInput();
        // if (numElementsForTest > 0)
        // {
        //    TestCollections testCollections = new TestCollections(numElementsForTest);
        //    testCollections.MeasureSearchTime();
        // } else {
        //    Console.WriteLine("Пропуск TestCollections, так как введено 0 элементов.");
        // }
    }
}
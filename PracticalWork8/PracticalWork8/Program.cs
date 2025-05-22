// Program.cs
using PracticalWork8; // Используем пространство имен, где определены все классы
using System;
using System.Collections.Generic;

// Определение перечисления TimeFrame на уровне пространства имен или внутри класса Program
// Это было в файле Program.cs из предыдущей работы
// enum TimeFrame { Year, TwoYears, Long } // Уже должно быть определено в другом файле или здесь

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("--- Практическая работа №8: Делегаты и События (Повышенный уровень) ---");

        // 1. Создать две коллекции ResearchTeamCollection<string>.
        // В качестве ключа будем использовать RegistrationNumber.ToString().
        ResearchTeamCollection<string> collection1 = new ResearchTeamCollection<string>("Коллекция Alpha (Gen)");
        ResearchTeamCollection<string> collection2 = new ResearchTeamCollection<string>("Коллекция Beta (Gen)");

        // 2. Создать объект TeamsJournal, подписать его на события ResearchTeamsChanged из обоих объектов.
        TeamsJournal journal = new TeamsJournal();
        collection1.ResearchTeamsChanged += journal.HandleResearchTeamsChanged;
        collection2.ResearchTeamsChanged += journal.HandleResearchTeamsChanged;

        Console.WriteLine("\n--- Начальные состояния коллекций ---");
        Console.WriteLine(collection1.ToString());
        Console.WriteLine(collection2.ToString());
        Console.WriteLine(journal.ToString()); // Журнал должен быть пуст

        // 3. Внести изменения в коллекции ResearchTeamCollection<string>

        // --- Добавить элементы в коллекции ---
        Console.WriteLine("\n--- 3.1. Добавление элементов ---");
        ResearchTeam rt1 = new ResearchTeam("Квантовая физика", "МГУ", 101, TimeFrame.Long);
        ResearchTeam rt2 = new ResearchTeam("Биоинженерия", "СПбГУ", 102, TimeFrame.TwoYears);
        ResearchTeam rt3 = new ResearchTeam("Искусственный интеллект", "ВШЭ", 103, TimeFrame.Year);

        collection1.Add(rt1.RegistrationNumber.ToString(), rt1);
        collection1.Add(rt2.RegistrationNumber.ToString(), rt2);
        Console.WriteLine($"Добавлены rt1 (РН {rt1.RegistrationNumber}) и rt2 (РН {rt2.RegistrationNumber}) в '{collection1.CollectionName}'. Событие 'Add' не генерируется по заданию.");

        collection2.Add(rt3.RegistrationNumber.ToString(), rt3);
        ResearchTeam rt4_for_coll2 = new ResearchTeam("Нейролингвистика", "РГГУ", 104, TimeFrame.TwoYears);
        collection2.Add(rt4_for_coll2.RegistrationNumber.ToString(), rt4_for_coll2);
        Console.WriteLine($"Добавлены rt3 (РН {rt3.RegistrationNumber}) и rt4 (РН {rt4_for_coll2.RegistrationNumber}) в '{collection2.CollectionName}'.");

        Console.WriteLine("\nСостояние после добавления:");
        Console.WriteLine(collection1.ToString());
        Console.WriteLine(collection2.ToString());
        Console.WriteLine(journal.ToString()); // Журнал все еще пуст, т.к. Add не вызывает событие

        // --- Изменить значения разных свойств элементов, входящих в коллекцию ---
        Console.WriteLine("\n--- 3.2. Изменение свойств элементов ---");
        if (collection1.Count > 0)
        {
            Console.WriteLine($"Изменение ResearchTopic у rt1 (РН {rt1.RegistrationNumber}) в '{collection1.CollectionName}'...");
            rt1.ResearchTopic = "Новая Квантовая Физика"; // Должно вызвать PropertyChanged в rt1 -> ResearchTeamsChanged в collection1

            Console.WriteLine($"Изменение Duration у rt2 (РН {rt2.RegistrationNumber}) в '{collection1.CollectionName}'...");
            rt2.Duration = TimeFrame.Long; // Должно вызвать PropertyChanged в rt2 -> ResearchTeamsChanged в collection1
        }
        if (collection2.Count > 0)
        {
            Console.WriteLine($"Изменение ResearchTopic у rt3 (РН {rt3.RegistrationNumber}) в '{collection2.CollectionName}'...");
            rt3.ResearchTopic = "Продвинутый ИИ"; // Должно вызвать PropertyChanged в rt3 -> ResearchTeamsChanged в collection2
        }

        Console.WriteLine("\nСостояние после изменения свойств:");
        Console.WriteLine(journal.ToString());

        // --- Удалить элемент из коллекции ---
        Console.WriteLine("\n--- 3.3. Удаление элемента ---");
        if (collection1.Count > 0)
        {
            Console.WriteLine($"Удаление rt1 (РН {rt1.RegistrationNumber}) из '{collection1.CollectionName}'...");
            bool removed = collection1.Remove(rt1); // Удаляем rt1, должно вызвать ResearchTeamsChanged (Remove)
            Console.WriteLine(removed ? "rt1 успешно удален." : "rt1 не найден для удаления.");
        }

        Console.WriteLine("\nСостояние после удаления rt1:");
        Console.WriteLine(collection1.ToString());
        Console.WriteLine(journal.ToString());

        // --- Изменить данные в удаленном элементе ---
        Console.WriteLine("\n--- 3.4. Изменение данных в УДАЛЕННОМ элементе (rt1) ---");
        Console.WriteLine($"Попытка изменить ResearchTopic у удаленного rt1 (РН {rt1.RegistrationNumber})...");
        rt1.ResearchTopic = "Тема удаленного элемента"; // Это изменит свойство самого объекта rt1,
                                                        // но collection1 уже отписалась от его PropertyChanged.
                                                        // В журнале НЕ должно быть новой записи от collection1 для этого изменения.
        Console.WriteLine($"Новая тема rt1 (объекта в памяти): {rt1.ResearchTopic}");

        Console.WriteLine("\nСостояние после изменения удаленного rt1:");
        Console.WriteLine(journal.ToString()); // Проверяем, что новых записей от collection1 нет

        // --- Заменить один из элементов коллекции ---
        Console.WriteLine("\n--- 3.5. Замена элемента ---");
        ResearchTeam rtNewForReplace = new ResearchTeam("Прикладная Математика", "ФизТех", rt2.RegistrationNumber, TimeFrame.Year); // Новый объект с тем же рег.номером, что и rt2
        if (collection1.Count > 0 && collection1[rt2.RegistrationNumber.ToString()] != null) // Убедимся, что rt2 еще в коллекции
        {
            Console.WriteLine($"Замена rt2 (РН {rt2.RegistrationNumber}) на rtNew (РН {rtNewForReplace.RegistrationNumber}) в '{collection1.CollectionName}' с использованием индексатора...");
            // Замена через индексатор
            collection1[rt2.RegistrationNumber.ToString()] = rtNewForReplace; // Должно вызвать ResearchTeamsChanged (Replace)
                                                                              // и отписаться от rt2, подписаться на rtNewForReplace
        }
        else
        {
            Console.WriteLine($"rt2 (РН {rt2.RegistrationNumber}) не найден в '{collection1.CollectionName}' для замены или коллекция пуста.");
            // Если rt2 был удален ранее или не добавлялся, то добавим rtNewForReplace как новый элемент для дальнейших тестов
            if (rt2 != null) // Проверка, чтобы RegistrationNumber был доступен
            {
                collection1.Add(rtNewForReplace.RegistrationNumber.ToString(), rtNewForReplace);
                Console.WriteLine($"rtNewForReplace (РН {rtNewForReplace.RegistrationNumber}) добавлен в '{collection1.CollectionName}'.");
            }
        }


        Console.WriteLine("\nСостояние после замены rt2:");
        Console.WriteLine(collection1.ToString());
        Console.WriteLine(journal.ToString());

        // --- Изменить данные в элементе, который был удален из коллекции при замене элемента ---
        Console.WriteLine("\n--- 3.6. Изменение данных в элементе, который был УДАЛЕН ПРИ ЗАМЕНЕ (rt2) ---");
        Console.WriteLine($"Попытка изменить ResearchTopic у rt2 (РН {rt2.RegistrationNumber}), который был заменен...");
        rt2.ResearchTopic = "Тема замененного элемента"; // collection1 должна была отписаться от rt2.
                                                         // В журнале НЕ должно быть новой записи от collection1.
        Console.WriteLine($"Новая тема rt2 (объекта в памяти): {rt2.ResearchTopic}");

        Console.WriteLine("\nПроверка изменения данных в новом элементе (rtNewForReplace), который пришел на замену:");
        Console.WriteLine($"Изменение Duration у rtNewForReplace (РН {rtNewForReplace.RegistrationNumber}) в '{collection1.CollectionName}'...");
        rtNewForReplace.Duration = TimeFrame.TwoYears; // Должно вызвать событие от collection1, т.к. она на него подписана.

        Console.WriteLine("\nСостояние после всех манипуляций:");
        Console.WriteLine(journal.ToString());

        // --- Демонстрация метода Replace ---
        Console.WriteLine("\n--- 3.7. Демонстрация метода Replace ---");
        if (collection2.Count > 0)
        {
            ResearchTeam rtToReplaceInC2 = collection2[rt4_for_coll2.RegistrationNumber.ToString()]; // Получаем элемент для замены
            if (rtToReplaceInC2 != null)
            {
                ResearchTeam rtReplacement = new ResearchTeam("Новая Нейролингвистика", "МПГУ", rt4_for_coll2.RegistrationNumber, TimeFrame.Long);
                Console.WriteLine($"Замена rt4 (РН {rtToReplaceInC2.RegistrationNumber}) на новый элемент (РН {rtReplacement.RegistrationNumber}) в '{collection2.CollectionName}' методом Replace...");
                bool replaced = collection2.Replace(rtToReplaceInC2, rtReplacement);
                Console.WriteLine(replaced ? "Замена rt4 прошла успешно." : "Замена rt4 не удалась.");

                Console.WriteLine("\nИзменение замененного (старого) rt4 (объекта в памяти):");
                rtToReplaceInC2.ResearchTopic = "Старая тема замененного"; // Не должно логироваться
                Console.WriteLine($"Новая тема старого rt4: {rtToReplaceInC2.ResearchTopic}");


                Console.WriteLine("\nИзменение нового элемента (rtReplacement), который пришел на замену:");
                rtReplacement.ResearchTopic = "Супер Новая Нейролингвистика"; // Должно логироваться

            }
        }
        Console.WriteLine("\nФинальное состояние журнала:");
        Console.WriteLine(journal.ToString());
    }
}
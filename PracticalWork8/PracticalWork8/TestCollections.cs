// TestCollections.cs
using PracticalWork8;
using System;
using System.Collections.Generic; // Для List<T> и Dictionary<K,V>
using System.Diagnostics;       // Для Stopwatch

namespace PracticalWork8
{
    // Класс TestCollections предназначен для тестирования производительности
    // различных типов коллекций при поиске элементов.
    public class TestCollections
    {
        // Список ключей типа Team.
        private List<Team> teamList;
        // Список ключей типа string.
        private List<string> stringList;
        // Словарь с ключами типа Team и значениями типа ResearchTeam.
        private Dictionary<Team, ResearchTeam> teamDict;
        // Словарь с ключами типа string и значениями типа ResearchTeam.
        private Dictionary<string, ResearchTeam> stringDict;

        // Конструктор, инициализирующий коллекции заданным количеством элементов.
        public TestCollections(int count)
        {
            teamList = new List<Team>(count); // Инициализация с начальной емкостью
            stringList = new List<string>(count);
            teamDict = new Dictionary<Team, ResearchTeam>(count);
            stringDict = new Dictionary<string, ResearchTeam>(count);

            // Заполнение коллекций.
            for (int i = 0; i < count; i++)
            {
                // Генерация уникального элемента ResearchTeam.
                ResearchTeam rt = GenerateElement(i);
                // Ключ типа Team (извлекается из данных ResearchTeam).
                Team keyAsTeam = rt.TeamData;
                // Ключ типа string (преобразуется из Team).
                string keyAsString = keyAsTeam.ToString(); // Используем ToString() для генерации строкового ключа.

                // Добавление элементов в списки.
                teamList.Add(keyAsTeam);
                stringList.Add(keyAsString);

                // Добавление элементов в словари.
                // Важно: ключи в словаре должны быть уникальны.
                // GenerateElement(i) гарантирует уникальность registrationNumber,
                // что, при корректной реализации GetHashCode/Equals для Team,
                // обеспечивает уникальность ключей Team. Строковые ключи также будут уникальны.
                // Дополнительная проверка `ContainsKey` не обязательна, если уникальность гарантирована.
                teamDict.Add(keyAsTeam, rt);
                stringDict.Add(keyAsString, rt);
            }
        }

        // Статический метод для генерации тестового элемента ResearchTeam.
        // Параметр value используется для обеспечения уникальности генерируемых данных.
        public static ResearchTeam GenerateElement(int value)
        {
            ResearchTeam rt = new ResearchTeam(
                $"Тема_{value}",        // Уникальная тема исследования.
                $"Организация_{value}", // Уникальное название организации.
                value + 1,             // Уникальный регистрационный номер (начиная с 1).
                TimeFrame.Year         // Фиксированная продолжительность исследования.
            );
            // Можно добавить генерацию участников и публикаций для большей реалистичности,
            // но для целей теста поиска ключей/значений это не обязательно.
            return rt;
        }

        // Приватный статический метод для преобразования тиков Stopwatch в миллисекунды.
        // Округляет результат до 3 знаков после запятой для удобства чтения.
        private static double TicksToMilliseconds(long ticks)
        {
            // 1 тик Stopwatch = 100 наносекунд (10^-7 секунды).
            // Миллисекунды = тики * (100 * 10^-9 с) / (10^-3 с) = тики * 10^-4.
            return Math.Round(ticks * 1e-4, 3);
        }

        // Метод для измерения и вывода времени поиска элементов в различных коллекциях.
        public void MeasureSearchTime()
        {
            // Если коллекции пусты (например, TestCollections(0)), измерение невозможно.
            if (teamList.Count == 0)
            {
                Console.WriteLine("Коллекции пусты. Измерение времени поиска не будет произведено.");
                return;
            }

            // --- Подготовка элементов для поиска ---
            // Первый, средний, последний и несуществующий элементы.
            Team firstKey = teamList[0];
            Team middleKey = teamList[teamList.Count / 2];
            Team lastKey = teamList[teamList.Count - 1];
            // Несуществующий ключ Team (с номером, которого точно нет в коллекции).
            Team nonExistentKey = new Team("НесуществующаяОрг", teamList.Count + 100);

            string firstStringKey = stringList[0];
            string middleStringKey = stringList[stringList.Count / 2];
            string lastStringKey = stringList[stringList.Count - 1];
            // Несуществующий строковый ключ.
            string nonExistentStringKey = new Team("НесуществующаяОргСтрока", stringList.Count + 101).ToString();

            // Значения для поиска (для метода ContainsValue в словаре).
            // Убедимся, что эти значения действительно присутствуют в словаре.
            ResearchTeam firstValue = teamDict[firstKey];
            ResearchTeam middleValue = teamDict[middleKey];
            ResearchTeam lastValue = teamDict[lastKey];
            // Гарантированно несуществующее значение ResearchTeam.
            ResearchTeam nonExistentValue = GenerateElement(teamList.Count + 200);

            Stopwatch sw = new Stopwatch(); // Объект для измерения времени.
            bool found; // Переменная для хранения результата поиска (найден/не найден).

            Console.WriteLine($"\n--- Измерение времени поиска для {teamList.Count} элементов ---");

            // --- Поиск в List<Team> (метод Contains, линейный поиск O(N)) ---
            Console.WriteLine("\nВремя поиска в List<Team> (метод Contains):");
            sw.Restart(); found = teamList.Contains(firstKey); sw.Stop();
            Console.WriteLine($"  Первый элемент: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            sw.Restart(); found = teamList.Contains(middleKey); sw.Stop();
            Console.WriteLine($"  Средний элемент: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            sw.Restart(); found = teamList.Contains(lastKey); sw.Stop();
            Console.WriteLine($"  Последний элемент: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            sw.Restart(); found = teamList.Contains(nonExistentKey); sw.Stop();
            Console.WriteLine($"  Несуществующий: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");

            // --- Поиск в List<string> (метод Contains, линейный поиск O(N)) ---
            Console.WriteLine("\nВремя поиска в List<string> (метод Contains):");
            sw.Restart(); found = stringList.Contains(firstStringKey); sw.Stop();
            Console.WriteLine($"  Первый элемент: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            // ... (аналогично для среднего, последнего, несуществующего) ...
            sw.Restart(); found = stringList.Contains(middleStringKey); sw.Stop();
            Console.WriteLine($"  Средний элемент: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            sw.Restart(); found = stringList.Contains(lastStringKey); sw.Stop();
            Console.WriteLine($"  Последний элемент: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            sw.Restart(); found = stringList.Contains(nonExistentStringKey); sw.Stop();
            Console.WriteLine($"  Несуществующий: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");


            // --- Поиск по ключу в Dictionary<Team, ResearchTeam> (ContainsKey, в среднем O(1)) ---
            Console.WriteLine("\nВремя поиска по ключу в Dictionary<Team, ResearchTeam> (ContainsKey):");
            sw.Restart(); found = teamDict.ContainsKey(firstKey); sw.Stop();
            Console.WriteLine($"  Первый ключ: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            // ... (аналогично) ...
            sw.Restart(); found = teamDict.ContainsKey(middleKey); sw.Stop();
            Console.WriteLine($"  Средний ключ: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            sw.Restart(); found = teamDict.ContainsKey(lastKey); sw.Stop();
            Console.WriteLine($"  Последний ключ: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            sw.Restart(); found = teamDict.ContainsKey(nonExistentKey); sw.Stop();
            Console.WriteLine($"  Несуществующий: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");

            // --- Поиск по ключу в Dictionary<string, ResearchTeam> (ContainsKey, в среднем O(1)) ---
            Console.WriteLine("\nВремя поиска по ключу в Dictionary<string, ResearchTeam> (ContainsKey):");
            sw.Restart(); found = stringDict.ContainsKey(firstStringKey); sw.Stop();
            Console.WriteLine($"  Первый ключ: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            // ... (аналогично) ...
            sw.Restart(); found = stringDict.ContainsKey(middleStringKey); sw.Stop();
            Console.WriteLine($"  Средний ключ: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            sw.Restart(); found = stringDict.ContainsKey(lastStringKey); sw.Stop();
            Console.WriteLine($"  Последний ключ: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            sw.Restart(); found = stringDict.ContainsKey(nonExistentStringKey); sw.Stop();
            Console.WriteLine($"  Несуществующий: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");

            // --- Поиск по значению в Dictionary<Team, ResearchTeam> (ContainsValue, линейный поиск O(N)) ---
            Console.WriteLine("\nВремя поиска по значению в Dictionary<Team, ResearchTeam> (ContainsValue):");
            sw.Restart(); found = teamDict.ContainsValue(firstValue); sw.Stop();
            Console.WriteLine($"  Первое значение: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            // ... (аналогично) ...
            sw.Restart(); found = teamDict.ContainsValue(middleValue); sw.Stop();
            Console.WriteLine($"  Среднее значение: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            sw.Restart(); found = teamDict.ContainsValue(lastValue); sw.Stop();
            Console.WriteLine($"  Последнее значение: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
            sw.Restart(); found = teamDict.ContainsValue(nonExistentValue); sw.Stop();
            Console.WriteLine($"  Несуществующее: {TicksToMilliseconds(sw.ElapsedTicks),8:F3} мс, Найден: {found}");
        }
    }
}

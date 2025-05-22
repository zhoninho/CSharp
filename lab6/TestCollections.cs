using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace lab6
{
    class TestCollections
    {
        private List<Team> teamList;
        private List<string> stringList;
        private Dictionary<Team, ResearchTeam> teamDict;
        private Dictionary<string, ResearchTeam> stringDict;

        public TestCollections(int count)
        {
            teamList = new List<Team>();
            stringList = new List<string>();
            teamDict = new Dictionary<Team, ResearchTeam>();
            stringDict = new Dictionary<string, ResearchTeam>();

            for (int i = 0; i < count; i++)
            {
                ResearchTeam rt = GenerateElement(i);
                Team key = rt.TeamData;
                string stringKey = key.ToString();

                teamList.Add(key);
                stringList.Add(stringKey);
                teamDict.Add(key, rt);
                stringDict.Add(stringKey, rt);
            }
        }

        public static ResearchTeam GenerateElement(int value)
        {
            ResearchTeam rt = new ResearchTeam(
                $"Topic_{value}",
                $"Org_{value}",
                value + 1, // Уникальный номер регистрации
                TimeFrame.Year
            );
            return rt;
        }

        // Метод для преобразования тиков в миллисекунды
        private static double TicksToMilliseconds(long ticks)
        {
            return Math.Round(ticks * 1e-4, 3); 
        }

        public void MeasureSearchTime()
        {
            if (teamList.Count == 0)
            {
                Console.WriteLine("Collections are empty.");
                return;
            }

            // Элементы для поиска
            Team firstKey = teamList[0];
            Team middleKey = teamList[teamList.Count / 2];
            Team lastKey = teamList[teamList.Count - 1];
            Team nonExistentKey = new Team("NonExistent", teamList.Count + 1);

            string firstStringKey = stringList[0];
            string middleStringKey = stringList[stringList.Count / 2];
            string lastStringKey = stringList[stringList.Count - 1];
            string nonExistentStringKey = $"Organization: NonExistent, Reg.Number: {teamList.Count + 1}";

            ResearchTeam firstValue = teamDict[firstKey];
            ResearchTeam middleValue = teamDict[middleKey];
            ResearchTeam lastValue = teamDict[lastKey];
            ResearchTeam nonExistentValue = GenerateElement(teamList.Count);

            Stopwatch sw = new Stopwatch();

            // Поиск в List<Team>
            Console.WriteLine("Search time in List<Team> (Contains):");
            sw.Restart();
            bool found = teamList.Contains(firstKey);
            sw.Stop();
            Console.WriteLine($"First element: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = teamList.Contains(middleKey);
            sw.Stop();
            Console.WriteLine($"Middle element: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = teamList.Contains(lastKey);
            sw.Stop();
            Console.WriteLine($"Last element: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = teamList.Contains(nonExistentKey);
            sw.Stop();
            Console.WriteLine($"Non-existent element: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");
            Console.WriteLine();

            // Поиск в List<string>
            Console.WriteLine("Search time in List<string> (Contains):");
            sw.Restart();
            found = stringList.Contains(firstStringKey);
            sw.Stop();
            Console.WriteLine($"First element: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = stringList.Contains(middleStringKey);
            sw.Stop();
            Console.WriteLine($"Middle element: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = stringList.Contains(lastStringKey);
            sw.Stop();
            Console.WriteLine($"Last element: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = stringList.Contains(nonExistentStringKey);
            sw.Stop();
            Console.WriteLine($"Non-existent element: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");
            Console.WriteLine();

            // Поиск по ключу в Dictionary<Team, ResearchTeam>
            Console.WriteLine("Search time in Dictionary<Team, ResearchTeam> (ContainsKey):");
            sw.Restart();
            found = teamDict.ContainsKey(firstKey);
            sw.Stop();
            Console.WriteLine($"First key: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = teamDict.ContainsKey(middleKey);
            sw.Stop();
            Console.WriteLine($"Middle key: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = teamDict.ContainsKey(lastKey);
            sw.Stop();
            Console.WriteLine($"Last key: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = teamDict.ContainsKey(nonExistentKey);
            sw.Stop();
            Console.WriteLine($"Non-existent key: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");
            Console.WriteLine();

            // Поиск по ключу в Dictionary<string, ResearchTeam>
            Console.WriteLine("Search time in Dictionary<string, ResearchTeam> (ContainsKey):");
            sw.Restart();
            found = stringDict.ContainsKey(firstStringKey);
            sw.Stop();
            Console.WriteLine($"First key: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = stringDict.ContainsKey(middleStringKey);
            sw.Stop();
            Console.WriteLine($"Middle key: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = stringDict.ContainsKey(lastStringKey);
            sw.Stop();
            Console.WriteLine($"Last key: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = stringDict.ContainsKey(nonExistentStringKey);
            sw.Stop();
            Console.WriteLine($"Non-existent key: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");
            Console.WriteLine();

            // Поиск по значению в Dictionary<Team, ResearchTeam>
            Console.WriteLine("Search time in Dictionary<Team, ResearchTeam> (ContainsValue):");
            sw.Restart();
            found = teamDict.ContainsValue(firstValue);
            sw.Stop();
            Console.WriteLine($"First value: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = teamDict.ContainsValue(middleValue);
            sw.Stop();
            Console.WriteLine($"Middle value: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = teamDict.ContainsValue(lastValue);
            sw.Stop();
            Console.WriteLine($"Last value: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");

            sw.Restart();
            found = teamDict.ContainsValue(nonExistentValue);
            sw.Stop();
            Console.WriteLine($"Non-existent value: {TicksToMilliseconds(sw.ElapsedTicks)} ms, Found: {found}");
        }
    }
}

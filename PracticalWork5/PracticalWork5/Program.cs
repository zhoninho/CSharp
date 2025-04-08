using System;
using System.Collections;

namespace PracticalWork5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== Задания на практическую работу №5 =====");

            // 1. Создать два объекта типа Team с совпадающими данными и проверить равенство
            Console.WriteLine("\n1. Проверка равенства объектов Team:");
            Team team1 = new Team("Research Institute", 12345);
            Team team2 = new Team("Research Institute", 12345);

            Console.WriteLine($"Ссылки равны: {ReferenceEquals(team1, team2)}");
            Console.WriteLine($"Объекты равны: {team1 == team2}");
            Console.WriteLine($"Хэш-код team1: {team1.GetHashCode()}");
            Console.WriteLine($"Хэш-код team2: {team2.GetHashCode()}");

            // 2. Попытка присвоить некорректное значение
            Console.WriteLine("\n2. Обработка исключения при некорректном значении:");
            try
            {
                team1.RegistrationNumber = -100;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение: {ex.Message}");
            }

            // 3. Создать объект ResearchTeam и добавить публикации и участников
            Console.WriteLine("\n3. Создание и наполнение объекта ResearchTeam:");
            ResearchTeam researchTeam = new ResearchTeam("AI Development", "Tech University", 54321, TimeFrame.TwoYears);

            // Создание авторов и публикаций
            Person author1 = new Person("John", "Smith", new DateTime(1980, 5, 15));
            Person author2 = new Person("Jane", "Doe", new DateTime(1985, 9, 23));
            Person author3 = new Person("Bob", "Johnson", new DateTime(1990, 3, 10));

            Paper paper1 = new Paper("AI Introduction", author1, new DateTime(2022, 3, 10));
            Paper paper2 = new Paper("Neural Networks", author2, new DateTime(2023, 8, 20));
            Paper paper3 = new Paper("Future of AI", author1, new DateTime(2024, 2, 5));

            // Добавление публикаций и участников
            researchTeam.AddPapers(paper1, paper2, paper3);
            researchTeam.AddMembers(author1, author2, author3);

            Console.WriteLine(researchTeam.ToString());

            // 4. Вывод значения свойства Team
            Console.WriteLine("\n4. Значение свойства Team:");
            Console.WriteLine(researchTeam.Team);

            // 5. Создание копии и изменение исходного объекта
            Console.WriteLine("\n5. Проверка глубокого копирования:");
            ResearchTeam researchTeamCopy = (ResearchTeam)researchTeam.DeepCopy();

            // Изменение данных оригинала
            researchTeam.Topic = "Modified Topic";
            researchTeam.Organization = "Modified Organization";

            // Добавление новой публикации в оригинал
            Paper newPaper = new Paper("New Paper", author3, DateTime.Now);
            researchTeam.AddPapers(newPaper);

            Console.WriteLine("Исходный объект после изменений:");
            Console.WriteLine(researchTeam.ToShortString());
            Console.WriteLine("\nКопия объекта (должна остаться без изменений):");
            Console.WriteLine(researchTeamCopy.ToShortString());

            // 6. Вывод списка участников без публикаций
            Console.WriteLine("\n6. Участники проекта без публикаций:");
            foreach (Person member in researchTeam.MembersWithoutPublications())
            {
                Console.WriteLine($"  - {member}");
            }

            // 7. Вывод публикаций за последние два года
            Console.WriteLine("\n7. Публикации за последние два года:");
            foreach (Paper paper in researchTeam.RecentPublications(2))
            {
                Console.WriteLine($"  - {paper}");
            }

            // 8. Вывод участников с публикациями через IEnumerable
            Console.WriteLine("\n8. Участники проекта с публикациями:");
            foreach (Person member in researchTeam)
            {
                Console.WriteLine($"  - {member}");
            }

            // 9. Вывод участников с более чем одной публикацией
            Console.WriteLine("\n9. Участники с более чем одной публикацией:");
            foreach (Person member in researchTeam.MembersWithMultiplePublications())
            {
                Console.WriteLine($"  - {member}");
            }

            // 10. Вывод публикаций за последний год
            Console.WriteLine("\n10. Публикации за последний год:");
            foreach (Paper paper in researchTeam.LastYearPublications())
            {
                Console.WriteLine($"  - {paper}");
            }
        }
    }
}
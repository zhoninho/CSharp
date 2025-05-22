using System;
using System.Collections;

namespace PracticalWork6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" Задания на практическую работу №5 ");

            Console.WriteLine("\n1. Проверка равенства объектов Team:");
            Team team1 = new Team("Research Institute", 12345);
            Team team2 = new Team("Research Institute", 12345);

            Console.WriteLine($"Ссылки равны: {ReferenceEquals(team1, team2)}");
            Console.WriteLine($"Объекты равны: {team1 == team2}");
            Console.WriteLine($"Хэш-код team1: {team1.GetHashCode()}");
            Console.WriteLine($"Хэш-код team2: {team2.GetHashCode()}");

            Console.WriteLine("\n2. Обработка исключения при некорректном значении:");
            try
            {
                team1.RegistrationNumber = -100;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение: {ex.Message}");
            }

            Console.WriteLine("\n3. Создание и наполнение объекта ResearchTeam:");
            ResearchTeam researchTeam = new ResearchTeam("AI Development", "Tech University", 54321, TimeFrame.TwoYears);

            Person author1 = new Person("John", "Smith", new DateTime(1980, 5, 15));
            Person author2 = new Person("Jane", "Doe", new DateTime(1985, 9, 23));
            Person author3 = new Person("Bob", "Johnson", new DateTime(1990, 3, 10));

            Paper paper1 = new Paper("AI Introduction", author1, new DateTime(2022, 3, 10));
            Paper paper2 = new Paper("Neural Networks", author2, new DateTime(2023, 8, 20));
            Paper paper3 = new Paper("Future of AI", author1, new DateTime(2024, 2, 5));

            researchTeam.AddPapers(paper1, paper2, paper3);
            researchTeam.AddMembers(author1, author2, author3);

            Console.WriteLine(researchTeam.ToString());

            Console.WriteLine("\n4. Значение свойства Team:");
            Console.WriteLine(researchTeam.Team);

            Console.WriteLine("\n5. Проверка глубокого копирования:");
            ResearchTeam researchTeamCopy = (ResearchTeam)researchTeam.DeepCopy();

            researchTeam.Topic = "Modified Topic";
            researchTeam.Organization = "Modified Organization";

            Paper newPaper = new Paper("New Paper", author3, DateTime.Now);
            researchTeam.AddPapers(newPaper);

            Console.WriteLine("Исходный объект после изменений:");
            Console.WriteLine(researchTeam.ToShortString());
            Console.WriteLine("\nКопия объекта (должна остаться без изменений):");
            Console.WriteLine(researchTeamCopy.ToShortString());

            Console.WriteLine("\n6. Участники проекта без публикаций:");
            foreach (Person member in researchTeam.MembersWithoutPublications())
            {
                Console.WriteLine($"  - {member}");
            }

            Console.WriteLine("\n7. Публикации за последние два года:");
            foreach (Paper paper in researchTeam.RecentPublications(2))
            {
                Console.WriteLine($"  - {paper}");
            }

            Console.WriteLine("\n8. Участники проекта с публикациями:");
            foreach (Person member in researchTeam)
            {
                Console.WriteLine($"  - {member}");
            }

            Console.WriteLine("\n9. Участники с более чем одной публикацией:");
            foreach (Person member in researchTeam.MembersWithMultiplePublications())
            {
                Console.WriteLine($"  - {member}");
            }

            Console.WriteLine("\n10. Публикации за последний год:");
            foreach (Paper paper in researchTeam.LastYearPublications())
            {
                Console.WriteLine($"  - {paper}");
            }
        }
    }
}

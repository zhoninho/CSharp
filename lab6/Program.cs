using lab6;

enum TimeFrame { Year, TwoYears, Long }


class Program
{
    static int GetValidInput()
    {
        int count;
        while (true)
        {
            Console.Write("Enter the number of elements for collections: ");
            try
            {
                count = int.Parse(Console.ReadLine());
                if (count >= 0)
                    return count;
                Console.WriteLine("Error: Number must be non-negative.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Please enter a valid integer.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void Main()
    {
        // 1. Создание и вывод ResearchTeamCollection
        ResearchTeamCollection collection = new ResearchTeamCollection();
        collection.AddDefaults();

        Person author1 = new Person("Alice", "Smith", new DateTime(1985, 5, 15));
        Person author2 = new Person("Bob", "Johnson", new DateTime(1978, 8, 22));
        Person member1 = new Person("Carol", "Williams", new DateTime(1990, 3, 10));

        ResearchTeam rt1 = new ResearchTeam("Quantum Computing", "Quantum Lab", 101, TimeFrame.TwoYears);
        rt1.AddMembers(author1, member1);
        rt1.AddPapers(
            new Paper("Quantum Paper 1", author1, new DateTime(2023, 1, 15)),
            new Paper("Quantum Paper 2", author1, new DateTime(2024, 6, 10))
        );

        ResearchTeam rt2 = new ResearchTeam("Bioinformatics", "Bio Center", 202, TimeFrame.Long);
        rt2.AddMembers(author2);
        rt2.AddPapers(new Paper("Bio Paper 1", author2, new DateTime(2024, 3, 20)));

        collection.AddResearchTeams(rt1, rt2);

        Console.WriteLine("1. ResearchTeamCollection:");
        Console.WriteLine(collection.ToString());
        Console.WriteLine();

        // 2. Сортировка
        Console.WriteLine("2. Sorting ResearchTeamCollection:");
        Console.WriteLine("Sorted by Registration Number:");
        collection.SortByRegistrationNumber();
        Console.WriteLine(collection.ToShortString());
        Console.WriteLine();

        Console.WriteLine("Sorted by Research Topic:");
        collection.SortByResearchTopic();
        Console.WriteLine(collection.ToShortString());
        Console.WriteLine();

        Console.WriteLine("Sorted by Publications Count:");
        collection.SortByPublicationsCount();
        Console.WriteLine(collection.ToShortString());
        Console.WriteLine();

        // 3. Операции с коллекцией
        Console.WriteLine("3. Operations with ResearchTeamCollection:");
        Console.WriteLine($"Minimum Registration Number: {collection.MinRegistrationNumber}");
        Console.WriteLine();

        Console.WriteLine("Projects with TwoYears duration:");
        foreach (ResearchTeam team in collection.TwoYearsProjects)
            Console.WriteLine(team.ToShortString());
        Console.WriteLine();

        Console.WriteLine("Grouping by number of members:");
        for (int i = 0; i <= 3; i++)
        {
            List<ResearchTeam> group = collection.NGroup(i);
            if (group.Count > 0)
            {
                Console.WriteLine($"Teams with {i} members:");
                foreach (ResearchTeam team in group)
                    Console.WriteLine(team.ToShortString());
            }
        }
        Console.WriteLine();

        // 4. TestCollections и измерение времени поиска
        Console.WriteLine("4. TestCollections Search Time:");
        int count = GetValidInput();
        TestCollections testCollections = new TestCollections(count);
        testCollections.MeasureSearchTime();
    }
}

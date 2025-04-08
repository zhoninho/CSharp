using System;
using System.Diagnostics;
using static PracticalWork3.ResearchTeam;

namespace PracticalWork3
{
    class Program
    {
        static void Main()
        {
            // 1. Создать объект типа ResearchTeam и вывести данные
            Console.WriteLine("1. Объект типа ResearchTeam:");
            ResearchTeam team = new ResearchTeam("Исследование алгоритмов машинного обучения",
                "МГУ", 12345, TimeFrame.TwoYears);
            Console.WriteLine(team.ToShortString());
            Console.WriteLine();

            // 2. Вывести значения индексатора для TimeFrame.Year, TimeFrame.TwoYears, TimeFrame.Long
            Console.WriteLine("2. Значения индексатора:");
            Console.WriteLine($"TimeFrame.Year: {team[TimeFrame.Year]}");
            Console.WriteLine($"TimeFrame.TwoYears: {team[TimeFrame.TwoYears]}");
            Console.WriteLine($"TimeFrame.Long: {team[TimeFrame.Long]}");
            Console.WriteLine();

            // 3. Присвоить значения всем свойствам и вывести данные
            Console.WriteLine("3. Присвоение значений свойствам:");
            team.Topic = "Нейронные сети и глубокое обучение";
            team.Organization = "Институт информационных технологий";
            team.RegistrationNumber = 54321;
            team.TimeFrame = TimeFrame.Long;
            Console.WriteLine(team.ToString());
            Console.WriteLine();

            // 4. Добавить элементы в список публикаций и вывести данные
            Console.WriteLine("4. Добавление публикаций:");
            team.AddPapers(
                new Paper("Основы нейронных сетей",
                    new Person("Алексей", "Петров", new DateTime(1985, 5, 15)),
                    new DateTime(2023, 3, 10)),
                new Paper("Глубокое обучение на практике",
                    new Person("Елена", "Смирнова", new DateTime(1990, 8, 25)),
                    new DateTime(2024, 1, 5))
            );
            Console.WriteLine(team.ToString());
            Console.WriteLine();

            // 5. Вывести значение свойства LatestPublication
            Console.WriteLine("5. Самая поздняя публикация:");
            Console.WriteLine(team.LatestPublication?.ToString() ?? "Нет публикаций");
            Console.WriteLine();


            // Сравнение производительности массивов
            CompareArrayPerformance();
        }

        static void CompareArrayPerformance()
        {
            Console.WriteLine("Введите количество строк и столбцов в формате 'nrow;ncolumn' (разделители: ';', ':', ','):");
            string input = Console.ReadLine();

            // Разбиваем введенную строку на параметры
            string[] parameters = input.Split(new char[] { ';', ':', ',' });

            if (parameters.Length < 2)
            {
                Console.WriteLine("Недостаточно параметров. Используются значения по умолчанию: 5 строк, 5 столбцов.");
                parameters = new string[] { "5", "5" };
            }

            int nrow = int.Parse(parameters[0]);
            int ncolumn = int.Parse(parameters[1]);
            int totalElements = nrow * ncolumn;

            Console.WriteLine($"Количество строк: {nrow}, количество столбцов: {ncolumn}, всего элементов: {totalElements}");

            // Создание массивов
            Person[] oneDimArray = new Person[totalElements];
            Person[,] twoDimArray = new Person[nrow, ncolumn];
            Person[][] jaggedArray = new Person[nrow][];

            // Инициализация массивов
            for (int i = 0; i < totalElements; i++)
            {
                oneDimArray[i] = new Person();
            }

            for (int i = 0; i < nrow; i++)
            {
                for (int j = 0; j < ncolumn; j++)
                {
                    twoDimArray[i, j] = new Person();
                }
            }

            for (int i = 0; i < nrow; i++)
            {
                jaggedArray[i] = new Person[ncolumn];
                for (int j = 0; j < ncolumn; j++)
                {
                    jaggedArray[i][j] = new Person();
                }
            }

            // Измерение времени выполнения операций для одномерного массива
            int startTime = Environment.TickCount;
            for (int i = 0; i < totalElements; i++)
            {
                oneDimArray[i].BirthYear = 1990;
            }
            int oneDimTime = Environment.TickCount - startTime;

            // Измерение времени выполнения операций для двумерного прямоугольного массива
            startTime = Environment.TickCount;
            for (int i = 0; i < nrow; i++)
            {
                for (int j = 0; j < ncolumn; j++)
                {
                    twoDimArray[i, j].BirthYear = 1990;
                }
            }
            int twoDimTime = Environment.TickCount - startTime;

            // Измерение времени выполнения операций для двумерного ступенчатого массива
            startTime = Environment.TickCount;
            for (int i = 0; i < nrow; i++)
            {
                for (int j = 0; j < ncolumn; j++)
                {
                    jaggedArray[i][j].BirthYear = 1990;
                }
            }
            int jaggedTime = Environment.TickCount - startTime;

            // Вывод результатов
            Console.WriteLine($"\nРезультаты измерения времени выполнения операций:");
            Console.WriteLine($"Количество строк: {nrow}, количество столбцов: {ncolumn}, всего элементов: {totalElements}");
            Console.WriteLine($"Время для одномерного массива: {oneDimTime} мс");
            Console.WriteLine($"Время для двумерного прямоугольного массива: {twoDimTime} мс");
            Console.WriteLine($"Время для двумерного ступенчатого массива: {jaggedTime} мс");

        }
    }

}

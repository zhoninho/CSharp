using PracticalWork7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7
{
    // Класс коллекции исследовательских команд
    class ResearchTeamCollection
    {
        // Список для хранения команд
        private List<ResearchTeam> teams;

        // Открытое автореализуемое свойство типа string с названием коллекции
        public string CollectionName { get; set; }

        // Событие, которое происходит при добавлении элемента в конец списка List<ResearchTeam>
        public event TeamListHandler ResearchTeamAdded;

        // Событие, которое происходит, когда новый элемент вставляется перед одним из элементов списка List<ResearchTeam>
        public event TeamListHandler ResearchTeamInserted;

        // Конструктор по умолчанию
        public ResearchTeamCollection(string collectionName = "Коллекция по умолчанию")
        {
            teams = new List<ResearchTeam>();
            CollectionName = collectionName;
        }

        // Защищенный виртуальный метод для вызова события ResearchTeamAdded
        // Позволяет классам-наследникам изменять поведение при вызове события
        protected virtual void OnResearchTeamAdded(string changeInfo, int elementIndex)
        {
            // Проверяем, есть ли подписчики на событие
            ResearchTeamAdded?.Invoke(this, new TeamListHandlerEventArgs(CollectionName, changeInfo, elementIndex));
        }

        // Защищенный виртуальный метод для вызова события ResearchTeamInserted
        protected virtual void OnResearchTeamInserted(string changeInfo, int elementIndex)
        {
            // Проверяем, есть ли подписчики на событие
            ResearchTeamInserted?.Invoke(this, new TeamListHandlerEventArgs(CollectionName, changeInfo, elementIndex));
        }

        // Метод добавления команд по умолчанию
        public void AddDefaults()
        {
            // Команды для добавления
            ResearchTeam team1 = new ResearchTeam("Исследование ИИ", "Тех. Университет", 123, TimeFrame.TwoYears);
            ResearchTeam team2 = new ResearchTeam("Разработка МО", "Научный Институт", 456, TimeFrame.Year);
            ResearchTeam team3 = new ResearchTeam("Наука о данных", "Исследовательский Центр", 789, TimeFrame.Long);

            teams.Add(team1);
            // Вызов события для первой добавленной команды
            OnResearchTeamAdded("Добавлен элемент (AddDefaults)", teams.Count - 1);

            teams.Add(team2);
            // Вызов события для второй добавленной команды
            OnResearchTeamAdded("Добавлен элемент (AddDefaults)", teams.Count - 1);

            teams.Add(team3);
            // Вызов события для третьей добавленной команды
            OnResearchTeamAdded("Добавлен элемент (AddDefaults)", teams.Count - 1);
        }

        // Метод добавления массива команд
        public void AddResearchTeams(params ResearchTeam[] newTeams)
        {
            foreach (var team in newTeams)
            {
                if (team != null) // Добавляем проверку на null для большей надежности
                {
                    teams.Add(team);
                    // Вызов события для каждого добавленного элемента
                    OnResearchTeamAdded("Добавлен элемент (AddResearchTeams)", teams.Count - 1);
                }
            }
        }

        // Метод void InsertAt (int j, ResearchTeam rt), который вставляет элемент rt в список List<ResearchTeam>
        // перед элементом с номером j; если в списке нет элемента с номером j, метод добавляет элемент в конец списка;
        public void InsertAt(int j, ResearchTeam rt)
        {
            if (rt == null) return; // Не вставляем null элементы

            // Проверяем, есть ли элемент с индексом j (для вставки ПЕРЕД ним)
            // Элемент с номером j есть в списке, если 0 <= j < teams.Count
            if (j >= 0 && j < teams.Count)
            {
                // Элемент с номером j существует, вставляем rt перед ним
                teams.Insert(j, rt);
                // Событие ResearchTeamInserted: элемент rt вставлен на позицию j
                OnResearchTeamInserted($"Элемент вставлен на индекс {j}", j);
            }
            else // Элемента с номером j нет в списке (j < 0 или j >= teams.Count)
            {
                // Добавляем элемент rt в конец списка
                teams.Add(rt);
                string reason;
                if (j == teams.Count) reason = $"Индекс {j} указывал на позицию вставки в конец списка";
                else if (j < 0) reason = $"Индекс {j} был отрицательным";
                else reason = $"Индекс {j} был больше или равен количеству элементов ({teams.Count - 1} после добавления)"; // -1 т.к. teams.Count уже увеличился

                // Событие ResearchTeamAdded: элемент добавлен в конец
                OnResearchTeamAdded($"Элемент добавлен в конец (причина: {reason}, попытка вставки на индекс {j})", teams.Count - 1);
            }
        }

        // Индексатор типа ResearchTeam (с методами get и set) с целочисленным индексом
        // для доступа к элементу списка List<ResearchTeam> с заданным номером.
        public ResearchTeam this[int index]
        {
            get
            {
                // Проверка корректности индекса для чтения
                if (index < 0 || index >= teams.Count)
                {
                    throw new IndexOutOfRangeException($"Индекс {index} выходит за пределы коллекции (размер: {teams.Count}).");
                }
                return teams[index];
            }
            set
            {
                // Проверка корректности индекса для записи
                if (index < 0 || index >= teams.Count)
                {
                    throw new IndexOutOfRangeException($"Индекс {index} для присвоения выходит за пределы коллекции (размер: {teams.Count}).");
                }
                // Присваиваем новое значение элементу по указанному индексу
                teams[index] = value;
                // ВАЖНО: Согласно заданию, на данный момент события ResearchTeamAdded и ResearchTeamInserted
                // НЕ должны бросаться при использовании сеттера индексатора.
                // Если бы требовалось событие для "замены элемента", оно было бы здесь.
                // Например: OnResearchTeamReplaced("Элемент заменен на индексе", index); (но такого события нет в задании)
            }
        }


        // Переопределение метода ToString для вывода информации о коллекции
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Название коллекции: {CollectionName}");
            sb.AppendLine("Команды в коллекции ResearchTeamCollection:");
            if (teams.Count == 0)
            {
                sb.AppendLine("  Коллекция пуста.");
            }
            else
            {
                for (int i = 0; i < teams.Count; i++)
                {
                    sb.AppendLine($"  [{i}] {teams[i].ToString()}");
                }
            }
            return sb.ToString();
        }

        // Переопределение метода ToShortString для краткого вывода информации о коллекции
        public virtual string ToShortString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Название коллекции: {CollectionName} (Кратко)");
            sb.AppendLine("Команды в коллекции ResearchTeamCollection:");
            if (teams.Count == 0)
            {
                sb.AppendLine("  Коллекция пуста.");
            }
            else
            {
                for (int i = 0; i < teams.Count; i++)
                {
                    sb.AppendLine($"  [{i}] {teams[i].ToShortString()}");
                }
            }
            return sb.ToString();
        }

        // Метод сортировки по регистрационному номеру
        public void SortByRegistrationNumber()
        {
            // Team реализует IComparable<Team> по RegistrationNumber
            teams.Sort();
        }

        // Метод сортировки по теме исследования
        public void SortByResearchTopic()
        {
            // ResearchTeam реализует IComparer<ResearchTeam> по ResearchTopic
            teams.Sort(new ResearchTeam());
        }

        // Метод сортировки по количеству публикаций
        public void SortByPublicationsCount()
        {
            teams.Sort(new PublicationsComparer());
        }

        // Свойство для получения минимального регистрационного номера
        public int MinRegistrationNumber
        {
            get
            {
                if (teams.Count == 0) return 0;
                return teams.Min(team => team.RegistrationNumber);
            }
        }

        // Свойство для получения команд с проектами на два года
        public IEnumerable<ResearchTeam> TwoYearsProjects
        {
            get
            {
                return teams.Where(team => team.Duration == TimeFrame.TwoYears);
            }
        }

        // Метод для группировки команд по количеству участников
        public List<ResearchTeam> NGroup(int value)
        {
            return teams.Where(team => team.Members.Count == value).ToList();
        }

        // Метод для получения доступа к внутреннему списку (может быть полезен для тестов или отладки)
        public List<ResearchTeam> GetTeamsList()
        {
            return teams;
        }
    }
}

// ResearchTeam.cs
using PracticalWork7;
using System;
using System.Collections;        // Для IEnumerable
using System.Collections.Generic; // Для List<T>, IComparer<T>
using System.Linq;              // Для LINQ методов (например, OrderBy, Where, Count, Any), если используются
using System.Text;              // Для StringBuilder (хотя здесь не используется)
using System.Threading.Tasks;   // Не используется, можно убрать

namespace PracticalWork7
{
    // Класс ResearchTeam (Исследовательская команда).
    // Наследуется от Team и реализует интерфейсы INameAndCopy, IEnumerable, IComparer<ResearchTeam>.
    class ResearchTeam : Team, INameAndCopy, IEnumerable, IComparer<ResearchTeam>
    {
        // Приватное поле для хранения темы исследования.
        private string researchTopic;
        // Приватное поле для хранения продолжительности исследования (тип TimeFrame - перечисление).
        private TimeFrame duration;
        // Приватное поле для списка участников команды (объекты типа Person).
        private List<Person> members;
        // Приватное поле для списка публикаций команды (объекты типа Paper).
        private List<Paper> publications;

        // Конструктор по умолчанию.
        // Вызывает конструктор базового класса Team() и инициализирует поля значениями по умолчанию.
        public ResearchTeam() : base()
        {
            researchTopic = "Неопределенная тема"; // Тема по умолчанию
            duration = TimeFrame.Year;             // Продолжительность по умолчанию
            members = new List<Person>();          // Инициализация пустого списка участников
            publications = new List<Paper>();      // Инициализация пустого списка публикаций
        }

        // Конструктор с параметрами.
        // Вызывает конструктор базового класса Team(org, regNumber) и инициализирует остальные поля.
        public ResearchTeam(string topic, string org, int regNumber, TimeFrame duration)
            : base(org, regNumber) // Вызов конструктора базового класса
        {
            researchTopic = topic;
            this.duration = duration;
            members = new List<Person>();
            publications = new List<Paper>();
        }

        // Свойство для доступа к теме исследования.
        public string ResearchTopic
        {
            get { return researchTopic; }
            set { researchTopic = value; }
        }

        // Свойство для доступа к продолжительности исследования.
        public TimeFrame Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        // Свойство для доступа к списку участников.
        // Позволяет получить или полностью заменить список участников.
        public List<Person> Members
        {
            get { return members; }
            set { members = value ?? new List<Person>(); } // Защита от присвоения null
        }

        // Свойство для доступа к списку публикаций.
        // Позволяет получить или полностью заменить список публикаций.
        public List<Paper> Publications
        {
            get { return publications; }
            set { publications = value ?? new List<Paper>(); } // Защита от присвоения null
        }

        // Свойство для получения последней (самой новой по дате) публикации.
        // Если публикаций нет, возвращает null.
        public Paper LatestPublication
        {
            get
            {
                if (publications == null || publications.Count == 0) return null;
                // Используем LINQ для поиска публикации с максимальной датой.
                return publications.OrderByDescending(p => p.PublicationDate).FirstOrDefault();
            }
        }

        // Свойство TeamData.
        // Позволяет получить или установить данные базового класса (Организация и Рег.Номер)
        // через объект типа Team.
        public Team TeamData
        {
            get { return new Team(organization, registrationNumber); }
            set
            {
                if (value != null)
                {
                    organization = value.Organization;
                    RegistrationNumber = value.RegistrationNumber; // Используем свойство для валидации
                }
            }
        }

        // Явная реализация свойства Name из интерфейса INameAndCopy.
        // Для ResearchTeam, Name - это тема исследования.
        string INameAndCopy.Name
        {
            get { return researchTopic; }
            set { researchTopic = value; }
        }

        // Явная реализация метода DeepCopy из интерфейса INameAndCopy.
        // Вызывает публичный виртуальный метод DeepCopy.
        object INameAndCopy.DeepCopy()
        {
            return DeepCopy();
        }

        // Публичный переопределенный метод DeepCopy.
        // Создает глубокую копию объекта ResearchTeam.
        public override object DeepCopy()
        {
            // Создаем новый объект ResearchTeam с базовыми данными.
            ResearchTeam copy = new ResearchTeam(researchTopic, organization, registrationNumber, duration);

            // Глубокое копирование списка участников.
            if (members != null)
            {
                copy.Members = new List<Person>();
                foreach (Person member in members)
                {
                    copy.Members.Add((Person)member.DeepCopy()); // Каждый участник копируется глубоко.
                }
            }

            // Глубокое копирование списка публикаций.
            if (publications != null)
            {
                copy.Publications = new List<Paper>();
                foreach (Paper paper in publications)
                {
                    copy.Publications.Add((Paper)paper.DeepCopy()); // Каждая публикация копируется глубоко.
                }
            }
            return copy;
        }

        // Метод для добавления одного или нескольких участников.
        public void AddMembers(params Person[] newMembers)
        {
            if (newMembers != null)
            {
                if (members == null) members = new List<Person>();
                members.AddRange(newMembers.Where(m => m != null)); // Добавляем только не-null участников
            }
        }

        // Метод для добавления одной или нескольких публикаций.
        public void AddPapers(params Paper[] newPapers)
        {
            if (newPapers != null)
            {
                if (publications == null) publications = new List<Paper>();
                publications.AddRange(newPapers.Where(p => p != null)); // Добавляем только не-null публикации
            }
        }

        // Переопределение метода ToString() для полного строкового представления объекта.
        public override string ToString()
        {
            // Формируем строку с информацией о команде, ее участниках и публикациях.
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Тема: {researchTopic}");
            sb.AppendLine(base.ToString()); // Информация из базового класса Team
            sb.AppendLine($"Длительность: {duration}");

            sb.AppendLine("Участники:");
            if (members != null && members.Count > 0)
                foreach (Person member in members) sb.AppendLine($"  - {member.ToString()}");
            else sb.AppendLine("  Нет участников.");

            sb.AppendLine("Публикации:");
            if (publications != null && publications.Count > 0)
                foreach (Paper paper in publications) sb.AppendLine($"  - {paper.ToString()}");
            else sb.AppendLine("  Нет публикаций.");

            return sb.ToString();
        }

        // Метод ToShortString() для краткого строкового представления объекта.
        public string ToShortString()
        {
            int memberCount = (members != null) ? members.Count : 0;
            int publicationCount = (publications != null) ? publications.Count : 0;
            return $"Тема: {researchTopic}, {base.ToString()}, Длительность: {duration}, Участников: {memberCount}, Публикаций: {publicationCount}";
        }

        // Итератор (реализация IEnumerable) для получения участников, у которых НЕТ публикаций в данной команде.
        public IEnumerable GetMembersWithoutPublications()
        {
            if (members == null) yield break; // Если нет участников, выходим

            foreach (Person member in members)
            {
                // Проверяем, есть ли у участника публикации в списке publications.
                // Используем LINQ Any() для проверки.
                bool hasPublication = publications != null && publications.Any(paper => paper.Author.Equals(member));
                if (!hasPublication)
                {
                    yield return member; // Возвращаем участника, если у него нет публикаций.
                }
            }
        }

        // Итератор для получения публикаций за последние N лет.
        public IEnumerable GetPublicationsLastYears(int years)
        {
            if (publications == null) yield break;

            DateTime thresholdDate = DateTime.Now.AddYears(-years); // Дата N лет назад.
            foreach (Paper paper in publications)
            {
                if (paper.PublicationDate >= thresholdDate)
                {
                    yield return paper; // Возвращаем публикацию, если она не старше N лет.
                }
            }
        }

        // Реализация метода GetEnumerator() из интерфейса IEnumerable.
        // Позволяет использовать foreach для итерации по участникам команды, ИМЕЮЩИМ публикации.
        public IEnumerator GetEnumerator()
        {
            // Используем кастомный нумератор ResearchTeamEnumerator.
            return new ResearchTeamEnumerator(this);
        }

        // Итератор для получения участников с БОЛЕЕ ЧЕМ ОДНОЙ публикацией.
        public IEnumerable GetMembersWithMultiplePublications()
        {
            if (members == null || publications == null) yield break;

            foreach (Person member in members)
            {
                // Считаем количество публикаций для каждого участника с помощью LINQ Count().
                int publicationCount = publications.Count(paper => paper.Author.Equals(member));
                if (publicationCount > 1)
                {
                    yield return member; // Возвращаем участника, если у него >1 публикации.
                }
            }
        }

        // Итератор для получения публикаций за ПОСЛЕДНИЙ ГОД.
        public IEnumerable GetPublicationsLastYear()
        {
            // Вызываем GetPublicationsLastYears с параметром 1.
            return GetPublicationsLastYears(1);
        }

        // Реализация метода Compare из интерфейса IComparer<ResearchTeam>.
        // Для сравнения объектов ResearchTeam по теме исследования (ResearchTopic).
        // Используется, например, при вызове List<ResearchTeam>.Sort(new ResearchTeam()).
        public int Compare(ResearchTeam x, ResearchTeam y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1; // null считается "меньше"
            if (y == null) return 1;  // не-null считается "больше"

            // Сравнение строк (тем исследований) без учета регистра.
            return string.Compare(x.ResearchTopic, y.ResearchTopic, StringComparison.OrdinalIgnoreCase);
        }
    }
}
// ResearchTeam.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel; // Для INotifyPropertyChanged
using System.Runtime.CompilerServices; // Для CallerMemberName

namespace PracticalWork8
{
    public enum TimeFrame
    {
        Year,
        TwoYears,
        Long
    }
    // Класс ResearchTeam (Исследовательская команда).
    // Наследуется от Team и реализует интерфейсы INameAndCopy, IEnumerable, IComparer<ResearchTeam>, INotifyPropertyChanged.
    public class ResearchTeam : Team, INameAndCopy, IEnumerable, IComparer<ResearchTeam>, INotifyPropertyChanged
    {
        // Приватное поле для хранения темы исследования.
        private string _researchTopicField; // Переименовано для избежания конфликта с автосвойством, если бы оно было
        // Приватное поле для хранения продолжительности исследования (тип TimeFrame - перечисление).
        private TimeFrame _durationField;

        // Событие из интерфейса INotifyPropertyChanged.
        // Происходит при изменении значения свойства объекта.
        public event PropertyChangedEventHandler PropertyChanged;

        // Метод для вызова события PropertyChanged.
        // Атрибут [CallerMemberName] позволяет автоматически подставить имя вызывающего свойства.
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Конструктор по умолчанию.
        public ResearchTeam() : base()
        {
            _researchTopicField = "Неопределенная тема";
            _durationField = TimeFrame.Year;
            members = new List<Person>();
            publications = new List<Paper>();
        }

        // Конструктор с параметрами.
        public ResearchTeam(string topic, string org, int regNumber, TimeFrame duration)
            : base(org, regNumber)
        {
            _researchTopicField = topic;
            _durationField = duration;
            members = new List<Person>();
            publications = new List<Paper>();
        }

        // Свойство для доступа к теме исследования.
        // При изменении вызывает событие PropertyChanged.
        public string ResearchTopic
        {
            get { return _researchTopicField; }
            set
            {
                if (_researchTopicField != value)
                {
                    _researchTopicField = value;
                    OnPropertyChanged(); // Автоматически подставит "ResearchTopic"
                }
            }
        }

        // Свойство для доступа к продолжительности исследования.
        // При изменении вызывает событие PropertyChanged.
        public TimeFrame Duration
        {
            get { return _durationField; }
            set
            {
                if (_durationField != value)
                {
                    _durationField = value;
                    OnPropertyChanged(); // Автоматически подставит "Duration"
                }
            }
        }

        // Приватные поля для списков, которые были в предыдущей версии
        private List<Person> members;
        private List<Paper> publications;

        // Свойство для доступа к списку участников.
        public List<Person> Members
        {
            get { return members; }
            set { members = value ?? new List<Person>(); }
        }

        // Свойство для доступа к списку публикаций.
        public List<Paper> Publications
        {
            get { return publications; }
            set { publications = value ?? new List<Paper>(); }
        }

        // Свойство для получения последней публикации.
        public Paper LatestPublication
        {
            get
            {
                if (publications == null || publications.Count == 0) return null;
                return publications.OrderByDescending(p => p.PublicationDate).FirstOrDefault();
            }
        }

        // Свойство TeamData.
        public Team TeamData
        {
            get { return new Team(organization, registrationNumber); }
            set
            {
                if (value != null)
                {
                    organization = value.Organization;
                    RegistrationNumber = value.RegistrationNumber;
                }
            }
        }

        // Явная реализация свойства Name из INameAndCopy.
        string INameAndCopy.Name
        {
            get { return ResearchTopic; } // Используем свойство ResearchTopic, чтобы при его изменении сработало PropertyChanged
            set { ResearchTopic = value; } // Установка через свойство ResearchTopic также вызовет OnPropertyChanged
        }

        // Явная реализация метода DeepCopy из INameAndCopy.
        object INameAndCopy.DeepCopy()
        {
            return DeepCopy();
        }

        // Публичный переопределенный метод DeepCopy.
        public override object DeepCopy()
        {
            ResearchTeam copy = new ResearchTeam(ResearchTopic, organization, registrationNumber, Duration); // Используем свойства
            if (members != null)
            {
                copy.Members = new List<Person>();
                foreach (Person member in members)
                    copy.Members.Add((Person)member.DeepCopy());
            }
            if (publications != null)
            {
                copy.Publications = new List<Paper>();
                foreach (Paper paper in publications)
                    copy.Publications.Add((Paper)paper.DeepCopy());
            }
            return copy;
        }

        // Метод для добавления участников.
        public void AddMembers(params Person[] newMembers)
        {
            if (newMembers != null)
            {
                if (members == null) members = new List<Person>();
                members.AddRange(newMembers.Where(m => m != null));
            }
        }

        // Метод для добавления публикаций.
        public void AddPapers(params Paper[] newPapers)
        {
            if (newPapers != null)
            {
                if (publications == null) publications = new List<Paper>();
                publications.AddRange(newPapers.Where(p => p != null));
            }
        }

        // Переопределение метода ToString().
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Тема: {ResearchTopic}"); // Используем свойство
            sb.AppendLine(base.ToString());
            sb.AppendLine($"Длительность: {Duration}"); // Используем свойство
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

        // Метод ToShortString().
        public string ToShortString()
        {
            int memberCount = (members != null) ? members.Count : 0;
            int publicationCount = (publications != null) ? publications.Count : 0;
            return $"Тема: {ResearchTopic}, {base.ToString()}, Длительность: {Duration}, Участников: {memberCount}, Публикаций: {publicationCount}";
        }

        // Итератор для участников без публикаций.
        public IEnumerable GetMembersWithoutPublications()
        {
            if (members == null) yield break;
            foreach (Person member in members)
            {
                bool hasPublication = publications != null && publications.Any(paper => paper.Author.Equals(member));
                if (!hasPublication) yield return member;
            }
        }

        // Итератор для публикаций за последние N лет.
        public IEnumerable GetPublicationsLastYears(int years)
        {
            if (publications == null) yield break;
            DateTime thresholdDate = DateTime.Now.AddYears(-years);
            foreach (Paper paper in publications)
            {
                if (paper.PublicationDate >= thresholdDate) yield return paper;
            }
        }

        // Реализация GetEnumerator() для итерации по участникам с публикациями.
        public IEnumerator GetEnumerator()
        {
            return new ResearchTeamEnumerator(this);
        }

        // Итератор для участников с несколькими публикациями.
        public IEnumerable GetMembersWithMultiplePublications()
        {
            if (members == null || publications == null) yield break;
            foreach (Person member in members)
            {
                int publicationCount = publications.Count(paper => paper.Author.Equals(member));
                if (publicationCount > 1) yield return member;
            }
        }

        // Итератор для публикаций за последний год.
        public IEnumerable GetPublicationsLastYear()
        {
            return GetPublicationsLastYears(1);
        }

        // Реализация IComparer<ResearchTeam> для сравнения по ResearchTopic.
        public int Compare(ResearchTeam x, ResearchTeam y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return string.Compare(x.ResearchTopic, y.ResearchTopic, StringComparison.OrdinalIgnoreCase);
        }
    }
}
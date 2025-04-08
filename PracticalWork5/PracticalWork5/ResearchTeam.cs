using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork5
{
    enum TimeFrame
    {
        Year,
        TwoYears,
        Long
    }
    internal class ResearchTeam : Team, INameAndCopy, IEnumerable
    {
        // Закрытые поля
        private string topic;
        private TimeFrame timeFrame;
        private ArrayList members;
        private ArrayList publications;

        // Конструктор с параметрами
        public ResearchTeam(string topic, string organization, int registrationNumber, TimeFrame timeFrame)
            : base(organization, registrationNumber)
        {
            this.topic = topic;
            this.timeFrame = timeFrame;
            this.members = new ArrayList();
            this.publications = new ArrayList();
        }

        // Конструктор без параметров
        public ResearchTeam() : base()
        {
            topic = "Default Research Topic";
            timeFrame = TimeFrame.Year;
            members = new ArrayList();
            publications = new ArrayList();
        }

        // Свойство для доступа к полю с названием темы исследований
        public string Topic
        {
            get { return topic; }
            set { topic = value; }
        }

        // Свойство для доступа к полю с продолжительностью исследований
        public TimeFrame TimeFrame
        {
            get { return timeFrame; }
            set { timeFrame = value; }
        }

        // Свойство для доступа к списку публикаций
        public ArrayList Publications
        {
            get { return publications; }
            set { publications = value; }
        }

        // Свойство для доступа к списку участников проекта
        public ArrayList Members
        {
            get { return members; }
            set { members = value; }
        }

        // Реализация свойства Name из интерфейса INameAndCopy
        public new string Name
        {
            get { return topic; }
            set { topic = value; }
        }

        // Свойство типа Team
        public Team Team
        {
            get { return new Team(organization, registrationNumber); }
            set
            {
                organization = value.Organization;
                registrationNumber = value.RegistrationNumber;
            }
        }

        // Свойство возвращающее публикацию с самой поздней датой
        public Paper LatestPublication
        {
            get
            {
                if (publications == null || publications.Count == 0)
                {
                    return null;
                }

                Paper latest = (Paper)publications[0];
                for (int i = 1; i < publications.Count; i++)
                {
                    Paper current = (Paper)publications[i];
                    if (current.PublicationDate > latest.PublicationDate)
                    {
                        latest = current;
                    }
                }
                return latest;
            }
        }

        // Индексатор
        public bool this[TimeFrame frame]
        {
            get { return timeFrame == frame; }
        }

        // Метод добавления публикаций
        public void AddPapers(params Paper[] papers)
        {
            if (papers == null)
                return;

            foreach (Paper paper in papers)
            {
                if (paper != null)
                    publications.Add(paper);
            }
        }

        // Метод добавления участников проекта
        public void AddMembers(params Person[] people)
        {
            if (people == null)
                return;

            foreach (Person person in people)
            {
                if (person != null)
                    members.Add(person);
            }
        }

        // Переопределение метода ToString()
        public override string ToString()
        {
            string result = $"Research Team:\n Topic: {topic}\n Organization: {organization}\n Registration Number: {registrationNumber}\n Time Frame: {timeFrame}\n";

            // Добавление списка участников
            result += "Members:";
            if (members != null && members.Count > 0)
            {
                foreach (Person member in members)
                {
                    result += $"\n  - {member}";
                }
            }
            else
            {
                result += "\n  No members";
            }

            // Добавление списка публикаций
            result += "\nPublications:";
            if (publications != null && publications.Count > 0)
            {
                foreach (Paper publication in publications)
                {
                    result += $"\n  - {publication}";
                }
            }
            else
            {
                result += "\n  No publications";
            }

            return result;
        }

        // Метод ToShortString()
        public string ToShortString()
        {
            return $"Research Team:\n Topic: {topic}\n Organization: {organization}\n Registration Number: {registrationNumber}\n Time Frame: {timeFrame}\n Members Count: {members.Count}\n Publications Count: {publications.Count}";
        }

        // Переопределение метода DeepCopy
        public override object DeepCopy()
        {
            ResearchTeam copy = new ResearchTeam(topic, organization, registrationNumber, timeFrame);

            // Копирование участников
            foreach (Person member in members)
            {
                if (member != null)
                    copy.Members.Add(member.DeepCopy());
            }

            // Копирование публикаций
            foreach (Paper publication in publications)
            {
                if (publication != null)
                    copy.Publications.Add(publication.DeepCopy());
            }

            return copy;
        }

        // Итератор для перебора участников проекта, не имеющих публикаций
        public IEnumerable MembersWithoutPublications()
        {
            foreach (Person member in members)
            {
                bool hasPublication = false;
                foreach (Paper publication in publications)
                {
                    if (publication.Author.Equals(member))
                    {
                        hasPublication = true;
                        break;
                    }
                }

                if (!hasPublication)
                    yield return member;
            }
        }

        // Итератор с параметром для перебора публикаций за последние n лет
        public IEnumerable RecentPublications(int years)
        {
            DateTime cutoffDate = DateTime.Now.AddYears(-years);
            foreach (Paper publication in publications)
            {
                if (publication.PublicationDate >= cutoffDate)
                    yield return publication;
            }
        }

        // Реализация интерфейса IEnumerable для перебора участников с публикациями
        public IEnumerator GetEnumerator()
        {
            return new ResearchTeamEnumerator(this);
        }

        // Вспомогательный класс ResearchTeamEnumerator для реализации IEnumerator
        private class ResearchTeamEnumerator : IEnumerator
        {
            private ResearchTeam team;
            private int position = -1;
            private ArrayList membersWithPublications;

            public ResearchTeamEnumerator(ResearchTeam team)
            {
                this.team = team;
                this.membersWithPublications = new ArrayList();

                // Заполнение списка участников с публикациями
                foreach (Person member in team.members)
                {
                    foreach (Paper publication in team.publications)
                    {
                        if (publication.Author.Equals(member))
                        {
                            membersWithPublications.Add(member);
                            break;
                        }
                    }
                }
            }

            public bool MoveNext()
            {
                position++;
                return (position < membersWithPublications.Count);
            }

            public void Reset()
            {
                position = -1;
            }

            public object Current
            {
                get
                {
                    try
                    {
                        return membersWithPublications[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }

        // Итератор для перебора участников с более чем одной публикацией
        public IEnumerable MembersWithMultiplePublications()
        {
            foreach (Person member in members)
            {
                int publicationCount = 0;
                foreach (Paper publication in publications)
                {
                    if (publication.Author.Equals(member))
                    {
                        publicationCount++;
                    }
                }

                if (publicationCount > 1)
                    yield return member;
            }
        }

        // Итератор для перебора публикаций за последний год
        public IEnumerable LastYearPublications()
        {
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            foreach (Paper publication in publications)
            {
                if (publication.PublicationDate >= oneYearAgo)
                    yield return publication;
            }
        }
    }
}

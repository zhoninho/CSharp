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
        private string topic;
        private TimeFrame timeFrame;
        private ArrayList members;
        private ArrayList publications;

        public ResearchTeam(string topic, string organization, int registrationNumber, TimeFrame timeFrame)
            : base(organization, registrationNumber)
        {
            this.topic = topic;
            this.timeFrame = timeFrame;
            this.members = new ArrayList();
            this.publications = new ArrayList();
        }

        public ResearchTeam() : base()
        {
            topic = "Тема исследований по умолчанию";
            timeFrame = TimeFrame.Year;
            members = new ArrayList();
            publications = new ArrayList();
        }

        public string Topic
        {
            get { return topic; }
            set { topic = value; }
        }

        public TimeFrame TimeFrame
        {
            get { return timeFrame; }
            set { timeFrame = value; }
        }

        public ArrayList Publications
        {
            get { return publications; }
            set { publications = value; }
        }

        public ArrayList Members
        {
            get { return members; }
            set { members = value; }
        }

        string INameAndCopy.Name
        {
            get { return topic; }
            set { topic = value; }
        }

        object INameAndCopy.DeepCopy()
        {
            return DeepCopy();
        }

        public Team Team
        {
            get { return new Team(organization, registrationNumber); }
            set
            {
                organization = value.Organization;
                registrationNumber = value.RegistrationNumber;
            }
        }

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

        public bool this[TimeFrame frame]
        {
            get { return timeFrame == frame; }
        }

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

        public override string ToString()
        {
            string result = $"Research Team:\n Тема исследований: {topic}\n Организация: {organization}\n Номер регистрации: {registrationNumber}\n Продолжительность: {timeFrame}\n";

            result += "Участники:";
            if (members != null && members.Count > 0)
            {
                foreach (Person member in members)
                {
                    result += $"\n  - {member}";
                }
            }
            else
            {
                result += "\n  Нет участников";
            }

            result += "\nПубликации:";
            if (publications != null && publications.Count > 0)
            {
                foreach (Paper publication in publications)
                {
                    result += $"\n  - {publication}";
                }
            }
            else
            {
                result += "\n  Нет публикаций";
            }

            return result;
        }

        public string ToShortString()
        {
            return $"Research Team:\n Тема: {topic}\n Организация: {organization}\n Номер регистрации: {registrationNumber}\n Продолжительность: {timeFrame}\n Кол-во участников: {members.Count}\n Кол-во публикаций: {publications.Count}";
        }

        public override object DeepCopy()
        {
            ResearchTeam copy = new ResearchTeam(topic, organization, registrationNumber, timeFrame);

            foreach (Person member in members)
            {
                if (member != null)
                    copy.Members.Add(member.DeepCopy());
            }

            foreach (Paper publication in publications)
            {
                if (publication != null)
                    copy.Publications.Add(publication.DeepCopy());
            }

            return copy;
        }

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

        public IEnumerable RecentPublications(int years)
        {
            DateTime cutoffDate = DateTime.Now.AddYears(-years);
            foreach (Paper publication in publications)
            {
                if (publication.PublicationDate >= cutoffDate)
                    yield return publication;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new ResearchTeamEnumerator(this);
        }

        private class ResearchTeamEnumerator : IEnumerator
        {
            private ResearchTeam team;
            private int position = -1;
            private ArrayList membersWithPublications;

            public ResearchTeamEnumerator(ResearchTeam team)
            {
                this.team = team;
                this.membersWithPublications = new ArrayList();

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

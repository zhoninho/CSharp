using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    class ResearchTeam : Team, INameAndCopy, IEnumerable, IComparer<ResearchTeam>
    {
        private string researchTopic;
        private TimeFrame duration;
        private List<Person> members;
        private List<Paper> publications;

        public ResearchTeam() : base()
        {
            researchTopic = "No Topic";
            duration = TimeFrame.Year;
            members = new List<Person>();
            publications = new List<Paper>();
        }

        public ResearchTeam(string topic, string org, int regNumber, TimeFrame duration)
            : base(org, regNumber)
        {
            researchTopic = topic;
            this.duration = duration;
            members = new List<Person>();
            publications = new List<Paper>();
        }

        public string ResearchTopic
        {
            get { return researchTopic; }
            set { researchTopic = value; }
        }

        public TimeFrame Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public List<Person> Members
        {
            get { return members; }
            set { members = value; }
        }

        public List<Paper> Publications
        {
            get { return publications; }
            set { publications = value; }
        }

        public Paper LatestPublication
        {
            get
            {
                if (publications.Count == 0) return null;
                Paper latest = publications[0];
                foreach (Paper paper in publications)
                {
                    if (paper.PublicationDate > latest.PublicationDate)
                        latest = paper;
                }
                return latest;
            }
        }

        public Team TeamData
        {
            get { return new Team(organization, registrationNumber); }
            set
            {
                organization = value.Organization;
                RegistrationNumber = value.RegistrationNumber;
            }
        }

        // Явная реализация свойства Name из INameAndCopy
        string INameAndCopy.Name
        {
            get { return researchTopic; }
            set { researchTopic = value; }
        }

        // Явная реализация метода DeepCopy из INameAndCopy
        object INameAndCopy.DeepCopy()
        {
            return DeepCopy();
        }

        // Публичный метод DeepCopy
        public override object DeepCopy()
        {
            ResearchTeam copy = new ResearchTeam(researchTopic, organization, registrationNumber, duration);
            copy.Members = new List<Person>();
            foreach (Person member in members)
                copy.Members.Add((Person)member.DeepCopy());
            copy.Publications = new List<Paper>();
            foreach (Paper paper in publications)
                copy.Publications.Add((Paper)paper.DeepCopy());
            return copy;
        }

        public void AddMembers(params Person[] newMembers)
        {
            members.AddRange(newMembers);
        }

        public void AddPapers(params Paper[] newPapers)
        {
            publications.AddRange(newPapers);
        }

        public override string ToString()
        {
            string result = $"Topic: {researchTopic}, {base.ToString()}, Duration: {duration}\nMembers:\n";
            foreach (Person member in members)
                result += member.ToString() + "\n";
            result += "Publications:\n";
            foreach (Paper paper in publications)
                result += paper.ToString() + "\n";
            return result;
        }

        public string ToShortString()
        {
            return $"Topic: {researchTopic}, {base.ToString()}, Duration: {duration}, Members: {members.Count}, Publications: {publications.Count}";
        }

        public IEnumerable GetMembersWithoutPublications()
        {
            foreach (Person member in members)
            {
                bool hasPublication = false;
                foreach (Paper paper in publications)
                {
                    if (paper.Author == member)
                    {
                        hasPublication = true;
                        break;
                    }
                }
                if (!hasPublication)
                    yield return member;
            }
        }

        public IEnumerable GetPublicationsLastYears(int years)
        {
            DateTime threshold = DateTime.Now.AddYears(-years);
            foreach (Paper paper in publications)
            {
                if (paper.PublicationDate >= threshold)
                    yield return paper;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new ResearchTeamEnumerator(this);
        }

        public IEnumerable GetMembersWithMultiplePublications()
        {
            foreach (Person member in members)
            {
                int publicationCount = 0;
                foreach (Paper paper in publications)
                {
                    if (paper.Author == member)
                        publicationCount++;
                }
                if (publicationCount > 1)
                    yield return member;
            }
        }

        public IEnumerable GetPublicationsLastYear()
        {
            DateTime threshold = DateTime.Now.AddYears(-1);
            foreach (Paper paper in publications)
            {
                if (paper.PublicationDate >= threshold)
                    yield return paper;
            }
        }

        // Реализация IComparer<ResearchTeam> для сравнения по ResearchTopic
        public int Compare(ResearchTeam x, ResearchTeam y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return string.Compare(x.ResearchTopic, y.ResearchTopic);
        }
    }

}

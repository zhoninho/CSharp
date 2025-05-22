using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    class ResearchTeamCollection
    {
        private List<ResearchTeam> teams;

        public ResearchTeamCollection()
        {
            teams = new List<ResearchTeam>();
        }

        public void AddDefaults()
        {
            teams.Add(new ResearchTeam("AI Research", "Tech University", 123, TimeFrame.TwoYears));
            teams.Add(new ResearchTeam("ML Development", "Science Institute", 456, TimeFrame.Year));
            teams.Add(new ResearchTeam("Data Science", "Research Center", 789, TimeFrame.Long));
        }

        public void AddResearchTeams(params ResearchTeam[] newTeams)
        {
            teams.AddRange(newTeams);
        }

        public override string ToString()
        {
            string result = "ResearchTeamCollection:\n";
            foreach (ResearchTeam team in teams)
                result += team.ToString() + "\n";
            return result;
        }

        public virtual string ToShortString()
        {
            string result = "ResearchTeamCollection (Short):\n";
            foreach (ResearchTeam team in teams)
                result += team.ToShortString() + "\n";
            return result;
        }

        public void SortByRegistrationNumber()
        {
            teams.Sort();
        }

        public void SortByResearchTopic()
        {
            teams.Sort(new ResearchTeam());
        }

        public void SortByPublicationsCount()
        {
            teams.Sort(new PublicationsComparer());
        }

        public int MinRegistrationNumber
        {
            get
            {
                if (teams.Count == 0) return 0;
                int min = teams[0].RegistrationNumber;
                foreach (ResearchTeam team in teams)
                {
                    if (team.RegistrationNumber < min)
                        min = team.RegistrationNumber;
                }
                return min;
            }
        }

        public IEnumerable<ResearchTeam> TwoYearsProjects
        {
            get
            {
                foreach (ResearchTeam team in teams)
                {
                    if (team.Duration == TimeFrame.TwoYears)
                        yield return team;
                }
            }
        }

        public List<ResearchTeam> NGroup(int value)
        {
            List<ResearchTeam> result = new List<ResearchTeam>();
            foreach (ResearchTeam team in teams)
            {
                if (team.Members.Count == value)
                    result.Add(team);
            }
            return result;
        }
    }

}

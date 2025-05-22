using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    class ResearchTeamEnumerator : IEnumerator
    {
        private ResearchTeam team;
        private int currentIndex;
        private Person currentMember;

        public ResearchTeamEnumerator(ResearchTeam team)
        {
            this.team = team;
            currentIndex = -1;
            currentMember = null;
        }

        public object Current
        {
            get { return currentMember; }
        }

        public bool MoveNext()
        {
            while (++currentIndex < team.Members.Count)
            {
                Person member = team.Members[currentIndex];
                foreach (Paper paper in team.Publications)
                {
                    if (paper.Author == member)
                    {
                        currentMember = member;
                        return true;
                    }
                }
            }
            return false;
        }

        public void Reset()
        {
            currentIndex = -1;
            currentMember = null;
        }
    }

}

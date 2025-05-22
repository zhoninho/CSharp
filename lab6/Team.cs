using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    class Team : INameAndCopy, IComparable<Team>
    {
        protected string organization;
        protected int registrationNumber;

        public Team()
        {
            organization = "No Organization";
            registrationNumber = 0;
        }

        public Team(string organization, int registrationNumber)
        {
            this.organization = organization;
            RegistrationNumber = registrationNumber;
        }

        public string Organization
        {
            get { return organization; }
            set { organization = value; }
        }

        public int RegistrationNumber
        {
            get { return registrationNumber; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Registration number must be positive.");
                registrationNumber = value;
            }
        }

        // Явная реализация свойства Name из INameAndCopy
        string INameAndCopy.Name
        {
            get { return organization; }
            set { organization = value; }
        }

        // Явная реализация метода DeepCopy из INameAndCopy
        object INameAndCopy.DeepCopy()
        {
            return DeepCopy();
        }

        // Публичный виртуальный метод DeepCopy
        public virtual object DeepCopy()
        {
            return new Team(organization, registrationNumber);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Team other = (Team)obj;
            return organization == other.organization && registrationNumber == other.registrationNumber;
        }

        public static bool operator ==(Team t1, Team t2)
        {
            if (ReferenceEquals(t1, null) && ReferenceEquals(t2, null))
                return true;
            if (ReferenceEquals(t1, null) || ReferenceEquals(t2, null))
                return false;
            return t1.Equals(t2);
        }

        public static bool operator !=(Team t1, Team t2)
        {
            return !(t1 == t2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(organization, registrationNumber);
        }

        public override string ToString()
        {
            return $"Organization: {organization}, Reg.Number: {registrationNumber}";
        }

        // Реализация IComparable<Team> для сравнения по RegistrationNumber
        public int CompareTo(Team other)
        {
            if (other == null) return 1;
            return registrationNumber.CompareTo(other.registrationNumber);
        }
    }

}

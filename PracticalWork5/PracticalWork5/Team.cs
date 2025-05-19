using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork5
{
    internal class Team : INameAndCopy
    {
        protected string organization;
        protected int registrationNumber;

        public Team(string organization, int registrationNumber)
        {
            this.organization = organization;
            if (registrationNumber <= 0)
                throw new ArgumentException("Номер регистрации должен быть положительным числом");
            this.registrationNumber = registrationNumber;
        }

        public Team()
        {
            organization = "Организация по умолчанию";
            registrationNumber = 1;
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
                    throw new ArgumentException("Номер регистрации должен быть положительным числом");
                registrationNumber = value;
            }
        }

        string INameAndCopy.Name
        {
            get { return organization; }
            set { organization = value; }
        }

        public virtual object DeepCopy()
        {
            return new Team(organization, registrationNumber);
        }

        object INameAndCopy.DeepCopy()
        {
            return DeepCopy();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Team other = (Team)obj;
            return organization == other.organization &&
                   registrationNumber == other.registrationNumber;
        }

        public static bool operator ==(Team left, Team right)
        {
            if (ReferenceEquals(left, right)) return true;
            if ((object)left == null || (object)right == null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(Team left, Team right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Organization.GetHashCode() + registrationNumber.GetHashCode();
        }

        public override string ToString()
        {
            return $"Команда: Организация: {organization}, Номер Регистрации: {registrationNumber}";
        }
    }
}

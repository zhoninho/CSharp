using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork5
{
    internal class Team : INameAndCopy
    {
        // Защищенные поля
        protected string organization;
        protected int registrationNumber;

        // Конструктор с параметрами
        public Team(string organization, int registrationNumber)
        {
            this.organization = organization;
            if (registrationNumber <= 0)
                throw new ArgumentException("Registration number must be positive");
            this.registrationNumber = registrationNumber;
        }

        // Конструктор без параметров
        public Team()
        {
            organization = "Default Organization";
            registrationNumber = 1;
        }

        // Свойство для доступа к полю с названием организации
        public string Organization
        {
            get { return organization; }
            set { organization = value; }
        }

        // Свойство для доступа к полю с номером регистрации
        public int RegistrationNumber
        {
            get { return registrationNumber; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Registration number must be positive");
                registrationNumber = value;
            }
        }

        // Реализация свойства Name из интерфейса INameAndCopy
        public string Name
        {
            get { return organization; }
            set { organization = value; }
        }

        // Виртуальный метод DeepCopy для интерфейса INameAndCopy
        public virtual object DeepCopy()
        {
            return new Team(organization, registrationNumber);
        }

        // Переопределение метода Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Team other = (Team)obj;
            return organization == other.organization &&
                   registrationNumber == other.registrationNumber;
        }

        // Переопределение операторов == и !=
        public static bool operator ==(Team left, Team right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(Team left, Team right)
        {
            return !(left == right);
        }

        // Переопределение метода GetHashCode
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (organization?.GetHashCode() ?? 0);
            hash = hash * 23 + registrationNumber.GetHashCode();
            return hash;
        }

        // Переопределение метода ToString()
        public override string ToString()
        {
            return $"Team: Organization: {organization}, Registration Number: {registrationNumber}";
        }
    }
}

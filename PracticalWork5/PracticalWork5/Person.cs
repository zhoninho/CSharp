using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PracticalWork5
{
    internal class Person : INameAndCopy
    {
        // Закрытые поля
        private string firstName;
        private string lastName;
        private DateTime birthDate;

        // Конструктор с параметрами
        public Person(string firstName, string lastName, DateTime birthDate)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
        }

        // Конструктор без параметров
        public Person()
        {
            firstName = "John";
            lastName = "Doe";
            birthDate = new DateTime(2000, 1, 1);
        }

        // Свойства для доступа к полям
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public DateTime BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }

        // Свойство для получения и изменения года рождения
        public int BirthYear
        {
            get { return birthDate.Year; }
            set { birthDate = new DateTime(value, birthDate.Month, birthDate.Day); }
        }

        // Реализация свойства Name из интерфейса INameAndCopy
        public string Name
        {
            get { return firstName + " " + lastName; }
            set
            {
                string[] parts = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    firstName = parts[0];
                    lastName = parts[1];
                }
            }
        }

        // Переопределение метода Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Person other = (Person)obj;
            return firstName == other.firstName &&
                   lastName == other.lastName &&
                   birthDate == other.birthDate;
        }

        // Переопределение операторов == и !=
        public static bool operator ==(Person left, Person right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(Person left, Person right)
        {
            return !(left == right);
        }

        // Переопределение метода GetHashCode
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (firstName?.GetHashCode() ?? 0);
            hash = hash * 23 + (lastName?.GetHashCode() ?? 0);
            hash = hash * 23 + birthDate.GetHashCode();
            return hash;
        }

        // Метод DeepCopy для интерфейса INameAndCopy
        public virtual object DeepCopy()
        {
            return new Person(firstName, lastName, birthDate);
        }

        // Переопределение метода ToString()
        public override string ToString()
        {
            return $"Person: {firstName} {lastName}, Birth Date: {birthDate.ToShortDateString()}";
        }

        // Виртуальный метод ToShortString()
        public virtual string ToShortString()
        {
            return $"{firstName} {lastName}";
        }
    }
}


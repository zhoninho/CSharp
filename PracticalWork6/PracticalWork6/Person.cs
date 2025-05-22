using PracticalWork6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PracticalWork6
{
    internal class Person : INameAndCopy
    {
        private string firstName;
        private string lastName;
        private DateTime birthDate;

        public Person(string firstName, string lastName, DateTime birthDate)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
        }

        public Person()
        {
            firstName = "Неизвестно";
            lastName = "Неизвестно";
            birthDate = new DateTime(2000, 1, 1);
        }

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

        public int BirthYear
        {
            get { return birthDate.Year; }
            set { birthDate = new DateTime(value, birthDate.Month, birthDate.Day); }
        }

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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Person other = (Person)obj;
            return firstName == other.firstName &&
                   lastName == other.lastName &&
                   birthDate == other.birthDate;
        }

        public static bool operator ==(Person left, Person right)
        {
            if (ReferenceEquals(left, right)) return true;
            if ((object)left == null || (object)right == null) return false;
            return left.firstName == right.firstName &&
                left.lastName == right.lastName &&
                left.birthDate == right.birthDate;
        }

        public static bool operator !=(Person left, Person right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return firstName.GetHashCode() + lastName.GetHashCode() + birthDate.GetHashCode();
        }

        public virtual object DeepCopy()
        {
            return new Person(firstName, lastName, birthDate);
        }

        public override string ToString()
        {
            return $"ФИО: {firstName} {lastName}, Дата рожд-я: {birthDate.ToShortDateString()}";
        }

        public virtual string ToShortString()
        {
            return $"{firstName} {lastName}";
        }
    }
}
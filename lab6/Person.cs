using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    class Person : INameAndCopy
    {
        private string firstName;
        private string lastName;
        private DateTime birthDate;

        public Person() : this("John", "Doe", new DateTime(1990, 1, 1)) { }

        public Person(string firstName, string lastName, DateTime birthDate)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
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

        public string Name
        {
            get { return firstName + " " + lastName; }
            set
            {
                string[] names = value.Split(' ');
                firstName = names[0];
                lastName = names.Length > 1 ? names[1] : "";
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Person other = (Person)obj;
            return firstName == other.firstName && lastName == other.lastName && birthDate == other.birthDate;
        }

        public static bool operator ==(Person p1, Person p2)
        {
            if (ReferenceEquals(p1, null) && ReferenceEquals(p2, null))
                return true;
            if (ReferenceEquals(p1, null) || ReferenceEquals(p2, null))
                return false;
            return p1.Equals(p2);
        }

        public static bool operator !=(Person p1, Person p2)
        {
            return !(p1 == p2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(firstName, lastName, birthDate);
        }

        public virtual object DeepCopy()
        {
            return new Person(firstName, lastName, birthDate);
        }

        public override string ToString()
        {
            return $"Name: {firstName} {lastName}, Birth Date: {birthDate.ToShortDateString()}";
        }

        public virtual string ToShortString()
        {
            return firstName + " " + lastName;
        }
    }

}

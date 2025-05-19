using System;

namespace PracticalWork3
{
    public class Person
    {
        private string name;
        private string surname;
        private System.DateTime birthDate;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        public System.DateTime BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }

        public int BirthYear
        {
            get { return birthDate.Year; }
            set { birthDate = new DateTime(value, birthDate.Month, birthDate.Day); }
        }

        public Person(Person other)
        {
            this.name = other.name;
            this.surname = other.surname;
            this.birthDate = other.birthDate;
        }


        public Person()
        {
            name = "N/A";
            surname = "N/A";
            birthDate = System.DateTime.Now;
        }

        public Person(string name, string surname, System.DateTime birthDate)
        {
            this.name = name;
            this.surname = surname;
            this.birthDate = birthDate;
        }

        

        public override string ToString()
        {
            return "Name: " + Name + " Surname: " + Surname + " Year of birth: " + BirthDate + " ";
        }

        virtual public string ToShortString()
        {
            return "Name: " + Name + " Surname: " + Surname + " ";
        }
    }
}

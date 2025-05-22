// Person.cs
using System;
// ... другие using ...

namespace PracticalWork9
{
    public class Person : INameAndCopy
    {
        private string firstName;
        private string lastName;
        private DateTime birthDate;

        // Конструктор по умолчанию, необходим для JSON-десериализации, если нет других подходящих
        public Person() : this("Неизвестно", "Неизвестно", DateTime.MinValue) { }

        public Person(string firstName, string lastName, DateTime birthDate)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
        }

        // Свойства должны быть public с get и set для JSON сериализации/десериализации
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

        // Реализация INameAndCopy.Name (используется для обратной совместимости, если где-то есть)
        // Для JSON-сериализации важны именно публичные свойства FirstName, LastName
        string INameAndCopy.Name
        {
            get { return firstName + " " + lastName; }
            set
            {
                string[] names = value.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (names.Length > 0) this.firstName = names[0]; else this.firstName = "";
                if (names.Length > 1) this.lastName = names[1]; else this.lastName = "";
            }
        }

        // Публичное свойство Name, которое будет сериализовано, если нужно
        // Но обычно сериализуют FirstName и LastName отдельно
        public string FullNameForSerialization // Пример, если нужно отдельное свойство для полного имени
        {
            get { return firstName + " " + lastName; }
            // set не нужен, если собирается из FirstName/LastName
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
            return $"Имя: {firstName} {lastName}, Дата рождения: {birthDate.ToShortDateString()}";
        }

        public virtual string ToShortString()
        {
            return firstName + " " + lastName;
        }
    }
}
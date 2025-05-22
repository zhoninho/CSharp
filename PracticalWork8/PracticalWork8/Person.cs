// Person.cs
using PracticalWork8;
using System;
using System.Collections.Generic; // Не используется напрямую
using System.Linq; // Не используется напрямую
using System.Text; // Не используется напрямую
using System.Threading.Tasks; // Не используется, можно убрать

namespace PracticalWork8
{
    // Класс, представляющий человека (персону).
    // Реализует интерфейс INameAndCopy.
    public class Person : INameAndCopy
    {
        // Приватное поле для хранения имени.
        private string firstName;
        // Приватное поле для хранения фамилии.
        private string lastName;
        // Приватное поле для хранения даты рождения.
        private DateTime birthDate;

        // Конструктор по умолчанию.
        // Вызывает другой конструктор этого же класса (this) с параметрами по умолчанию.
        public Person() : this("Иван", "Иванов", new DateTime(1990, 1, 1)) { }

        // Конструктор с параметрами.
        // Инициализирует поля объекта заданными значениями.
        public Person(string firstName, string lastName, DateTime birthDate)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
        }

        // Свойство для доступа к имени (FirstName).
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        // Свойство для доступа к фамилии (LastName).
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        // Свойство для доступа к дате рождения (BirthDate).
        public DateTime BirthDate
        {
            get { return birthDate; }
            // Позволяет изменить дату рождения, если объект не должен быть иммутабельным в этом аспекте.
            set { birthDate = value; }
        }

        // Реализация свойства Name из интерфейса INameAndCopy.
        // Для класса Person, Name - это полное имя (имя + фамилия).
        public string Name
        {
            get { return firstName + " " + lastName; }
            set
            {
                // При установке имени, строка разделяется на имя и фамилию.
                // Предполагается, что имя и фамилия разделены пробелом.
                string[] names = value.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries); // Разделяем максимум на 2 части
                if (names.Length > 0) firstName = names[0];
                else firstName = ""; // Если строка пустая или только пробелы

                if (names.Length > 1) lastName = names[1];
                else lastName = ""; // Если нет второго слова (фамилии)
            }
        }

        // Переопределение метода Equals для сравнения объектов Person.
        // Два объекта Person считаются равными, если у них совпадают имя, фамилия и дата рождения.
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Person other = (Person)obj;
            return firstName == other.firstName &&
                   lastName == other.lastName &&
                   birthDate == other.birthDate;
        }

        // Перегрузка оператора равенства (==).
        public static bool operator ==(Person p1, Person p2)
        {
            // Если оба объекта null, они равны.
            if (ReferenceEquals(p1, null) && ReferenceEquals(p2, null))
                return true;
            // Если один из объектов null, а другой нет, они не равны.
            if (ReferenceEquals(p1, null) || ReferenceEquals(p2, null))
                return false;
            // Иначе, используем метод Equals для сравнения.
            return p1.Equals(p2);
        }

        // Перегрузка оператора неравенства (!=).
        public static bool operator !=(Person p1, Person p2)
        {
            return !(p1 == p2);
        }

        // Переопределение метода GetHashCode.
        // Важно для корректной работы объектов Person в хеш-таблицах (например, Dictionary).
        public override int GetHashCode()
        {
            // Используем HashCode.Combine для генерации хеш-кода на основе ключевых полей.
            return HashCode.Combine(firstName, lastName, birthDate);
        }

        // Реализация метода DeepCopy из интерфейса INameAndCopy.
        // Создает и возвращает глубокую копию текущего объекта Person.
        public virtual object DeepCopy()
        {
            // firstName и lastName (string) копируются по ссылке, но строки иммутабельны.
            // birthDate (DateTime) - значимый тип, копируется по значению.
            // Поэтому для Person достаточно создать новый объект с теми же значениями полей.
            return new Person(firstName, lastName, birthDate);
        }

        // Переопределение метода ToString().
        // Возвращает полное строковое представление объекта Person.
        public override string ToString()
        {
            return $"Имя: {firstName} {lastName}, Дата рождения: {birthDate.ToShortDateString()}";
        }

        // Метод ToShortString().
        // Возвращает краткое строковое представление объекта Person (только имя и фамилия).
        public virtual string ToShortString()
        {
            return firstName + " " + lastName;
        }
    }
}
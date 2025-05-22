// Team.cs
using System;
// ... другие using ...

namespace PracticalWork9
{
    public class Team : INameAndCopy, IComparable<Team>
    {
        // Поля должны быть public или иметь public свойства для JSON-сериализации
        // Если оставлять protected, то System.Text.Json их не увидит по умолчанию.
        // Сделаем свойства public.
        protected string _organization; // переименуем, чтобы не конфликтовать со свойством
        protected int _registrationNumber;

        // Конструктор по умолчанию для JSON
        public Team()
        {
            _organization = "Нет организации";
            _registrationNumber = 0; // Будет перезаписано при десериализации, если есть в JSON
        }

        public Team(string organization, int registrationNumber)
        {
            this.Organization = organization; // Используем свойство для валидации
            this.RegistrationNumber = registrationNumber; // Используем свойство для валидации
        }

        public string Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

        public int RegistrationNumber
        {
            get { return _registrationNumber; }
            set
            {
                if (value < 0 && value != 0) // Разрешим 0 для конструктора по умолчанию, но не для пользовательского ввода
                    throw new ArgumentException("Регистрационный номер должен быть положительным или 0 для инициализации.");
                _registrationNumber = value;
            }
        }

        string INameAndCopy.Name
        {
            get { return Organization; }
            set { Organization = value; }
        }

        object INameAndCopy.DeepCopy()
        {
            return DeepCopy();
        }

        // Типизированный DeepCopy для удобства, если нужен
        public Team DeepCopyTyped()
        {
            return new Team(Organization, RegistrationNumber);
        }

        public virtual object DeepCopy()
        {
            return new Team(Organization, RegistrationNumber);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Team other = (Team)obj;
            return Organization == other.Organization && RegistrationNumber == other.RegistrationNumber;
        }

        public static bool operator ==(Team t1, Team t2)
        {
            if (ReferenceEquals(t1, null) && ReferenceEquals(t2, null)) return true;
            if (ReferenceEquals(t1, null) || ReferenceEquals(t2, null)) return false;
            return t1.Equals(t2);
        }

        public static bool operator !=(Team t1, Team t2)
        {
            return !(t1 == t2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Organization, RegistrationNumber);
        }

        public override string ToString()
        {
            return $"Организация: {Organization}, Рег.номер: {RegistrationNumber}";
        }

        public int CompareTo(Team other)
        {
            if (other == null) return 1;
            return RegistrationNumber.CompareTo(other.RegistrationNumber);
        }
    }
}
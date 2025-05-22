using PracticalWork7;
using System;
using System.Collections.Generic; // Для IComparable<T>
using System.Linq;              // Не используется напрямую
using System.Text;              // Не используется напрямую
using System.Threading.Tasks;   // Не используется, можно убрать

namespace PracticalWork7
{
    // Базовый класс Team, представляющий команду или организацию.
    // Реализует интерфейсы INameAndCopy и IComparable<Team>.
    class Team : INameAndCopy, IComparable<Team>
    {
        // Защищенное поле для названия организации. Доступно в этом классе и наследниках.
        protected string organization;
        // Защищенное поле для регистрационного номера.
        protected int registrationNumber;

        // Конструктор по умолчанию.
        // Инициализирует объект значениями по умолчанию.
        public Team()
        {
            organization = "Нет организации"; // Название организации по умолчанию.
            // registrationNumber по умолчанию 0. Сеттер требует >0.
            // Если при создании объекта Team без параметров нужен валидный номер,
            // то лучше либо присвоить здесь дефолтный положительный номер,
            // либо изменить логику сеттера/конструктора.
            // Оставим 0, предполагая, что он будет установлен позже через свойство или конструктор с параметрами.
            // Однако, если сразу после этого конструктора вызвать, например, ToString(), где используется regNumber,
            // это может быть не совсем корректно, если 0 - невалидное состояние.
            // Для данного задания, где номер устанавливается в конструкторе или через свойство с валидацией, это приемлемо.
        }

        // Конструктор с параметрами.
        // Инициализирует объект заданными значениями организации и регистрационного номера.
        public Team(string organization, int registrationNumber)
        {
            this.organization = organization;
            // Используем свойство RegistrationNumber для установки значения,
            // чтобы применилась валидация (номер должен быть положительным).
            RegistrationNumber = registrationNumber;
        }

        // Свойство для доступа к названию организации.
        public string Organization
        {
            get { return organization; }
            set { organization = value; }
        }

        // Свойство для доступа к регистрационному номеру.
        public int RegistrationNumber
        {
            get { return registrationNumber; }
            set
            {
                // Валидация: регистрационный номер должен быть строго положительным.
                if (value <= 0)
                    throw new ArgumentException("Регистрационный номер должен быть положительным.");
                registrationNumber = value;
            }
        }

        // Явная реализация свойства Name из интерфейса INameAndCopy.
        // Для класса Team, Name - это название организации.
        string INameAndCopy.Name
        {
            get { return organization; }
            set { organization = value; }
        }

        // Явная реализация метода DeepCopy из интерфейса INameAndCopy.
        // Вызывает публичный виртуальный метод DeepCopy.
        object INameAndCopy.DeepCopy()
        {
            return DeepCopy();
        }

        // Публичный виртуальный метод DeepCopy.
        // Создает глубокую копию объекта Team.
        // "Виртуальный", чтобы классы-наследники могли его переопределить для своей специфики.
        public virtual object DeepCopy()
        {
            // organization (string) копируется по ссылке, но строки иммутабельны.
            // registrationNumber (int) - значимый тип, копируется по значению.
            // Поэтому для Team достаточно создать новый объект с теми же значениями полей.
            return new Team(organization, registrationNumber);
        }

        // Переопределение метода Equals для сравнения объектов Team.
        // Два объекта Team считаются равными, если у них совпадают организация и регистрационный номер.
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Team other = (Team)obj;
            return organization == other.organization &&
                   registrationNumber == other.registrationNumber;
        }

        // Перегрузка оператора равенства (==).
        public static bool operator ==(Team t1, Team t2)
        {
            if (ReferenceEquals(t1, null) && ReferenceEquals(t2, null)) return true;
            if (ReferenceEquals(t1, null) || ReferenceEquals(t2, null)) return false;
            return t1.Equals(t2);
        }

        // Перегрузка оператора неравенства (!=).
        public static bool operator !=(Team t1, Team t2)
        {
            return !(t1 == t2);
        }

        // Переопределение метода GetHashCode.
        // Важно для корректной работы объектов Team в хеш-таблицах.
        public override int GetHashCode()
        {
            return HashCode.Combine(organization, registrationNumber);
        }

        // Переопределение метода ToString().
        // Возвращает строковое представление объекта Team.
        public override string ToString()
        {
            return $"Организация: {organization}, Рег.номер: {registrationNumber}";
        }

        // Реализация метода CompareTo из интерфейса IComparable<Team>.
        // Позволяет сравнивать объекты Team по регистрационному номеру.
        // Используется для стандартной сортировки.
        public int CompareTo(Team other)
        {
            if (other == null) return 1; // Текущий объект "больше", если другой null (стандартное поведение).
            // Сравнение по регистрационному номеру.
            return registrationNumber.CompareTo(other.registrationNumber);
        }
    }
}
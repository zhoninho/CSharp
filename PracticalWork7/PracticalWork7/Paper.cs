using PracticalWork7;
using System;
using System.Collections.Generic; // Не используется напрямую, но может быть полезно для расширений
using System.Linq; // Не используется напрямую
using System.Text; // Не используется напрямую
using System.Threading.Tasks; // Не используется, можно убрать

namespace PracticalWork7
{
    // Класс, представляющий научную публикацию (статью).
    // Реализует интерфейс INameAndCopy.
    class Paper : INameAndCopy
    {
        // Открытое автореализуемое свойство для названия статьи.
        public string Title { get; set; }
        // Открытое автореализуемое свойство для автора статьи (объект типа Person).
        public Person Author { get; set; }
        // Открытое автореализуемое свойство для даты публикации.
        public DateTime PublicationDate { get; set; }

        // Конструктор по умолчанию.
        // Инициализирует объект значениями по умолчанию.
        public Paper()
        {
            Title = "Без названия";         // Название по умолчанию
            Author = new Person();          // Автор по умолчанию (создается новый объект Person)
            PublicationDate = DateTime.Now; // Дата публикации по умолчанию (текущая дата и время)
        }

        // Конструктор с параметрами.
        // Позволяет создать объект Paper с заданными значениями.
        public Paper(string title, Person author, DateTime publicationDate)
        {
            Title = title;
            Author = author;
            PublicationDate = publicationDate;
        }

        // Реализация свойства Name из интерфейса INameAndCopy.
        // Для класса Paper, Name соответствует свойству Title (название статьи).
        public string Name
        {
            get { return Title; }
            set { Title = value; }
        }

        // Реализация метода DeepCopy из интерфейса INameAndCopy.
        // Создает и возвращает глубокую копию текущего объекта Paper.
        public virtual object DeepCopy()
        {
            // Создаем новый объект Person путем глубокого копирования текущего автора.
            Person authorCopy = (Person)Author.DeepCopy();
            // Создаем новый объект Paper, используя скопированные и исходные (для значимых типов) данные.
            // Title (string) копируется по ссылке, но строки иммутабельны, так что это безопасно (эффективно как копия по значению).
            // PublicationDate (DateTime) является структурой (значимый тип), поэтому копируется по значению.
            return new Paper(Title, authorCopy, PublicationDate);
        }

        // Переопределение метода ToString().
        // Возвращает строковое представление объекта Paper.
        public override string ToString()
        {
            // Используем Author.ToShortString() для краткой информации об авторе.
            // PublicationDate.ToShortDateString() для форматирования даты.
            return $"Название: {Title}, Автор: {Author.ToShortString()}, Дата: {PublicationDate.ToShortDateString()}";
        }
    }
}

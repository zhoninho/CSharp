// Paper.cs
using PracticalWork9;
using System;
// ... другие using ...

namespace PracticalWork9
{
    public class Paper : INameAndCopy
    {
        // Свойства должны быть public с get и set для JSON сериализации/десериализации
        public string Title { get; set; }
        public Person Author { get; set; }
        public DateTime PublicationDate { get; set; }

        // Конструктор по умолчанию, необходим для JSON-десериализации
        public Paper()
        {
            Title = "Без названия";
            Author = new Person(); // Важно инициализировать ссылочные типы
            PublicationDate = DateTime.Now;
        }

        public Paper(string title, Person author, DateTime publicationDate)
        {
            Title = title;
            Author = author;
            PublicationDate = publicationDate;
        }

        // Реализация INameAndCopy.Name
        string INameAndCopy.Name
        {
            get { return Title; }
            set { Title = value; }
        }

        public virtual object DeepCopy()
        {
            Person authorCopy = (Author != null) ? (Person)Author.DeepCopy() : new Person();
            return new Paper(Title, authorCopy, PublicationDate);
        }

        public override string ToString()
        {
            string authorStr = Author != null ? Author.ToShortString() : "Нет автора";
            return $"Название: {Title}, Автор: {authorStr}, Дата: {PublicationDate.ToShortDateString()}";
        }
    }
}
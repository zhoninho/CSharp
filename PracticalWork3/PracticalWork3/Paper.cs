using System;

namespace PracticalWork3
{
    public class Paper
    {
        public string Title { get; set; }
        public Person Author { get; set; }
        public DateTime PublicationDate { get; set; }

        // Конструктор с параметрами
        public Paper(string title, Person author, DateTime publicationDate)
        {
            Title = title;
            Author = author;
            PublicationDate = publicationDate;
        }

        // Конструктор без параметров
        public Paper()
        {
            Title = "Безымянная публикация";
            Author = new Person();
            PublicationDate = DateTime.Now;
        }

        // Переопределение метода ToString
        public override string ToString()
        {
            return $"Название: {Title}, Автор: {Author}, Дата публикации: {PublicationDate.ToShortDateString()}";
        }

    }
}
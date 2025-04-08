using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork5
{
    internal class Paper : INameAndCopy
    {
        // Свойства
        public string Title { get; set; }
        public Person Author { get; set; }
        public DateTime PublicationDate { get; set; }

        // Реализация свойства Name из интерфейса INameAndCopy
        public string Name
        {
            get { return Title; }
            set { Title = value; }
        }

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
            Title = "Default Title";
            Author = new Person();
            PublicationDate = DateTime.Now.AddDays(-30);
        }

        // Переопределение метода ToString()
        public override string ToString()
        {
            return $"Paper: {Title}, Author: {Author}, Publication Date: {PublicationDate.ToShortDateString()}";
        }

        // Виртуальный метод DeepCopy для интерфейса INameAndCopy
        public virtual object DeepCopy()
        {
            Person authorCopy = Author != null ? (Person)Author.DeepCopy() : null;
            return new Paper(Title, authorCopy, PublicationDate);
        }
    }
}

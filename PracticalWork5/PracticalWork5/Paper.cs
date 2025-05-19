using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork5
{
    internal class Paper : INameAndCopy
    {
        public string Title { get; set; }
        public Person Author { get; set; }
        public DateTime PublicationDate { get; set; }

        public string Name
        {
            get { return Title; }
            set { Title = value; }
        }

        public Paper(string title, Person author, DateTime publicationDate)
        {
            Title = title;
            Author = author;
            PublicationDate = publicationDate;
        }

        public Paper()
        {
            Title = "Название по умолчанию";
            Author = new Person();
            PublicationDate = DateTime.Now.AddDays(-30);
        }

        public override string ToString()
        {
            return $"Paper: {Title}, Автор: {Author}, Дата публикации: {PublicationDate.ToShortDateString()}";
        }

        public virtual object DeepCopy()
        {
            Person authorCopy = Author != null ? (Person)Author.DeepCopy() : null;
            return new Paper(Title, authorCopy, PublicationDate);
        }
    }
}

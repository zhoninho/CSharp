using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    class Paper : INameAndCopy
    {
        public string Title { get; set; }
        public Person Author { get; set; }
        public DateTime PublicationDate { get; set; }

        public Paper()
        {
            Title = "Untitled";
            Author = new Person();
            PublicationDate = DateTime.Now;
        }

        public Paper(string title, Person author, DateTime publicationDate)
        {
            Title = title;
            Author = author;
            PublicationDate = publicationDate;
        }

        public string Name
        {
            get { return Title; }
            set { Title = value; }
        }

        public virtual object DeepCopy()
        {
            return new Paper(Title, (Person)Author.DeepCopy(), PublicationDate);
        }

        public override string ToString()
        {
            return $"Title: {Title}, Author: {Author.ToShortString()}, Date: {PublicationDate.ToShortDateString()}";
        }
    }
}

using System;
using System.Linq;

namespace PracticalWork3
{
    public enum TimeFrame
    {
        Year,
        TwoYears,
        Long
    }
    public class ResearchTeam
    {
        private string topic;
        private string organization;
        private int registrationNumber;
        private TimeFrame timeFrame;
        private Paper[] publications;

        // Конструктор с параметрами
        public ResearchTeam(string topic, string organization, int registrationNumber, TimeFrame timeFrame)
        {
            this.topic = topic;
            this.organization = organization;
            this.registrationNumber = registrationNumber;
            this.timeFrame = timeFrame;
            this.publications = new Paper[0];
        }

        // Конструктор без параметров
        public ResearchTeam()
        {
            topic = "Неизвестная тема";
            organization = "Неизвестная организация";
            registrationNumber = 0;
            timeFrame = TimeFrame.Year;
            publications = new Paper[0];
        }

        // Свойства
        public string Topic
        {
            get { return topic; }
            set { topic = value; }
        }

        public string Organization
        {
            get { return organization; }
            set { organization = value; }
        }

        public int RegistrationNumber
        {
            get { return registrationNumber; }
            set { registrationNumber = value; }
        }

        public TimeFrame TimeFrame
        {
            get { return timeFrame; }
            set { timeFrame = value; }
        }

        public Paper[] Publications
        {
            get { return publications; }
            set { publications = value; }
        }

        // Свойство для получения самой поздней публикации
        public Paper LatestPublication
        {
            get
            {
                if (publications == null || publications.Length == 0)
                    return null;

                Paper latest = publications[0];
                foreach (Paper paper in publications)
                {
                    if (paper.PublicationDate > latest.PublicationDate)
                        latest = paper;
                }
                return latest;
            }
        }

        // Индексатор для проверки соответствия timeFrame
        public bool this[TimeFrame tf]
        {
            get { return timeFrame == tf; }
        }

        // Метод для добавления публикаций
        public void AddPapers(params Paper[] newPapers)
        {
            if (newPapers == null)
                return;

            int oldLength = publications.Length;
            Array.Resize(ref publications, oldLength + newPapers.Length);
            Array.Copy(newPapers, 0, publications, oldLength, newPapers.Length);
        }

        // Переопределение метода ToString
        public override string ToString()
        {
            string result = $"Тема: {topic}, Организация: {organization}, " +
                            $"Регистрационный номер: {registrationNumber}, " +
                            $"Продолжительность исследований: {timeFrame}\n";

            result += "Публикации:\n";
            if (publications.Length == 0)
            {
                result += "Нет публикаций";
            }
            else
            {
                for (int i = 0; i < publications.Length; i++)
                {
                    result += $"  {i + 1}. {publications[i]}\n";
                }
            }
            return result;
        }

        // Метод ToShortString
        public virtual string ToShortString()
        {
            return $"Тема: {topic}, Организация: {organization}, " +
                   $"Регистрационный номер: {registrationNumber}, " +
                   $"Продолжительность исследований: {timeFrame}";
        }
    }

}

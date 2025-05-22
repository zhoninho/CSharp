using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7
{
    // Класс TeamsJournalEntry содержит информацию об отдельном изменении в коллекции ResearchTeamCollection
    public class TeamsJournalEntry
    {
        // Открытое автореализуемое свойство типа string с названием коллекции, в которой произошло событие
        public string CollectionName { get; set; }

        // Открытое автореализуемое свойство типа string с информацией о том, какое событие произошло в коллекции
        public string ChangeType { get; set; } // Например, "Добавлен элемент", "Вставлен элемент"

        // Номер нового элемента (индекс в коллекции)
        public int ElementNumber { get; set; }

        // Конструктор для инициализации полей класса
        public TeamsJournalEntry(string collectionName, string changeType, int elementNumber)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ElementNumber = elementNumber;
        }

        // Перегруженная версия метода string ToString()
        public override string ToString()
        {
            return $"Запись: Коллекция='{CollectionName}', Изменение='{ChangeType}', Индекс элемента={ElementNumber}";
        }
    }
}

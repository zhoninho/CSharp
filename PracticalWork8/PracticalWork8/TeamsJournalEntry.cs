// TeamsJournalEntry.cs
using PracticalWork8;

namespace PracticalWork8
{
    /// <summary>
    /// Представляет запись в журнале TeamsJournal об одном изменении
    /// в коллекции ResearchTeamCollection<TKey> или ее элементе.
    /// </summary>
    public class TeamsJournalEntry
    {
        /// <summary>
        /// Название коллекции, в которой произошло событие.
        /// </summary>
        public string CollectionName { get; }

        /// <summary>
        /// Тип изменения, вызвавший событие.
        /// </summary>
        public Revision RevisionType { get; }

        /// <summary>
        /// Название свойства класса ResearchTeam, которое явилось причиной изменения данных элемента.
        /// Пустая строка, если событие вызвано удалением или заменой элемента.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Номер регистрации объекта ResearchTeam для удаленного элемента,
        /// замененного элемента (номер старого элемента) или элемента, данные которого были изменены.
        /// </summary>
        public int RegistrationNumber { get; }

        /// <summary>
        /// Конструктор для инициализации всех свойств записи журнала.
        /// </summary>
        public TeamsJournalEntry(string collectionName, Revision revisionType, string propertyName, int registrationNumber)
        {
            CollectionName = collectionName;
            RevisionType = revisionType;
            PropertyName = propertyName ?? string.Empty;
            RegistrationNumber = registrationNumber;
        }

        /// <summary>
        /// Возвращает строковое представление записи журнала.
        /// </summary>
        public override string ToString()
        {
            return $"Запись журнала: Коллекция='{CollectionName}', Тип изменения='{RevisionType}', " +
                   $"Измененное свойство='{(string.IsNullOrEmpty(PropertyName) ? "N/A" : PropertyName)}', " +
                   $"Рег.Номер затронутого RT={RegistrationNumber}";
        }
    }
}
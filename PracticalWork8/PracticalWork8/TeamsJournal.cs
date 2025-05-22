// TeamsJournal.cs
using PracticalWork8;
using System.Collections.Generic;
using System.Text;

namespace PracticalWork8
{
    /// <summary>
    /// Класс для накопления информации об изменениях в коллекциях типа ResearchTeamCollection<TKey>.
    /// Подписывается на событие ResearchTeamsChanged и логирует полученную информацию.
    /// </summary>
    public class TeamsJournal
    {
        // Закрытое поле List<TeamsJournalEntry> для списка изменений.
        private List<TeamsJournalEntry> entries;

        /// <summary>
        /// Конструктор. Инициализирует пустой список записей журнала.
        /// </summary>
        public TeamsJournal()
        {
            entries = new List<TeamsJournalEntry>();
        }

        /// <summary>
        /// Обработчик события ResearchTeamsChanged.
        /// Создает новую запись TeamsJournalEntry на основе полученных данных и добавляет ее в список.
        /// </summary>
        /// <typeparam name="TKey">Тип ключа коллекции, от которой пришло событие.</typeparam>
        /// <param name="source">Источник события.</param>
        /// <param name="args">Аргументы события.</param>
        public void HandleResearchTeamsChanged<TKey>(object source, ResearchTeamsChangedEventArgs<TKey> args)
        {
            TeamsJournalEntry entry = new TeamsJournalEntry(
                args.CollectionName,
                args.RevisionType,
                args.PropertyName,
                args.RegistrationNumber);
            entries.Add(entry);
            // Можно добавить вывод в консоль при получении события для отладки
            // Console.WriteLine($"LOGGED TO JOURNAL: {entry}");
        }

        /// <summary>
        /// Возвращает строковое представление всех записей в журнале.
        /// </summary>
        public override string ToString()
        {
            if (entries.Count == 0)
            {
                return "Журнал TeamsJournal пуст.";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Содержимое журнала TeamsJournal ({entries.Count} записей):");
            foreach (var entry in entries)
            {
                sb.AppendLine($"  {entry.ToString()}");
            }
            return sb.ToString();
        }
    }
}

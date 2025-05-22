using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7
{
    // Класс TeamsJournal для накопления информации об изменениях в коллекциях типа ResearchTeamCollection
    public class TeamsJournal
    {
        // Закрытое поле List<TeamsJournalEntry> для списка изменений
        private List<TeamsJournalEntry> entries;

        // Конструктор
        public TeamsJournal()
        {
            entries = new List<TeamsJournalEntry>();
        }

        // Обработчик событий ResearchTeamAdded и ResearchTeamInserted.
        // Этот метод будет вызываться, когда соответствующее событие произойдет в ResearchTeamCollection.
        public void HandleTeamEvent(object sender, TeamListHandlerEventArgs args)
        {
            // sender - объект, вызвавший событие (ResearchTeamCollection)
            // args - данные события (TeamListHandlerEventArgs)

            // Создаем новую запись журнала на основе информации из аргументов события
            TeamsJournalEntry entry = new TeamsJournalEntry(args.CollectionName, args.ChangeType, args.ElementNumber);

            // Добавляем запись в список
            entries.Add(entry);
        }

        // Перегруженная версия метода string ToString() для формирования строки
        // с информацией обо всех элементах списка List<TeamsJournalEntry>.
        public override string ToString()
        {
            if (entries.Count == 0)
            {
                return "Журнал пуст.";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Содержимое журнала TeamsJournal:");
            foreach (var entry in entries)
            {
                sb.AppendLine($"  {entry.ToString()}");
            }
            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7
{
    // Делегат для обработки событий изменения в коллекции команд
    // object source - источник события (объект ResearchTeamCollection)
    // TeamListHandlerEventArgs args - аргументы события
    public delegate void TeamListHandler(object source, TeamListHandlerEventArgs args);

    // Класс аргументов события для TeamListHandler
    // Наследуется от EventArgs, стандартного базового класса для всех данных событий
    public class TeamListHandlerEventArgs : EventArgs
    {
        // Открытое автореализуемое свойство типа string с названием коллекции, в которой произошло событие
        public string CollectionName { get; set; }

        // Открытое автореализуемое свойство типа string с информацией о типе изменений в коллекции
        public string ChangeType { get; set; }

        // Открытое автореализуемое свойство типа int с номером элемента, который был добавлен или заменен
        // (в данном случае, это индекс нового элемента в списке)
        public int ElementNumber { get; set; }

        // Конструктор для инициализации класса
        public TeamListHandlerEventArgs(string collectionName, string changeType, int elementNumber)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ElementNumber = elementNumber;
        }

        // Перегруженная версия метода string ToString() для формирования строки с информацией обо всех полях класса
        public override string ToString()
        {
            return $"Коллекция: {CollectionName}, Тип изменения: {ChangeType}, Индекс элемента: {ElementNumber}";
        }
    }
}

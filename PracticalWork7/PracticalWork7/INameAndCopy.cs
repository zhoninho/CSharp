using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; // Не используется, можно убрать для .NET Core/5+ если не нужны Task-специфичные вещи

namespace PracticalWork7
{
    // Интерфейс, определяющий общее поведение для объектов,
    // которые имеют имя и могут быть глубоко скопированы.
    interface INameAndCopy
    {
        // Свойство Name (Имя)
        // Позволяет получать и устанавливать имя объекта.
        string Name { get; set; }

        // Метод DeepCopy (Глубокое копирование)
        // Должен возвращать новую, полностью независимую копию объекта.
        // Все вложенные ссылочные типы также должны быть скопированы.
        object DeepCopy();
    }
}

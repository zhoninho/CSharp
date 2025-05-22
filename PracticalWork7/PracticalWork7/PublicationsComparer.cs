// PublicationsComparer.cs
using System;
using System.Collections.Generic; // Для IComparer<T>
using System.Linq; // Не используется напрямую
using System.Text; // Не используется напрямую
using System.Threading.Tasks; // Не используется, можно убрать

namespace PracticalWork7
{
    // Класс PublicationsComparer реализует интерфейс IComparer<ResearchTeam>.
    // Предназначен для сравнения двух объектов ResearchTeam по количеству их публикаций.
    class PublicationsComparer : IComparer<ResearchTeam>
    {
        // Метод Compare сравнивает два объекта ResearchTeam (x и y).
        // Возвращает:
        // - отрицательное значение, если x меньше y.
        // - ноль, если x равно y.
        // - положительное значение, если x больше y.
        public int Compare(ResearchTeam x, ResearchTeam y)
        {
            // Обработка случаев, когда один или оба объекта равны null.
            if (x == null && y == null) return 0; // Если оба null, считаем их равными.
            if (x == null) return -1;             // Если x null, а y нет, x "меньше" y.
            if (y == null) return 1;              // Если y null, а x нет, x "больше" y.

            // Сравнение по количеству публикаций.
            // Используем метод CompareTo стандартного типа int.
            return x.Publications.Count.CompareTo(y.Publications.Count);
        }
    }
}
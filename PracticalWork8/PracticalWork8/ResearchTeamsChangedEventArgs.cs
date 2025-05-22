// ResearchTeamsChangedEventArgs.cs
using PracticalWork8;
using System;

namespace PracticalWork8
{
    /// <summary>
    /// Делегат для обработки события изменения в коллекции ResearchTeamCollection<TKey>.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа в коллекции.</typeparam>
    /// <param name="source">Источник события (объект ResearchTeamCollection<TKey>).</param>
    /// <param name="args">Аргументы события, содержащие информацию об изменении.</param>
    public delegate void ResearchTeamsChangedHandler<TKey>(object source, ResearchTeamsChangedEventArgs<TKey> args);

    /// <summary>
    /// Класс аргументов для события ResearchTeamsChanged.
    /// Содержит информацию об изменениях в коллекции ResearchTeamCollection<TKey> или ее элементах.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа в коллекции.</typeparam>
    public class ResearchTeamsChangedEventArgs<TKey> : EventArgs
    {
        /// <summary>
        /// Название коллекции, в которой произошло событие.
        /// </summary>
        public string CollectionName { get; }

        /// <summary>
        /// Тип изменения, вызвавший событие (удаление, замена, изменение свойства).
        /// </summary>
        public Revision RevisionType { get; }

        /// <summary>
        /// Название свойства класса ResearchTeam, которое является источником изменения данных элемента.
        /// Для событий, вызванных удалением или заменой элемента, значение этого свойства – пустая строка.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Номер регистрации объекта ResearchTeam для элемента, который был удален,
        /// заменен (в этом случае это номер старого, удаленного элемента) или данные которого были изменены.
        /// </summary>
        public int RegistrationNumber { get; }

        /// <summary>
        /// Конструктор для инициализации аргументов события.
        /// </summary>
        /// <param name="collectionName">Название коллекции.</param>
        /// <param name="revisionType">Тип изменения.</param>
        /// <param name="propertyName">Название измененного свойства (или пустая строка).</param>
        /// <param name="registrationNumber">Регистрационный номер затронутого ResearchTeam.</param>
        public ResearchTeamsChangedEventArgs(string collectionName, Revision revisionType, string propertyName, int registrationNumber)
        {
            CollectionName = collectionName;
            RevisionType = revisionType;
            PropertyName = propertyName ?? string.Empty; // Гарантируем, что не null
            RegistrationNumber = registrationNumber;
        }

        /// <summary>
        /// Возвращает строковое представление информации о событии.
        /// </summary>
        public override string ToString()
        {
            return $"Коллекция: '{CollectionName}', Тип изменения: {RevisionType}, " +
                   $"Свойство элемента: '{(string.IsNullOrEmpty(PropertyName) ? "N/A" : PropertyName)}', " +
                   $"Рег.Номер RT: {RegistrationNumber}";
        }
    }
}

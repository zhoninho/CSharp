// ResearchTeamCollectionGeneric.cs
using PracticalWork8;
using System;
using System.Collections.Generic;
using System.ComponentModel; // Для PropertyChangedEventHandler
using System.Linq;

namespace PracticalWork8
{
    /// <summary>
    /// Типизированная коллекция для хранения объектов ResearchTeam, использующая словарь.
    /// Уведомляет об изменениях в коллекции и в данных ее элементов с помощью событий.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа для словаря, используемого для хранения команд.</typeparam>
    public class ResearchTeamCollection<TKey> // Переименовал для соответствия заданию, если требуется один класс коллекции
    {
        // Внутренний словарь для хранения команд.
        private Dictionary<TKey, ResearchTeam> teamsDictionary;

        /// <summary>
        /// Открытое автореализуемое свойство с названием коллекции.
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Событие, которое происходит, когда изменяется набор элементов в коллекции-словаре
        /// или изменяются данные одного из ее элементов.
        /// </summary>
        public event ResearchTeamsChangedHandler<TKey> ResearchTeamsChanged;

        /// <summary>
        /// Конструктор коллекции.
        /// </summary>
        /// <param name="collectionName">Название коллекции.</param>
        public ResearchTeamCollection(string collectionName = "Новая Коллекция Команд")
        {
            CollectionName = collectionName;
            teamsDictionary = new Dictionary<TKey, ResearchTeam>();
        }

        /// <summary>
        /// Вспомогательный метод для вызова события ResearchTeamsChanged.
        /// </summary>
        protected virtual void OnResearchTeamsChanged(Revision revisionType, string propertyName, int registrationNumber)
        {
            ResearchTeamsChanged?.Invoke(this, new ResearchTeamsChangedEventArgs<TKey>(CollectionName, revisionType, propertyName, registrationNumber));
        }

        /// <summary>
        /// Обработчик события PropertyChanged, возникающего в элементах ResearchTeam.
        /// Преобразует это событие в событие ResearchTeamsChanged для всей коллекции.
        /// </summary>
        private void HandleResearchTeamPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ResearchTeam changedTeam = sender as ResearchTeam;
            if (changedTeam != null)
            {
                // Проверяем, что команда действительно все еще в нашей коллекции.
                // Это важно, если отписка может происходить с задержкой.
                // Здесь мы ищем команду по ссылке, чтобы убедиться.
                if (teamsDictionary.ContainsValue(changedTeam))
                {
                    OnResearchTeamsChanged(Revision.Property, e.PropertyName, changedTeam.RegistrationNumber);
                }
                else
                {
                    // Если команда уже не в коллекции (например, была удалена, а событие пришло позже),
                    // то отписываемся от нее на всякий случай, чтобы избежать утечек.
                    changedTeam.PropertyChanged -= HandleResearchTeamPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Индексатор для доступа к элементам ResearchTeam по ключу.
        /// Присвоение нового значения или замена существующего элемента управляют подписками
        /// на событие PropertyChanged элемента и вызывают событие ResearchTeamsChanged для коллекции.
        /// </summary>
        public ResearchTeam this[TKey key]
        {
            get
            {
                if (teamsDictionary.TryGetValue(key, out ResearchTeam team))
                {
                    return team;
                }
                // В предыдущей лабе здесь было исключение. Для удобства использования в Main,
                // можно возвращать null, если это допустимо по логике приложения.
                // Либо оставить исключение. Для задания оставим исключение.
                throw new KeyNotFoundException($"Элемент с ключом '{key}' не найден в коллекции '{CollectionName}'.");
            }
            set
            {
                if (value == null)
                {
                    // Если по ключу key был элемент, его нужно удалить и отписаться
                    if (teamsDictionary.TryGetValue(key, out ResearchTeam teamToRemove))
                    {
                        teamToRemove.PropertyChanged -= HandleResearchTeamPropertyChanged;
                        teamsDictionary.Remove(key);
                        OnResearchTeamsChanged(Revision.Remove, string.Empty, teamToRemove.RegistrationNumber);
                    }
                    // Если присваивается null ключу, которого не было, ничего не делаем или бросаем исключение.
                    // В данном случае, если value == null, мы эффективно удаляем элемент по ключу, если он был.
                    return;
                }


                if (teamsDictionary.TryGetValue(key, out ResearchTeam oldTeam))
                {
                    // Замена существующего элемента
                    if (ReferenceEquals(oldTeam, value)) return; // Замена на тот же самый объект

                    oldTeam.PropertyChanged -= HandleResearchTeamPropertyChanged;
                    teamsDictionary[key] = value;
                    value.PropertyChanged += HandleResearchTeamPropertyChanged;
                    OnResearchTeamsChanged(Revision.Replace, string.Empty, oldTeam.RegistrationNumber);
                }
                else
                {
                    // Добавление нового элемента
                    teamsDictionary.Add(key, value);
                    value.PropertyChanged += HandleResearchTeamPropertyChanged;
                    // Событие о добавлении (Add) не требуется по заданию №8.
                }
            }
        }

        /// <summary>
        /// Добавляет элемент в коллекцию. Управляет подпиской на PropertyChanged.
        /// </summary>
        public void Add(TKey key, ResearchTeam team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team), "Нельзя добавить null в коллекцию.");
            if (teamsDictionary.ContainsKey(key))
                throw new ArgumentException($"Элемент с ключом '{key}' уже существует в коллекции '{CollectionName}'.");

            teamsDictionary.Add(key, team);
            team.PropertyChanged += HandleResearchTeamPropertyChanged;
            // Событие добавления не требуется по заданию.
        }


        /// <summary>
        /// Удаляет элемент со значением rt из словаря.
        /// </summary>
        /// <param name="rt">Элемент ResearchTeam для удаления.</param>
        /// <returns>True, если элемент был найден и удален; иначе false.</returns>
        public bool Remove(ResearchTeam rt)
        {
            if (rt == null) return false;

            TKey keyToRemove = default;
            bool found = false;

            // Ищем ключ для объекта rt. Это может быть неэффективно для больших словарей.
            foreach (var pair in teamsDictionary)
            {
                if (ReferenceEquals(pair.Value, rt)) // Сравнение по ссылке
                {
                    keyToRemove = pair.Key;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                rt.PropertyChanged -= HandleResearchTeamPropertyChanged; // Отписываемся от событий элемента
                teamsDictionary.Remove(keyToRemove);                     // Удаляем из словаря
                OnResearchTeamsChanged(Revision.Remove, string.Empty, rt.RegistrationNumber); // Вызываем событие
                return true;
            }
            return false;
        }

        /// <summary>
        /// Заменяет в словаре элемент со значением rtold на элемент rtnew.
        /// </summary>
        /// <param name="rtold">Элемент ResearchTeam, который нужно заменить.</param>
        /// <param name="rtnew">Новый элемент ResearchTeam.</param>
        /// <returns>True, если замена прошла успешно; иначе false.</returns>
        public bool Replace(ResearchTeam rtold, ResearchTeam rtnew)
        {
            if (rtold == null || rtnew == null) return false;
            if (ReferenceEquals(rtold, rtnew)) return false; // Замена на тот же самый объект

            TKey keyOfOld = default;
            bool found = false;

            foreach (var pair in teamsDictionary)
            {
                if (ReferenceEquals(pair.Value, rtold))
                {
                    keyOfOld = pair.Key;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                rtold.PropertyChanged -= HandleResearchTeamPropertyChanged; // Отписываемся от старого
                teamsDictionary[keyOfOld] = rtnew;                          // Заменяем значение по найденному ключу
                rtnew.PropertyChanged += HandleResearchTeamPropertyChanged;   // Подписываемся на новый
                OnResearchTeamsChanged(Revision.Replace, string.Empty, rtold.RegistrationNumber); // Уведомляем о замене rtold
                return true;
            }
            return false;
        }

        // Для удобства демонстрации в Main, добавим методы из предыдущей версии,
        // адаптировав их для работы с ключами (предполагаем TKey=string, ключ=RegistrationNumber.ToString())
        public void AddDefaults()
        {
            if (typeof(TKey) != typeof(string))
            {
                Console.WriteLine("Предупреждение: AddDefaults в этой реализации ResearchTeamCollection<TKey> " +
                                  "оптимизирован для TKey=string (RegistrationNumber.ToString()). Для других TKey может не работать корректно.");
                // Можно либо выбросить исключение, либо просто не выполнять добавление
                // throw new InvalidOperationException("AddDefaults supports TKey=string only for this example.");
            }

            var defaults = new List<ResearchTeam>
            {
                new ResearchTeam("Исследование ИИ по умолчанию", "Тех. Университет", 12301, TimeFrame.TwoYears),
                new ResearchTeam("Разработка МО по умолчанию", "Научный Институт", 45601, TimeFrame.Year),
                new ResearchTeam("Наука о данных по умолчанию", "Исследовательский Центр", 78901, TimeFrame.Long)
            };

            foreach (var team in defaults)
            {
                try
                {
                    // Попытка преобразовать RegistrationNumber в TKey. Это будет работать для TKey=string.
                    TKey key = (TKey)Convert.ChangeType(team.RegistrationNumber.ToString(), typeof(TKey));
                    if (!teamsDictionary.ContainsKey(key)) // Проверка на случай повторного вызова
                    {
                        this.Add(key, team); // Используем метод Add для подписки
                    }
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine($"Ошибка в AddDefaults: не удалось преобразовать ключ для команды с рег.номером {team.RegistrationNumber}. Пропущено.");
                }
            }
        }

        /// <summary>
        /// Возвращает количество элементов в коллекции.
        /// </summary>
        public int Count => teamsDictionary.Count;

        /// <summary>
        /// Возвращает строковое представление коллекции.
        /// </summary>
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Коллекция: '{CollectionName}', Количество элементов: {teamsDictionary.Count}");
            if (teamsDictionary.Count == 0)
            {
                sb.AppendLine("  Коллекция пуста.");
            }
            else
            {
                foreach (var kvp in teamsDictionary)
                {
                    sb.AppendLine($"  Ключ: {kvp.Key} => {kvp.Value.ToShortString()}");
                }
            }
            return sb.ToString();
        }
    }
}

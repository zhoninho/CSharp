// ResearchTeam.cs
using PracticalWork9;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO; // Для работы с файлами
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json; // Для JSON сериализации
// using System.Text.Json.Serialization; // Для атрибутов типа [JsonConstructor] если нужны

namespace PracticalWork9
{
    public enum TimeFrame
    {
        Year,
        TwoYears,
        Long
    }
    public class ResearchTeam : Team, INameAndCopy, IEnumerable, IComparer<ResearchTeam>, INotifyPropertyChanged
    {
        private string _researchTopicField;
        private TimeFrame _durationField;
        private List<Person> _membersField; // Переименовано для избежания конфликта с public свойством
        private List<Paper> _publicationsField;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Конструктор по умолчанию ВАЖЕН для System.Text.Json
        public ResearchTeam() : base()
        {
            _researchTopicField = "Неопределенная тема";
            _durationField = TimeFrame.Year;
            _membersField = new List<Person>();
            _publicationsField = new List<Paper>();
        }

        public ResearchTeam(string topic, string org, int regNumber, TimeFrame duration)
            : base(org, regNumber) // Используем свойства базового класса, которые public
        {
            this.ResearchTopic = topic; // Используем свойства текущего класса
            this.Duration = duration;
            _membersField = new List<Person>();
            _publicationsField = new List<Paper>();
        }

        // Публичные свойства для сериализации
        public string ResearchTopic
        {
            get { return _researchTopicField; }
            set
            {
                if (_researchTopicField != value)
                {
                    _researchTopicField = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeFrame Duration
        {
            get { return _durationField; }
            set
            {
                if (_durationField != value)
                {
                    _durationField = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Person> Members // Свойство для сериализации списка участников
        {
            get { return _membersField; }
            set { _membersField = value ?? new List<Person>(); OnPropertyChanged(); /* Если нужно уведомление */  }
        }

        public List<Paper> Publications // Свойство для сериализации списка публикаций
        {
            get { return _publicationsField; }
            set { _publicationsField = value ?? new List<Paper>(); OnPropertyChanged(); /* Если нужно уведомление */ }
        }

        // TeamData остается как было, если оно не для сериализации напрямую, а для логики
        // Если нужно сериализовать данные Team как вложенный объект, то лучше иметь
        // свойства Organization и RegistrationNumber на этом уровне или на уровне Team
        // (что уже сделано через наследование)
        [System.Text.Json.Serialization.JsonIgnore] // Игнорируем это свойство при сериализации, т.к. есть базовые
        public Team TeamData
        {
            get { return new Team(base.Organization, base.RegistrationNumber); } // Используем свойства базового класса
            set
            {
                if (value != null)
                {
                    base.Organization = value.Organization;
                    base.RegistrationNumber = value.RegistrationNumber;
                }
            }
        }


        // --- Реализация INameAndCopy ---
        string INameAndCopy.Name
        {
            get { return ResearchTopic; }
            set { ResearchTopic = value; }
        }

        object INameAndCopy.DeepCopy()
        {
            return this.DeepCopy(); // Вызываем типизированный метод
        }

        // --- Задание 1: Новые методы ---

        /// <summary>
        /// Создает полную (глубокую) копию объекта ResearchTeam.
        /// </summary>
        public ResearchTeam DeepCopy() // Типизированный метод
        {
            ResearchTeam copy = new ResearchTeam(this.ResearchTopic, this.Organization, this.RegistrationNumber, this.Duration);

            if (this.Members != null)
            {
                copy.Members = new List<Person>();
                foreach (Person member in this.Members)
                {
                    copy.Members.Add(member != null ? (Person)member.DeepCopy() : null);
                }
            }

            if (this.Publications != null)
            {
                copy.Publications = new List<Paper>();
                foreach (Paper paper in this.Publications)
                {
                    copy.Publications.Add(paper != null ? (Paper)paper.DeepCopy() : null);
                }
            }
            return copy;
        }

        // Переопределение виртуального метода DeepCopy из базового класса Team (если он был virtual object)
        // Если в Team был `public virtual object DeepCopy()`, то здесь `public override object DeepCopy()`
        //public override object DeepCopy() // Переопределяем базовый, чтобы он вызывал новый типизированный
        //{
        //    return this.DeepCopy(); // Вызов типизированного DeepCopy() текущего класса
        //}


        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true, // Для читаемого JSON файла
                // ReferenceHandler = ReferenceHandler.Preserve // Для обработки циклических ссылок, если они есть
                // Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Для кириллицы без экранирования
            };
        }

        /// <summary>
        /// Сохраняет данные текущего объекта ResearchTeam в файл.
        /// </summary>
        public bool Save(string filename)
        {
            FileStream fs = null;
            try
            {
                // FileMode.Create создаст файл, если его нет, или перезапишет, если есть.
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                JsonSerializer.SerializeAsync(fs, this, GetJsonSerializerOptions()).Wait(); // Синхронное ожидание для простоты
                // JsonSerializer.Serialize(fs, this, GetJsonSerializerOptions()); // для .NET 5+ можно использовать синхронный Serialize
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении объекта в файл '{filename}': {ex.Message}");
                return false;
            }
            finally
            {
                fs?.Close();
            }
        }

        /// <summary>
        /// Инициализирует текущий объект ResearchTeam данными из файла.
        /// </summary>
        public bool Load(string filename)
        {
            FileStream fs = null;
            ResearchTeam tempTeam = null;
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine($"Файл '{filename}' не найден для загрузки.");
                    return false;
                }

                fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                // tempTeam = JsonSerializer.DeserializeAsync<ResearchTeam>(fs, GetJsonSerializerOptions()).Result; // синхронное ожидание
                tempTeam = JsonSerializer.Deserialize<ResearchTeam>(fs, GetJsonSerializerOptions()); // .NET 5+

                if (tempTeam != null)
                {
                    // Копируем данные из загруженного объекта в текущий
                    this.ResearchTopic = tempTeam.ResearchTopic;
                    this.Organization = tempTeam.Organization; // из базового класса
                    this.RegistrationNumber = tempTeam.RegistrationNumber; // из базового класса
                    this.Duration = tempTeam.Duration;
                    this.Members = tempTeam.Members; // Глубокое копирование списков не обязательно, если tempTeam больше не нужен
                    this.Publications = tempTeam.Publications;
                    return true;
                }
                return false;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Ошибка JSON при загрузке объекта из файла '{filename}': {jsonEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Общая ошибка при загрузке объекта из файла '{filename}': {ex.Message}");
                return false;
            }
            finally
            {
                fs?.Close();
            }
        }

        /// <summary>
        /// Добавляет новую публикацию (Paper) в список Publications, данные для которой вводятся с консоли.
        /// </summary>
        public bool AddFromConsole()
        {
            Console.WriteLine("\n--- Добавление новой публикации ---");
            Console.WriteLine("Введите данные публикации в формате: ");
            Console.WriteLine("Название статьи; Имя автора; Фамилия автора; ГГГГ-ММ-ДД (дата рождения автора); ГГГГ-ММ-ДД (дата публикации)");
            Console.WriteLine("Разделитель - точка с запятой ';'. Пример:");
            Console.WriteLine("Новое открытие в науке;Иван;Петров;1980-05-15;2024-01-20");
            Console.Write("Ввод: ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ошибка: Ввод не может быть пустым.");
                return false;
            }

            string[] parts = input.Split(';');
            if (parts.Length != 5)
            {
                Console.WriteLine("Ошибка: Неверное количество полей. Ожидалось 5 полей, разделенных ';'.");
                return false;
            }

            try
            {
                string title = parts[0].Trim();
                string authorFirstName = parts[1].Trim();
                string authorLastName = parts[2].Trim();

                if (string.IsNullOrWhiteSpace(title) ||
                    string.IsNullOrWhiteSpace(authorFirstName) ||
                    string.IsNullOrWhiteSpace(authorLastName))
                {
                    Console.WriteLine("Ошибка: Название статьи и имя/фамилия автора не могут быть пустыми.");
                    return false;
                }

                DateTime authorBirthDate = DateTime.Parse(parts[3].Trim()); // Может бросить FormatException
                DateTime publicationDate = DateTime.Parse(parts[4].Trim()); // Может бросить FormatException

                Person author = new Person(authorFirstName, authorLastName, authorBirthDate);
                Paper newPaper = new Paper(title, author, publicationDate);

                if (this.Publications == null)
                {
                    this.Publications = new List<Paper>();
                }
                this.Publications.Add(newPaper);
                Console.WriteLine("Публикация успешно добавлена.");
                return true;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Ошибка формата данных: {ex.Message}. Пожалуйста, проверьте формат дат (ГГГГ-ММ-ДД).");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при обработке ввода: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Статический метод для сохранения объекта ResearchTeam в файл.
        /// </summary>
        public static bool Save(string filename, ResearchTeam obj)
        {
            if (obj == null)
            {
                Console.WriteLine("Ошибка (static Save): Объект для сохранения не может быть null.");
                return false;
            }
            // Просто вызываем экземплярный метод Save
            return obj.Save(filename);
        }

        /// <summary>
        /// Статический метод для инициализации объекта ResearchTeam данными из файла.
        /// </summary>
        public static bool Load(string filename, ResearchTeam obj)
        {
            if (obj == null)
            {
                Console.WriteLine("Ошибка (static Load): Объект для загрузки данных не может быть null.");
                return false;
            }
            // Просто вызываем экземплярный метод Load
            // Важно: экземплярный Load изменяет состояние `this`.
            // Здесь мы передаем `obj`, и его состояние будет изменено.
            return obj.Load(filename);
        }

        // --- Остальные методы и свойства класса ResearchTeam (из предыдущих работ) ---
        [System.Text.Json.Serialization.JsonIgnore] // Игнорируем для сериализации, т.к. есть список Publications
        public Paper LatestPublication
        {
            get
            {
                if (Publications == null || Publications.Count == 0) return null;
                return Publications.OrderByDescending(p => p.PublicationDate).FirstOrDefault();
            }
        }

        public void AddMembers(params Person[] newMembers)
        {
            if (newMembers != null)
            {
                if (Members == null) Members = new List<Person>();
                Members.AddRange(newMembers.Where(m => m != null));
            }
        }

        public void AddPapers(params Paper[] newPapers)
        {
            if (newPapers != null)
            {
                if (Publications == null) Publications = new List<Paper>();
                Publications.AddRange(newPapers.Where(p => p != null));
            }
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Тема: {ResearchTopic}");
            sb.AppendLine(base.ToString()); // Информация из Team (Organization, RegistrationNumber)
            sb.AppendLine($"Длительность: {Duration}");

            sb.AppendLine("Участники:");
            if (Members != null && Members.Count > 0)
                foreach (Person member in Members) sb.AppendLine($"  - {member?.ToString() ?? "null member"}");
            else sb.AppendLine("  Нет участников.");

            sb.AppendLine("Публикации:");
            if (Publications != null && Publications.Count > 0)
                foreach (Paper paper in Publications) sb.AppendLine($"  - {paper?.ToString() ?? "null paper"}");
            else sb.AppendLine("  Нет публикаций.");

            return sb.ToString();
        }

        public string ToShortString()
        {
            int memberCount = (Members != null) ? Members.Count : 0;
            int publicationCount = (Publications != null) ? Publications.Count : 0;
            // base.ToString() уже выводит Org и RegNumber
            return $"Тема: {ResearchTopic}, {base.ToString()}, Длительность: {Duration}, Участников: {memberCount}, Публикаций: {publicationCount}";
        }

        public IEnumerator GetEnumerator()
        {
            return new ResearchTeamEnumerator(this); // Предполагается, что ResearchTeamEnumerator адаптирован или работает с public свойством Members
        }
        // ... другие итераторы GetMembersWithoutPublications, GetPublicationsLastYears и т.д. ...
        // Они должны использовать публичные свойства Members и Publications
        public IEnumerable GetMembersWithoutPublications()
        {
            if (Members == null) yield break;
            foreach (Person member in Members)
            {
                bool hasPublication = Publications != null && Publications.Any(paper => paper.Author != null && paper.Author.Equals(member));
                if (!hasPublication) yield return member;
            }
        }

        public IEnumerable GetPublicationsLastYears(int years)
        {
            if (Publications == null) yield break;
            DateTime thresholdDate = DateTime.Now.AddYears(-years);
            foreach (Paper paper in Publications)
            {
                if (paper.PublicationDate >= thresholdDate) yield return paper;
            }
        }

        public int Compare(ResearchTeam x, ResearchTeam y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return string.Compare(x.ResearchTopic, y.ResearchTopic, StringComparison.OrdinalIgnoreCase);
        }
    }
}
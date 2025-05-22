// ResearchTeamEnumerator.cs
using PracticalWork9;
using System;
using System.Collections;        // Для IEnumerator
using System.Collections.Generic; // Для List<T> (неявно, через ResearchTeam)
using System.Linq;              // Не используется напрямую
using System.Text;              // Не используется напрямую
using System.Threading.Tasks;   // Не используется, можно убрать

namespace PracticalWork9
{
    // Пользовательский нумератор для класса ResearchTeam.
    // Этот нумератор перебирает участников команды (Person), у которых есть 
    // хотя бы одна публикация в списке публикаций этой команды.
    class ResearchTeamEnumerator : IEnumerator
    {
        // Ссылка на объект ResearchTeam, по которому будет происходить итерация.
        private ResearchTeam team;
        // Текущий индекс в списке участников (team.Members).
        private int currentIndex;
        // Текущий участник, который будет возвращен свойством Current.
        // Хранит участника, соответствующего условию (имеет публикации).
        private Person currentMember;

        // Конструктор, принимающий объект ResearchTeam.
        public ResearchTeamEnumerator(ResearchTeam team)
        {
            this.team = team;
            // Инициализация: currentIndex устанавливается в -1, что означает "перед первым элементом".
            currentIndex = -1;
            currentMember = null; // Текущий элемент (участник) еще не определен.
        }

        // Свойство Current из интерфейса IEnumerator.
        // Возвращает текущий элемент коллекции (участника с публикациями).
        public object Current
        {
            get
            {
                // Если currentMember не null (т.е. MoveNext успешно нашел элемент), возвращаем его.
                // Стандартное поведение - бросать InvalidOperationException, если Current вызывается 
                // до первого вызова MoveNext() или после того, как MoveNext() вернул false.
                // Однако, в данной реализации, если currentMember равен null (например, до первого MoveNext,
                // или если подходящих элементов нет), будет возвращен null.
                // Для строгого соответствия можно добавить проверку:
                // if (currentIndex == -1 || currentMember == null)
                //    throw new InvalidOperationException("Перечисление не было начато или уже завершено, или подходящих элементов нет.");
                return currentMember;
            }
        }

        // Метод MoveNext() из интерфейса IEnumerator.
        // Перемещает нумератор к следующему элементу коллекции (участнику с публикациями).
        // Возвращает true, если переход успешен, и false, если достигнут конец коллекции.
        public bool MoveNext()
        {
            // Проверяем, что списки участников и публикаций существуют.
            if (team.Members == null || team.Publications == null)
            {
                currentMember = null;
                return false; // Невозможно продолжить итерацию.
            }

            // Продолжаем поиск со следующего участника (увеличиваем currentIndex).
            while (++currentIndex < team.Members.Count)
            {
                Person member = team.Members[currentIndex]; // Получаем следующего участника.

                // Проверяем, есть ли у этого участника публикации в данной команде.
                // Используем LINQ Any() для эффективности: ищем хотя бы одну публикацию.
                if (team.Publications.Any(paper => paper.Author.Equals(member)))
                {
                    // Участник найден, он имеет публикацию.
                    currentMember = member; // Устанавливаем его как текущий.
                    return true; // Успешно перешли к следующему элементу.
                }
            }
            // Если цикл завершился, значит, больше нет участников, удовлетворяющих условию.
            currentMember = null; // Сбрасываем текущий элемент.
            return false; // Больше подходящих элементов нет.
        }

        // Метод Reset() из интерфейса IEnumerator.
        // Сбрасывает нумератор в исходное состояние (перед первым элементом).
        public void Reset()
        {
            currentIndex = -1;    // Устанавливаем индекс перед первым элементом.
            currentMember = null; // Текущий элемент не определен.
        }
    }
}
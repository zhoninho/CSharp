using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork8
{
    /// <summary>
    /// Определяет тип изменения, произошедшего в коллекции ResearchTeamCollection<TKey>
    /// или в одном из ее элементов.
    /// </summary>
    public enum Revision
    {
        /// <summary>
        /// Элемент был удален из коллекции.
        /// </summary>
        Remove,

        /// <summary>
        /// Элемент в коллекции был заменен на другой.
        /// </summary>
        Replace,

        /// <summary>
        /// Данные (свойство) одного из элементов коллекции были изменены.
        /// </summary>
        Property
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using szosztar.Models;

namespace szosztar.Logic.Interfaces
{
    /// <summary>
    ///     Defines methods for <see cref="WordLogic"/>
    /// </summary>
    public interface IWordLogic
    {
        /// <summary>
        ///     Sorts words
        /// </summary>
        /// <returns>A list of words</returns>
        Task<IList<Word>> ProcessWords();
    }
}

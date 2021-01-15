using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using szosztar.Models;

namespace szosztar.Data.Interfaces
{
    /// <summary>
    ///     Defines methods for <see cref="Data"/>
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        ///     Gets words
        /// </summary>
        /// <returns>A list of words</returns>
        Task<IList<Word>> GetWords();

        /// <summary>
        ///     Posts a word
        /// </summary>
        /// <returns>Success boolean</returns>
        Task<bool> PostWord(Word word);
    }
}

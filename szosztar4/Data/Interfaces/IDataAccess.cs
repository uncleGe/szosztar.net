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
        ///     Gets categories
        /// </summary>
        /// <returns>A list of categories</returns>
        Task<IList<string>> GetCategories();

        /// <summary>
        ///     Gets Categories and IDs
        /// </summary>
        /// <returns>A dict of category data</returns>
        Task<IDictionary<string, int?>> GetAndMapCategories();

        /// <summary>
        ///     Posts a category
        /// </summary>
        /// <returns>Success boolean</returns>
        Task<bool> PostCategory(string category);

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

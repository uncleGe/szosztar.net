using FirebaseAdmin.Auth;
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
        Task<IList<string>> GetCategories(string userId);

        /// <summary>
        ///     Get the <see cref="User"/>
        /// </summary>
        /// <param name="externalId"></param>
        /// <returns><see cref="User"/></returns>
        Task<User> GetUser(string externalId);

        /// <summary>
        ///     Post the new user
        /// </summary>
        /// <param name="user">The <see cref="User"/></param>
        /// <returns>Success bool</returns>
        Task<bool> PostUser(User user);

        /// <summary>
        ///     Gets Categories and IDs
        /// </summary>
        /// <returns>A dict of category data</returns>
        Task<IDictionary<string, int?>> GetAndMapCategories(string userId);

        /// <summary>
        ///     Posts a category
        /// </summary>
        /// <returns>Success boolean</returns>
        Task<bool> PostCategory(string userId, string category);

        /// <summary>
        ///     Gets words
        /// </summary>
        /// <returns>A list of words</returns>
        Task<IList<Word>> GetWords(string userId);

        /// <summary>
        ///     Posts a word
        /// </summary>
        /// <returns>Success boolean</returns>
        Task<bool> PostWord(string userId, Word word);
    }
}

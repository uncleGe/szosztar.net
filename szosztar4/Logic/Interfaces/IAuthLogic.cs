using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace szosztar.Logic.Interfaces
{
    /// <summary>
    ///     Defines methods for <see cref="AuthLogic"/>
    /// </summary>
    public interface IAuthLogic
    {
        /// <summary>
        ///     Authenticates with Firbase
        /// </summary>
        /// <returns>A list of categories</returns>
        Task<string> FirebaseAuthenticate(string authToken);
    }
}

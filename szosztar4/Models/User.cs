using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace szosztar.Models
{
    public class User
    {
        /// <summary>
        ///     The user's ID
        /// </summary>
        public int? userId { get; set; }

        /// <summary>
        ///     The user's Firebase ID
        /// </summary>
        public string externalId { get; set; }

        /// <summary>
        ///     The user's username/email address
        /// </summary>
        public string username { get; set; }
    }
}

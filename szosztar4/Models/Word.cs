using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace szosztar.Models
{
    [DataContract]
    public class Word
    {
        /// <summary>
        ///     The id
        /// </summary>
        [DataMember(Name = "id")]
        public int id { get; set; }

        /// <summary>
        ///     The english word
        /// </summary>
        [DataMember(Name = "english")]
        public string english { get; set; }

        /// <summary>
        ///     The hungarian word
        /// </summary>
        [DataMember(Name = "hungarian")]
        public string hungarian { get; set; }

        /// <summary>
        ///     The category
        /// </summary>
        [DataMember(Name = "category")]
        public Category? category { get; set; }

        /// <summary>
        ///     The notes
        /// </summary>
        [DataMember(Name = "notes")]
        public string? notes { get; set; }

    }
}

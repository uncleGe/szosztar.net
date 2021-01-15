using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace szosztar.Models
{
    [DataContract]
    public class WeatherInfo
    {
        /// <summary>
        ///     The actual location
        /// </summary>
        [DataMember(Name = "location")]
        public string Location { get; set; }

        /// <summary>
        ///     Is it hot?
        /// </summary>
        [DataMember(Name = "degree")]
        public float Degree { get; set; }

        /// <summary>
        ///     What it dit is?
        /// </summary
        [DataMember(Name = "dateTime")]
        public DateTime DateTime { get; set; }
    }
}

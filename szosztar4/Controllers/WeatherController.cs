using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using szosztar.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace szosztar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        // GET: api/<WeatherController>
        [HttpGet]
        public IEnumerable<WeatherInfo> Get()
        {
            var weatherInfoList = new List<WeatherInfo>();
            for (int i = 0; i < 10; i++)
            {
                weatherInfoList.Add(new WeatherInfo
                {
                    Location = $"Location {i}",
                    Degree = i * 23 / 17,
                    DateTime = DateTime.Now.ToUniversalTime()
                }); ;
            }
            //return new string[] { "value1", "value2" };
            return weatherInfoList;
        }

        // GET api/<WeatherController>/5
        [HttpGet("{id}")]
        public List<WeatherInfo> Get(int id)
        {
            //return "value";
            var weatherInfoList = new List<WeatherInfo>();
            for (int i = 0; i < 10; i++)
            {
                weatherInfoList.Add(new WeatherInfo
                {
                    Location = $"Location {id}",
                    Degree = i * 23 / 17,
                    DateTime = DateTime.Now.ToUniversalTime()
                }); ;
            }
            //return new string[] { "value1", "value2" };
            return weatherInfoList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using szosztar.Data.Interfaces;
using szosztar.Logic.Interfaces;
using szosztar.Models;

namespace szosztar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IWordLogic logic;
        private readonly IAuthLogic authLogic;
        private readonly IDataAccess dataAccess;
        public WordController(IWordLogic logic, IAuthLogic authLogic, IDataAccess dataAccess)
        {
            this.logic = logic;
            this.authLogic = authLogic;
            this.dataAccess = dataAccess;
        }

        /// <summary>
        ///  Get words
        /// </summary>
        /// <returns>Some <see cref="Word"/>s</returns>
        // GET: api/<WordController>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string auth)
        {
            var externalId = await authLogic.FirebaseAuthenticate(auth);

            if (String.IsNullOrEmpty(externalId))
            {
                return Unauthorized();
            }

            var words = await logic.ProcessWords(externalId);


            if (words == null)
            {
                Console.WriteLine("word get 6");

                return NotFound();
            }
            Console.WriteLine("word get 7");
            return Ok(words);
        }

        // GET api/<WordController>/5
        [HttpGet("{id}")]
        public IList<Word> Get(int id)
        {
            //return "value";
            return new List<Word>
            {
                new Word
                {
                    id = id+1,
                    english = "First1",
                    hungarian = "Second1",
                },
                new Word
                {
                    id = id+2,
                    english = "First2",
                    hungarian = "Second2",
                },
                new Word
                {
                    id = id+3,
                    english = "First3",
                    hungarian = "Second3",
                }
            };
        }

        // POST api/<WordController>
        /// <summary>
        ///  Post a word
        /// </summary>
        /// <returns>Success bool </returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Word word, [FromQuery] string auth)
        {
            var externalId = await authLogic.FirebaseAuthenticate(auth);

            if (String.IsNullOrEmpty(externalId))
            {
                return Unauthorized();
            }

            if (word == null)
            {
                return BadRequest();
            }

            var result = await dataAccess.PostWord(externalId, word);

            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }

        // PUT api/<WordController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WordController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

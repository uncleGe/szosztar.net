using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using szosztar.Data.Interfaces;

namespace szosztar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        //private readonly IWordLogic logic;
        private readonly IDataAccess dataAccess;
        public CategoryController(
            //IWordLogic logic,
            IDataAccess dataAccess
            )
        {
            //this.logic = logic;
            this.dataAccess = dataAccess;
        }

        /// <summary>
        ///     Get categories
        /// </summary>
        /// <returns>Categories</returns>
        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await dataAccess.GetCategories();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        ///  Post a category
        /// </summary>
        /// <returns>Success bool</returns>
        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            var result = await dataAccess.PostCategory(category);

            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

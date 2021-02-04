using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using szosztar.Data.Interfaces;
using szosztar.Logic.Interfaces;

namespace szosztar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        //private readonly IWordLogic logic;
        private readonly IDataAccess dataAccess;
        private readonly IAuthLogic authLogic;

        public CategoryController(
            //IWordLogic logic,
            IDataAccess dataAccess,
            IAuthLogic authLogic
            )
        {
            //this.logic = logic;
            this.dataAccess = dataAccess;
            this.authLogic = authLogic;
        }

        /// <summary>
        ///     Get categories
        /// </summary>
        /// <returns>Categories</returns>
        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string auth)
        {
            var externalId = await authLogic.FirebaseAuthenticate(auth);

            if (String.IsNullOrEmpty(externalId))
            {
                return Unauthorized();
            }

            var result = await dataAccess.GetCategories(externalId);

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
        public async Task<IActionResult> Post([FromBody] string category, [FromQuery] string auth)
        {
            var externalId = await authLogic.FirebaseAuthenticate(auth);

            if (String.IsNullOrEmpty(externalId))
            {
                return Unauthorized();
            }

            if (category == null)
            {
                return BadRequest();
            }

            var result = await dataAccess.PostCategory(externalId, category);

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

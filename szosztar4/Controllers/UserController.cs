using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using szosztar.Data.Interfaces;
using szosztar.Logic.Interfaces;
using szosztar.Models;

namespace szosztar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthLogic authLogic;
        private readonly IDataAccess dataAccess;
        public UserController(IWordLogic logic, IAuthLogic authLogic, IDataAccess dataAccess)
        {
            this.authLogic = authLogic;
            this.dataAccess = dataAccess;
        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user, [FromQuery] string auth)
        {
            var externalId = await authLogic.FirebaseAuthenticate(auth);

            if (String.IsNullOrEmpty(externalId))
            {
                return Unauthorized();
            }

            if (user == null)
            {
                return BadRequest();
            }

            var result = await dataAccess.PostUser(user);

            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

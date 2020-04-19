using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BetPlanetTest.Data;
using BetPlanetTest.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BetPlanetTest.Controllers
{
    // Controller version 1
    [Route("secretapi/1/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IDatabaseDispatcher dispatcher;

        public TestController(IDatabaseDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [Route("[action]/{id}")]
        [HttpGet]
        public string UsersGet(string id)
        {
            int numId;
            bool success = Int32.TryParse(id, out numId);
            if (!success)
            {
                return "Ошибка: параметр Id не является целым числом";
            }

            Users user = dispatcher.GetUserById(numId);

            if(user == null)
            {
                return "User с Id = " + id + " не найден.";
            }

            return JsonConvert.SerializeObject(user);
        }






        [Route("[action]")]
        [HttpGet]
        public IEnumerable<string> UsersGet()
        {
            return new List<string> { "User1", "User2", "User3" };
        }

        // GET: api/Postgres
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
                
        // GET: api/Postgres/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Postgres
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Postgres/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

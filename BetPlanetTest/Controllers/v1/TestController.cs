using System;
using System.Collections.Generic;
using System.Linq;
using BetPlanetTest.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetPlanetTest.Controllers
{
    // Controller version 1
    [Route("secretapi/1/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IDatabaseDispatcher dispatcher;
        private HttpContext httpContext;

        public TestController(IDatabaseDispatcher dispatcher, IHttpContextAccessor httpContextAccessor)
        {
            this.dispatcher = dispatcher;
            httpContext = httpContextAccessor.HttpContext;
            httpContext.Response.ContentType = "application/json";
        }

        /// <summary>
        /// GET: {endpoint}/1/test/usersget/1
        /// Возвращает запись Users по ID в JSON формате:
        /// {"id":5,"name":"User Name","email":"email@email.com"}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200:Users/400/404</returns>
        [Route("[action]/{id}")]
        [HttpGet]
        public ActionResult<Users> UsersGet(string id)
        {
            int numId;

            if (!Int32.TryParse(id, out numId))
            {
                return BadRequest(); // 400
            }

            Users user = dispatcher.GetUserById(numId);

            if (user == null)
            {
                return NotFound(); // 404
            }

            return Ok(user);
        }

        /// <summary>
        /// GET: {endpoint}/1/test/usersget
        /// Возвращает список всех записей Users в JSON формате:
        /// [{"id":1,"name":"User Name1","email":"email1@email.com"},
        ///  {"id":2,"name":"User Name2","email":"email2@email.com"},
        ///  {"id":3,"name":"User Name3","email":"email3@email.com"}]
        /// </summary>
        /// <returns>200:List<Users>/204</returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<Users>> UsersGet()
        {
            List<Users> users = dispatcher.GetUsers().ToList();

            if (users == null || users.Count == 0)
            {
                return NoContent(); // 204
            }

            //List<Users> users = new List<Users>() 
            //{
            //    new Users(){ Id = 1, Name = "User Name1", Email = "email1@email.com" },
            //    new Users(){ Id = 2, Name = "User Name2", Email = "email2@email.com" },
            //    new Users(){ Id = 3, Name = "User Name3", Email = "email3@email.com" }
            //};

            return Ok(users);
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

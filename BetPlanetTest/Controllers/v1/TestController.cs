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
        /// GET: {endpoint}/1/test/usersnameget/name
        /// Возвращает запись Users по name в JSON формате:
        /// {"id":5,"name":"User Name","email":"email@email.com"}
        /// </summary>
        /// <param name="name"></param>
        /// <returns>200:Users/400/404</returns>
        [Route("[action]/{name}")]
        [HttpGet]
        public ActionResult<Users> UsersNameGet(string name)
        {
            
            Users user = dispatcher.GetUserByName(name);

            if (user == null)
            {
                return NotFound(); // 404
            }

            return Ok(user);
        }

        /// <summary>
        /// GET: {endpoint}/1/test/usersemailget/name
        /// Возвращает запись Users по email в JSON формате:
        /// {"id":5,"name":"User Name","email":"email@email.com"}
        /// </summary>
        /// <param name="email"></param>
        /// <returns>200:Users/400/404</returns>
        [Route("[action]/{email}")]
        [HttpGet]
        public ActionResult<Users> UsersEmailGet(string email)
        {

            Users user = dispatcher.GetUserByEmail(email);

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

            return Ok(users);
        }

        // PUT: 
        [Route("[action]")]
        [HttpPut]
        public Users UsersUpd([FromBody] Users user)
        {
            return user;
        }

        // POST: 
        [Route("[action]")]
        [HttpPost]
        public Users UsersIns([FromBody] Users user)
        {
            return user;
        }

        // DELETE: 
        [Route("[action]/{id}")]
        [HttpDelete]
        public ActionResult UsersDel(int id)
        {
            return Ok(id);
        }
    }
}

﻿using System.Collections.Generic;
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

        #region --------------------------------- Users -------------------------------------------

        /// <summary>
        /// GET: {endpoint}/1/test/usersget/1
        /// Возвращает запись Users по ID в JSON формате:
        /// {"id":5,"name":"User Name","email":"email@email.com"}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200:Users/404</returns>
        [Route("[action]/{id}")]
        [HttpGet]
        public ActionResult<Users> UsersGet(int id)
        {            
            Users user = dispatcher.GetUserById(id);

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
        /// GET: {endpoint}/1/test/usersemailget/email
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
            return Ok(dispatcher.GetUsers().ToList());
        }

        /// <summary>
        ///  PUT: {endpoint}/1/test/usersupd
        ///  body content: JSON Users
        /// </summary>
        /// <param name="user"></param>
        /// <returns>200:Users.Id/500</returns>        
        [Route("[action]")]
        [HttpPut]
        public ActionResult UsersUpd([FromBody] Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            if (dispatcher.GetUserById(user.Id) == null)
            {
                return NotFound(user.Id);
            }

            if (!dispatcher.UpdateUser(user))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(user.Id);
        }

        /// <summary>
        /// POST: {endpoint}/1/test/usersins
        /// body content: JSON Users
        /// </summary>
        /// <param name="user"></param>
        /// <returns>200:Users.Id/500</returns>        
        [Route("[action]")]
        [HttpPost]
        public ActionResult UsersIns([FromBody] Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            if (dispatcher.CreateUser(user) == -1)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(user.Id);
        }

        /// <summary>
        /// DELETE: {endpoint}/1/test/usersdel
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200:Users.Id/500</returns>        
        [Route("[action]/{id}")]
        [HttpDelete]
        public ActionResult UsersDel(int id)
        {
            if (dispatcher.GetUserById(id) == null)
            {
                return NotFound(id);
            }

            if (!dispatcher.DeleteUser(id))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(id);
        }

        #endregion ------------------------------ Users -------------------------------------------

        /// <summary>
        /// GET: {endpoint}/1/test/commentsuseersidget/1
        /// Возвращает список записей Comments по UseersId в JSON формате:
        /// [{"id":1,"idUser":1,"txt":"test text"},
        ///  {"id":2,"idUser":1,"txt":"test text"},
        ///  {"id":3,"idUser":1,"txt":"test text"}]
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200:List<Comments></returns>
        [Route("[action]/{id}")]
        [HttpGet]
        public ActionResult<List<Comments>> CommentsUseersIdGet(int id)
        {
            return Ok(dispatcher.GetCommentsByUserId(id).ToList());
        }

        /// <summary>
        /// GET: {endpoint}/1/test/commentsget/1
        /// Возвращает запись Comments по ID в JSON формате:
        /// {"id": 1,"idUser": 2,"txt": "test text"}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200:Comments/404</returns>
        [Route("[action]/{id}")]
        [HttpGet]
        public ActionResult<Comments> CommentsGet(int id)
        {
            Comments comment = dispatcher.GetCommentById(id);

            if (comment == null)
            {
                return NotFound(); // 404
            }

            return Ok(comment);
        }

        /// <summary>
        /// GET: {endpoint}/1/test/commentsget
        /// Возвращает все записи Comments в JSON формате:
        /// [{"id":1,"idUser":1,"txt":"test text"},
        ///  {"id":2,"idUser":2,"txt":"test text"},
        ///  {"id":3,"idUser":3,"txt":"test text"}]
        /// </summary>
        /// <param></param>
        /// <returns>200:List<Comments></returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<Comments>> CommentsGet()
        {            
            return Ok(dispatcher.GetComments().ToList());
        }

        /// <summary>
        ///  PUT: {endpoint}/1/test/commentsupd
        ///  body content: JSON Comments
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>200:Comments.Id/500</returns>        
        [Route("[action]")]
        [HttpPut]
        public ActionResult CommentsUpd([FromBody] Comments comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            if (dispatcher.GetCommentById(comment.Id) == null)
            {
                return NotFound(comment.Id);
            }

            if (!dispatcher.UpdateComment(comment))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(comment.Id);
        }

        /// <summary>
        /// POST: {endpoint}/1/test/commentsins
        /// body content: JSON Comments
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>200:Comments.Id/500</returns>        
        [Route("[action]")]
        [HttpPost]
        public ActionResult CommentsIns([FromBody] Comments comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            if (dispatcher.CreateComment(comment) == -1)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(comment.Id);
        }

        /// <summary>
        /// DELETE: {endpoint}/1/test/commentsdel
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200:Comments.Id/500</returns>        
        [Route("[action]/{id}")]
        [HttpDelete]
        public ActionResult CommentsDel(int id)
        {
            if (dispatcher.GetCommentById(id) == null)
            {
                return NotFound(id);
            }

            if (!dispatcher.DeleteComment(id))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(id);
        }

    }
}

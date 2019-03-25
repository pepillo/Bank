using Bank.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Bank.Controllers.WebApi
{

    [RoutePrefix("api/v1/user")]
    public class UserController : ApiStruct.BaseAPI
    {
        [HttpGet]
        [Route("{ID:int:min(1)}")]
        [Route("~/api/user/{ID:int:min(1)}")]
        public dynamic GetUser(int ID)
        {
            var User = new User().GetByID(ID);

            return this.SetValue(User).Result();
        }

        // GET /api/authors/1/books
        //[Route("~/api/authors/{authorId:int:min(1)}/books")]
        //public IEnumerable<Book> GetByAuthor(int authorId) { ... }
    }
}

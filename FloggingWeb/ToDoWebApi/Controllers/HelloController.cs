using System.Collections.Generic;
using System.Web.Http;

namespace ToDoWebApi.Controllers
{
    [AllowAnonymous]
    public class HelloController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "Hello", "World" };
        }                
    }
}

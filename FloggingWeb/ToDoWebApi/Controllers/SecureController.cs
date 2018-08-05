using System;
using System.Web.Http;

namespace ToDoWebApi.Controllers
{
    public class SecureController : ApiController
    {
        public string Get(string goodOrBad)
        {
            if (goodOrBad.ToLower() != "good")
                throw new Exception("Not the droids you're looking for.");

            return "Hello there!";
        }    
    }
}

using Flogging.Core;
using System.Web.Http;

namespace FloggingApi.Controllers
{
    public class ErrorController : ApiController
    {
        public void Write([FromBody] FlogDetail logEntry)
        {            
            Flogger.WriteError(logEntry);
        }
    }
}

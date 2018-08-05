using Flogging.Core;
using System.Web.Http;

namespace FloggingApi.Controllers
{
    public class UsageController : ApiController
    {
        public void Write([FromBody] FlogDetail logEntry)
        {
            Flogger.WriteUsage(logEntry);
        }
    }
}

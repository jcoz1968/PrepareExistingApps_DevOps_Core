using Flogging.Core;
using System.Web.Http;

namespace FloggingApi.Controllers
{
    public class PerformanceController : ApiController
    {
        public void Write([FromBody] FlogDetail logEntry)
        {
            Flogger.WritePerf(logEntry);
        }
    }
}

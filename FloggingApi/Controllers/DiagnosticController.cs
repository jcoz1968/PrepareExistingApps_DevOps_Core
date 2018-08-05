using Flogging.Core;
using System.Web.Http;

namespace FloggingApi.Controllers
{
    public class DiagnosticController : ApiController
    {
        // Requires EnableDiagnostics=true in web.config AppSettings
        public void Write([FromBody] FlogDetail logEntry)
        {
            Flogger.WriteDiagnostic(logEntry);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ServerSidePowerShell
{
    class PowershellHttpHandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            return new PowerShellHttpHandler();
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
            //handler.Dispose();
        }
    }
}

using System.Web;

namespace ServerSidePowerShell
{
    internal class PSHttpRequest
    {
        private HttpRequestBase request;

        public PSHttpRequest(HttpRequestBase request)
        {
            this.request = request;
        }
    }
}
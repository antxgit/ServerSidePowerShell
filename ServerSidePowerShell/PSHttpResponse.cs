using System.Web;

namespace ServerSidePowerShell
{
    internal class PSHttpResponse
    {
        private HttpRequestBase request;

        public PSHttpResponse(HttpRequestBase request)
        {
            this.request = request;
        }
    }
}
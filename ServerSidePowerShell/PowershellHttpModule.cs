using System;
using System.Web;

namespace ServerSidePowerShell
{
    public class PowershellHttpModule : IHttpModule
    {
        public String ModuleName
        {
            get { return "PowershellHttpModule"; }
        }

        void IHttpModule.Dispose()
        {
            throw new NotImplementedException();
        }

        // In the Init function, register for HttpApplication 
        // events by adding your handlers.
        void IHttpModule.Init(HttpApplication application)
        {
            application.BeginRequest += (new EventHandler(this.Application_BeginRequest));
            application.EndRequest += (new EventHandler(this.Application_EndRequest));
        }

        // Your BeginRequest event handler.
        private void Application_BeginRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            context.Response.Write("<h1><font color=red>HelloWorldModule: Beginning of Request</font></h1><hr>");
        }

        // Your EndRequest event handler.
        private void Application_EndRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            context.Response.Write("<hr><h1><font color=red>HelloWorldModule: End of Request</font></h1>");
        }
    }
}

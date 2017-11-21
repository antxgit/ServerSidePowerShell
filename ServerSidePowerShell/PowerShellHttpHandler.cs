using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Web;

namespace ServerSidePowerShell
{

    public class PowerShellHttpHandler : IHttpHandler
    {
        bool IHttpHandler.IsReusable => false;

        void IHttpHandler.ProcessRequest(HttpContext context)
        {

            HttpContextBase contextBase = new HttpContextWrapper(context);
            this.ProcessRequest(contextBase);

        }


        internal void ProcessRequest(HttpContextBase contextBase)
        {
            RunspaceConfiguration config = RunspaceConfiguration.Create();
            Runspace runspace = RunspaceFactory.CreateRunspace(config);
            runspace.Open();
            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);
            scriptInvoker.Invoke("Set-ExecutionPolicy Unrestricted -Scope Process");

            using (runspace)
            {
                PowerShell powershell = PowerShell.Create();

                powershell.Runspace = runspace;

                var pipeline = runspace.CreatePipeline();
                using (pipeline)
                {
                    string scriptFullPath = contextBase.Request.PhysicalPath;
                    var cmd = new Command(scriptFullPath, true);

                    IEnumerable<string> parameters = null;
                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                        {
                            cmd.Parameters.Add(p);
                        }
                    }
                    pipeline.Commands.Add(cmd);
                    var results = pipeline.Invoke();
                    foreach(var result in results)
                    {
                        Console.WriteLine(result);
                    }
                }

            }


        }
    }
}

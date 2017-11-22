using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
                    using (StreamReader sr = new StreamReader(scriptFullPath))
                    {
                        // Read the stream to a string, and write the string to the console.
                        String scriptContent = sr.ReadToEnd();
                        pipeline.Commands.AddScript(scriptContent);
                        //var cmd = new Command(scriptContent);

                        Command myCommand = new Command(scriptFullPath);
                        CommandParameter testParam = new CommandParameter("key", "value");
                        myCommand.Parameters.Add(testParam);

                        pipeline.Commands.Add(myCommand);

                        IEnumerable<string> parameters = null;

                        if (parameters != null)
                        {
                        }

                    }

                    var psRequest = new PSHttpRequest(contextBase.Request);
                    var psResponse = new PSHttpResponse(contextBase.Request);

                    runspace.SessionStateProxy.SetVariable("Request", psRequest);
                    runspace.SessionStateProxy.SetVariable("Response", psResponse);

                    var results = pipeline.Invoke();

                    System.Collections.ObjectModel.Collection<PSObject> outputs = pipeline.Output.NonBlockingRead();

                    foreach (var output in outputs)
                    {
                        contextBase.Response.Write("o:" + output.ToString());
                    }

                    foreach (var result in results)
                    {
                        contextBase.Response.Write("r:" + result.ToString());

                    }
                    foreach (var error in powershell.Streams.Error)
                    {
                        contextBase.Response.Write("e:" + error);
                    }


                }

            }


        }
    }
}

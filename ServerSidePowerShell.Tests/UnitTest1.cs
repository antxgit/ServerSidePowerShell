using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

namespace ServerSidePowerShell.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var targetHandler = new PowerShellHttpHandler();

            HttpContextBase testContext = new TestHttpContext();

            targetHandler.ProcessRequest(testContext);

        }
    }

    class TestHttpContext : HttpContextBase
    {
        

    }

}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.IO;

namespace ServerSidePowerShell.Tests
{
    [TestClass]
    public class UnitTest1 : ScriptTestBase
    {
        [ClassInitialize]
        public static void TestClassInitialize(TestContext testContext)
        {
            ScriptTestBase.TestBaseInitialize(testContext);
        }


        [TestMethod]
        [DeploymentItem("Scripts\\HelloWorld.ps1", "Scripts")]
        public void TestPlaneScript()
        {
            var script = GetScript("HelloWorld.ps1");

            var webMock = new WebMocks
            {
                PhysicalPath = GetScript("HelloWorld.ps1")
            };

            var httpContext = webMock.CreateHttpContext();

            var targetHandler = new PowerShellHttpHandler();
            targetHandler.ProcessRequest(httpContext);
            Assert.AreEqual("Hello World!", webMock.GetResponseAsText());

        }
    }
}

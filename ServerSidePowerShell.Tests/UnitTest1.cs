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
            ScriptTestBase.TestClassInitialize(testContext);
        }


        [TestMethod]
        [DeploymentItem("Scripts\\HelloWorld.ps1", "Scripts")]
        public void TestPlaneScript()
        {
            HttpServerUtilityMock httpServerUtilityMock = new HttpServerUtilityMock();
            HttpContextBase httpContextMock = new HttpContextMock
            {
                TheRequest = new HttpRequestMock { MyPhysicalPath = GetScript("HelloWorld.ps1") },
                TheServer = httpServerUtilityMock
            };

            var targetHandler = new PowerShellHttpHandler();
            targetHandler.ProcessRequest(httpContextMock);
            Assert.Equals(httpContextMock.Response.ToString(), "HelloWorld!");

        }
    }
    class HttpRequestMock : HttpRequestBase
    {
        public override bool IsAuthenticated { get { return IsItAuthenticated; } }
        public bool IsItAuthenticated { get; set; }

        public override string PhysicalPath => MyPhysicalPath;
        public string MyPhysicalPath { get; set; }

    }

    class HttpContextMock : HttpContextBase
    {
        public override HttpRequestBase Request { get { return TheRequest; } }
        public override HttpServerUtilityBase Server { get { return TheServer; } }
        public HttpRequestBase TheRequest { get; set; }
        public HttpServerUtilityBase TheServer { get; set; }

    }

    class HttpServerUtilityMock : HttpServerUtilityBase
    {
        private string _path;
        public override void TransferRequest(string path)
        {
            _path = path;
        }
        public void ShouldHaveTransferredTo(string expectedPath)
        {
            Assert.AreEqual(expectedPath, _path);
        }
    }
}

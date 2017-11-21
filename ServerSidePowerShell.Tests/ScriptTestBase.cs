using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSidePowerShell.Tests
{
    [TestClass]
    public abstract class ScriptTestBase
    {
        private static string testScriptDir;

        [ClassInitialize]
        public static void TestClassInitialize(TestContext testContext)
        {
            testScriptDir = Path.Combine(testContext.DeploymentDirectory, "Scripts");
        }

        [TestInitialize]
        public void Setup()
        {
            Console.WriteLine("Setup executed.");
            //begin transaction
        }

        [TestCleanup]
        public void Cleanup()
        {
            Console.WriteLine("Cleanup executed.");
            //rollback transaction
        }

        public static string GetScript(string scriptName)
        {
            return Path.Combine(testScriptDir, scriptName);
        }

    }
}

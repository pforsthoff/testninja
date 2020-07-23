using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Core;

namespace TestNinja.Models
{
    public static class TestRunner
    {
        public static bool InProgress = false;
        public static StringBuilder Output = new StringBuilder();
        private static RemoteTestRunner Runner;

        public static void Start(string fileName)
        {
            InProgress = true;
            Output = new StringBuilder();
            StartTests(fileName);
        }

        private static void StartTests(string fileName)
        {
            //start nunit.
            var testPackage = new TestPackage(fileName);
            Runner = new RemoteTestRunner();
            Runner.Load(testPackage);
            var nunitEventListener = new NUnitEventListener();
            nunitEventListener.CompletedRun += new EventHandler(nunitEventListener_CompletedRun);
            nunitEventListener.Output = Output;
            Runner.BeginRun(nunitEventListener,null,false,LoggingThreshold.Fatal);
        }

        static void nunitEventListener_CompletedRun(object sender, EventArgs e)
        {
            if (Runner != null)
            {
                Runner.CancelRun();
                Runner = null;
            }
            InProgress = false;
        }
    }
}

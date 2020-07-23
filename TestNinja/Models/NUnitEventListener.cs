using System;
using System.Text;
using NUnit.Core;

namespace TestNinja.Models
{
    public class NUnitEventListener : NUnit.Core.EventListener
    {
        public event EventHandler CompletedRun;
        public StringBuilder Output;
        private int TotalTestsPassed = 0;
        private int TotalTestsErrored = 0;

        public void RunStarted(string name, int testCount)
        {
            Output.AppendLine(TimeStamp + "Running " + testCount + " tests in " + name + "<br/><br/>");
            TotalTestsPassed = 0;
            TotalTestsErrored = 0;
        }

        public void RunFinished(System.Exception exception)
        {
            Output.AppendLine(TimeStamp + "Run errored: " + exception.ToString() + "<br/>");
            //notify event consumers.
            if (CompletedRun != null)
                CompletedRun(exception, new EventArgs());
        }

        public void RunFinished(NUnit.Core.TestResult result)
        {
            Output.AppendLine(TimeStamp + "<label class='normal " + (TotalTestsErrored == 0 ? "green" : "red")
                + "'>" + TotalTestsPassed + " tests passed, " + TotalTestsErrored + " tests failed</label><br/>");
            Output.AppendLine(TimeStamp + "Run completed in " + result.Time + " seconds<br/>");
            //notify event consumers.
            if (CompletedRun != null)
                CompletedRun(result, new EventArgs());
        }

        public void TestStarted(TestName testName)
        {
            Output.AppendLine(TimeStamp + testName.FullName + "<br/>");
        }

        public void TestOutput(NUnit.Core.TestOutput testOutput)
        {
            if (testOutput.Text.IndexOf("NHibernate:") == -1)
                Output.AppendLine(TimeStamp + testOutput.Text + "<br/>");
        }

        public void TestFinished(NUnit.Core.TestResult result)
        {
            if (result.IsSuccess)
            {
                Output.AppendLine(TimeStamp + "<label class='green normal'>Test Passed!</label><br/><br/>");
                TotalTestsPassed++;
            }
            else
            {
                Output.AppendLine(TimeStamp + "<label class='red normal'>Test Failed!<br/>" + result.Message.Replace(Environment.NewLine, "<br/>") + "</label><br/>");
                TotalTestsErrored++;
            }
        }

        public void UnhandledException(System.Exception exception)
        {
            Output.AppendLine(TimeStamp + "Unhandled Exception: " + exception.ToString() + "<br/>");
        }

        public void SuiteStarted(TestName testName)
        {
        }

        public void SuiteFinished(NUnit.Core.TestResult result)
        {
        }

        private string TimeStamp
        {
            get
            {
                return "[" + DateTime.Now.ToString() + "] ";
            }
        }
    }
}

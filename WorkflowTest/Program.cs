//using NumberGuessWorkflowActivities;
using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WorkflowTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Activity workflow1 = new Workflow1();
            WorkflowInvoker.Invoke(workflow1);

            //AutoResetEvent syncEvent = new AutoResetEvent(false);
            //AutoResetEvent idleEvent = new AutoResetEvent(false);

            //var inputs = new Dictionary<string, object>() { { "MaxNumber", 100 } };

            //WorkflowApplication wfApp =
            //    new WorkflowApplication(new FlowchartNumberGuessWorkflow(), inputs);

            //wfApp.Run();
        }
    }
}

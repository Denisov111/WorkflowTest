using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInvoker
{
    public interface IWorkflowRuntime
    {
        Version Version { get; }

        Activity GetRootActivity(Guid workflowInstanceId);
    }
}

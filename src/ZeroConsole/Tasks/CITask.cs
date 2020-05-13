using System;
using System.Collections.Generic;
using System.Text;
using ZeroConsole.Model;

namespace ZeroConsole.Tasks
{
    public class CITask
    {
        public CITask(GitPushEvent pushEvent)
        {
            Id = ObjectId.Next("Task#");
            PushEvent = pushEvent;
        }
        public int Id { get; set; }

        public GitPushEvent PushEvent { get; }

        
    }
}

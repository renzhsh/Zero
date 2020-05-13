using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZeroConsole.Model;

namespace ZeroConsole.Tasks
{
    /// <summary>
    /// TaskEngine
    /// </summary>
    public class TaskEngine
    {
        private readonly ConcurrentBag<CITask> taskQueue = new ConcurrentBag<CITask>();

        /// <summary>
        /// ThreadPool 最大工作线程数
        /// </summary>
        public static int MaxWorkingThreadCount = 3;

        /// <summary>
        /// ThreadPool 最大异步IO线程数
        /// </summary>
        public static int MaxAIOThreadCount = 8;

        public TaskEngine()
        {
            ThreadPool.SetMaxThreads(MaxWorkingThreadCount, MaxAIOThreadCount);
        }

        public void AddPushEvent(GitPushEvent pushEvent)
        {
            if (string.IsNullOrEmpty(pushEvent.Platform) ||
                !(pushEvent.Platform.Equals("github") ||
                pushEvent.Platform.Equals("gitee")))
            {
                return;
            }

            taskQueue.Add(new CITask(pushEvent));

            this.StartNextExecutor();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartNextExecutor()
        {
            int available = MaxWorkingThreadCount - (int)ThreadPool.PendingWorkItemCount;

            for (var i = 0; i < available; i++)
            {
                if (taskQueue.Count() == 0) break;

                if (taskQueue.TryTake(out CITask task))
                {
                    var executor = new TaskExecutor(task);

                    executor.TaskFinished += (state) =>
                    {
                        this.StartNextExecutor();
                    };

                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        executor.Run();
                    });
                }
            }
        }
    }
}

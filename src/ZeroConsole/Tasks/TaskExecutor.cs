using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ZeroConsole.Tasks.Shells;
using System.Threading.Tasks;

namespace ZeroConsole.Tasks
{
    using CIOptions;
    using System.Linq;

    public delegate void TaskFinished(CITask task);

    /// <summary>
    /// 任务执行器
    /// </summary>
    public class TaskExecutor
    {
        private readonly CITask target;

        public TaskContext Context { get; }

        public List<JobOption> Jobs { get; }

        public string[] BeforeScript { get; private set; }

        public string[] AfterScript { get; private set; }

        public TaskExecutor(CITask task)
        {
            target = task;

            Context = new TaskContext()
            {
                PushBranch = target.PushEvent.Branch,
                ProjectName = target.PushEvent.Repository.Name,
                ProjectDir = $"{target.PushEvent.Platform}/{target.PushEvent.Repository.FullName}"
            };

            Jobs = new List<JobOption>();
        }

        public async void Run()
        {
            await Init();

            Sheller sheller = new Sheller();

            bool isFailed = false;

            foreach (var job in Jobs)
            {
                Console.WriteLine($"===stage: {job.Stage}, job:{job.Name}===");
                if (BeforeScript != null)
                {
                    try
                    {
                        await sheller.Run(BeforeScript);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("==========BeforeScript Exception==========");
                        Console.WriteLine(ex.Message);
                    }
                }

                try
                {
                    if (job.When == ExecuteWhen.on_success && isFailed) continue;
                    if (job.When == ExecuteWhen.on_failure && !isFailed) continue;

                    if (!await sheller.Run(job.Script))
                    {
                        isFailed = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (AfterScript != null)
                {
                    try
                    {
                        await sheller.Run(AfterScript);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("==========AfterScript Exception==========");
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            TaskFinished?.Invoke(target);
        }

        private async Task Init()
        {
            await PullCode();

            ZeroCIOption options = FindCIOption();
            if (options == null) return;

            BuildJobs(options);
        }

        /// <summary>
        /// 拉取代码
        /// </summary>
        private async Task PullCode()
        {
            Sheller sheller = new Sheller
            {
                LogReceiver = new ConsoleLogReceiver()
            };

            List<string> cmds = new List<string>();

            DirectoryInfo dir = new DirectoryInfo(Context.ProjectDir);

            // git clone
            if (!dir.Exists)
            {
                var parent = dir.Parent;
                if (!parent.Exists) parent.Create();

                cmds.Add($"cd {parent.FullName}");
                cmds.Add($"git clone {target.PushEvent.Repository.SshUrl}");
            }

            cmds.Add($"cd {dir.FullName}");
            cmds.Add($"git fetch");
            cmds.Add($"git checkout {Context.PushBranch}");
            cmds.Add($"git pull origin {Context.PushBranch}");

            await sheller.Run(cmds.ToArray());
        }

        private ZeroCIOption FindCIOption()
        {
            FileInfo info = new FileInfo($"{Context.ProjectDir}/zero-ci.yml");

            if (!info.Exists)
            {
                return null;
            }

            return new ZeroCIOptionBuilder().AddYamlFile(info.FullName).Build();
        }

        private void BuildJobs(ZeroCIOption options)
        {
            var jobOptions = options.Where(job => !job.IsIgnore && job.Script != null &&
                (
                (job.Only != null && job.Only.Contains(Context.PushBranch) ||
                !(job.Except != null && job.Except.Contains(Context.PushBranch)))
                )
            );

            if (options.BeforeScript != null)
            {
                BeforeScript = PretreatJobScript(options.BeforeScript);
            }

            if (options.AfterScript != null)
            {
                AfterScript = PretreatJobScript(options.AfterScript);
            }

            if (jobOptions == null) return;

            foreach (var stage in options.Stages)
            {
                var stageJobs = jobOptions.Where(job => job.Stage == stage)
                    .Select(job =>
                    {
                        job.Script = PretreatJobScript(job.Script);

                        return job;
                    });

                if (stageJobs != null)
                {
                    Jobs.AddRange(stageJobs);
                }
            }
        }

        /// <summary>
        /// 预处理JobScript
        /// 1、定位到项目根目录
        /// 2、变量替换
        /// </summary>
        /// <param name="jobOption"></param>
        /// <returns></returns>
        private string[] PretreatJobScript(string[] script)
        {
            var temp = script.Prepend($"cd {Context.ProjectDir}").ToArray();

            // TODO : 替换变量

            return temp;
        }

        /// <summary>
        /// 任务完成
        /// </summary>
        public event TaskFinished TaskFinished;

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroConsole.Tasks
{
    public class TaskContext
    {
        /// <summary>
        /// 项目目录
        /// </summary>
        public string ProjectDir { get; set; }

        /// <summary>
        /// 推送分支
        /// </summary>
        public string PushBranch { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
    }
}

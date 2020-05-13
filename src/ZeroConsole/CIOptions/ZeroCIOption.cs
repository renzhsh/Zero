using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroConsole.CIOptions
{
    public class ZeroCIOption : List<JobOption>
    {
        /// <summary>
        /// 定义 stages（阶段）。任务将按此顺序执行。
        /// </summary>
        public string[] Stages { get; set; }

        /// <summary>
        /// 每个job之前都会执行
        /// </summary>
        public string[] BeforeScript { get; set; }

        /// <summary>
        /// 每个job之后都会执行
        /// </summary>
        public string[] AfterScript { get; set; }

        /// <summary>
        /// 全局变量
        /// </summary>
        public Dictionary<string, string> Variables { get; set; }
    }

    public class JobOption
    {
        public string Name { get; set; }

        public string Stage { get; set; }

        /// <summary>
        /// 哪些分支提交代码可以执行这个任务
        /// </summary>
        public string[] Only { get; set; }

        /// <summary>
        /// 哪些分支提交代码不执行这个任务
        /// </summary>
        public string[] Except { get; set; }

        public string[] Script { get; set; }

        /// <summary>
        /// 执行时机
        /// </summary>
        public ExecuteWhen When { get; set; } = ExecuteWhen.on_success;

        /// <summary>
        /// 不执行
        /// </summary>
        public bool IsIgnore
        {
            get
            {
                return !string.IsNullOrEmpty(Name) && Name.StartsWith(".");
            }
        }
    }

    /// <summary>
    /// 执行时机
    /// </summary>
    public enum ExecuteWhen
    {
        /// <summary>
        /// 总是执行
        /// </summary>
        always,
        /// <summary>
        /// 前面几步全部成功后执行
        /// </summary>
        on_success,
        /// <summary>
        /// 在失败的情况下执行
        /// </summary>
        on_failure
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroConsole.Model
{
    /// <summary>
    /// Git推送事件
    /// </summary>
    public class GitPushEvent
    {
        /// <summary>
        /// 变更的分支
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// github or gitee or gitlab
        /// </summary>
        public string Platform { get; set; }

        public Repository Repository { get; set; }

        public HeadCommit HeadCommit { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime PushedTime { get; } = DateTime.Now;

        /// <summary>
        /// 同一分支重复提交
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsDuplicated(GitPushEvent other)
        {
            return Repository.SshUrl.Equals(other.Repository.SshUrl) &&
                Branch.Equals(other.Branch);
        }
    }

    public class Repository
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 仓库全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 仓库地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// SSH访问地址
        /// </summary>
        public string SshUrl { get; set; }

    }

    public class HeadCommit
    {
        public string Id { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }

        public string Url { get; set; }

        public Author Author { get; set; }

        public Author Committer { get; set; }

        public string[] Added { get; set; }

        public string[] Removed { get; set; }

        public string[] Modified { get; set; }
    }

    public class Author
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }
    }
}

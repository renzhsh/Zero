using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroWeb.Model
{
    /// <summary>
    /// 仓库
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// github or gitee
        /// </summary>
        public string Platform { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Url { get; set; }

        public string SslUrl { get; set; }
    }
}

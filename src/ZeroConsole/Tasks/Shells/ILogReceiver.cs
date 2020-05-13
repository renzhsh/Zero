using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroConsole.Tasks.Shells
{
    /// <summary>
    /// Task日志接收器
    /// </summary>
    public interface ILogReceiver
    {
        /// <summary>
        /// 接受标准输出
        /// </summary>
        /// <param name="message"></param>
        void ReceiveInfo(string message);

        /// <summary>
        /// 接受警告输出
        /// </summary>
        /// <param name="message"></param>
        void ReceiveWarn(string message);

        /// <summary>
        /// 接受错误输出
        /// </summary>
        /// <param name="message"></param>
        void ReceiveError(string message);
    }
}

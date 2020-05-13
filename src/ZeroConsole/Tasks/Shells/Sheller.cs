using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroConsole.Tasks.Shells
{
    /// <summary>
    /// 脚本执行器
    /// </summary>
    public class Sheller
    {
        /// <summary>
        /// 日志接收器
        /// </summary>
        public ILogReceiver LogReceiver { get; set; } = new ConsoleLogReceiver();

        /// <summary>
        /// 运行脚本
        /// </summary>
        /// <param name="cmdLine"></param>
        public Task<bool> Run(string cmdLine)
        {
            return Run(new string[] { cmdLine });
        }

        /// <summary>
        /// 运行脚本
        /// </summary>
        /// <param name="cmdLines"></param>
        public Task<bool> Run(string[] cmdLines)
        {
            string cmdFile = WriteFile(cmdLines);

            try
            {
                var psi = new ProcessStartInfo(cmdFile)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                var proc = Process.Start(psi);
                proc.BeginErrorReadLine();
                proc.BeginOutputReadLine();

                proc.OutputDataReceived += (sender, args) =>
                {
                    LogReceiver?.ReceiveInfo(args.Data);
                };

                proc.ErrorDataReceived += (sender, args) =>
                {
                    LogReceiver?.ReceiveError(args.Data);
                };

                proc.WaitForExit();

                return Task.FromResult(proc.ExitCode == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Process启动异常", ex);
            }
            finally
            {
                File.Delete(cmdFile);
            }
        }

        private string WriteFile(string[] cmdLines)
        {
            string file = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                file = $"{file}.bat";
                File.WriteAllLines(file, cmdLines, Encoding.ASCII);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                file = $"{file}.sh";
                File.WriteAllText(file, "#!/bin/bash", Encoding.ASCII);
                File.WriteAllLines(file, cmdLines, Encoding.ASCII);
            }

            return file;
        }
    }
}

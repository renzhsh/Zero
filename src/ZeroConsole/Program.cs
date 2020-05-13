using System;
using System.Diagnostics;
using System.IO;
using ZeroConsole.Model;
using System.Threading.Tasks;
using System.Text;
using System.Runtime.InteropServices;
using ZeroConsole.Tasks;
using System.Threading;
using ZeroConsole.Tasks.Shells;

namespace ZeroConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            GitPushEvent gitPushEvent =
                new GitPushEventBuilder()
                .AddPushPayload(File.ReadAllText("payload.json"))
                .Build();

            gitPushEvent.Platform = "github";

            TaskEngine engine = new TaskEngine();

            engine.AddPushEvent(gitPushEvent);

            Console.ReadKey();

        }
    }

    public class ConsoleLogReceiver : ILogReceiver
    {
        public void ReceiveError(string message)
        {
            Console.WriteLine(message);
        }

        public void ReceiveInfo(string message)
        {
            Console.WriteLine(message);
        }

        public void ReceiveWarn(string message)
        {
            Console.WriteLine(message);
        }
    }
}

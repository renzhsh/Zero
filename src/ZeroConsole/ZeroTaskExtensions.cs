using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroConsole.Tasks;

namespace ZeroConsole
{
    public static class ZeroTaskExtensions
    {
        public static void AddZeroTask(this IServiceCollection services)
        {
            services.AddSingleton<TaskEngine>();
        }
    }
}

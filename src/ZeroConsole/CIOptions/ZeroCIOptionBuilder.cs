using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroConsole.CIOptions
{
    public class ZeroCIOptionBuilder
    {
        private IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

        public ZeroCIOptionBuilder AddYamlFile(string path)
        {
            configurationBuilder.AddYamlFile(path);
            return this;
        }

        public ZeroCIOption Build()
        {
            IConfiguration configuration = configurationBuilder.Build();
            ZeroCIOption option = new ZeroCIOption
            {
                Stages = configuration.GetSection("stages").Get<string[]>(),
                BeforeScript = configuration.GetSection("before_script").Get<string[]>(),
                AfterScript = configuration.GetSection("after_script").Get<string[]>(),
                Variables = configuration.GetSection("variables").Get<Dictionary<string, string>>()
            };

            string[] usedKeys = new string[] { "stages", "before_script", "after_script", "variables" };

            var sections = configuration.GetChildren();
            foreach (var section in sections)
            {
                if (usedKeys.Contains(section.Key)) continue;

                var job = section.Get<JobOption>();
                if (job.Stage != null && job.Script != null)
                {
                    job.Name = section.Key;
                    option.Add(job);
                }
            }

            return option;
        }
    }
}

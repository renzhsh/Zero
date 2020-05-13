using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ZeroConsole.Model
{
    public class GitPushEventBuilder
    {
        private readonly IConfigurationBuilder builder = new ConfigurationBuilder();
        public GitPushEventBuilder AddPushPayload(string payload)
        {
            builder.AddJsonString(payload);
            return this;
        }

        public GitPushEvent Build()
        {
            var config = builder.Build();

            GitPushEvent pushEvent = new GitPushEvent();

            string branch = config.GetSection("ref").Get<string>();
            if (!string.IsNullOrEmpty(branch))
            {
                pushEvent.Branch = branch.Split('/').Last();
            }

            pushEvent.HeadCommit = config.GetSection("head_commit").Get<HeadCommit>();

            var repoSection = config.GetSection("repository");

            pushEvent.Repository = repoSection.Get<Repository>();

            if (pushEvent.Repository != null)
            {
                pushEvent.Repository.FullName = repoSection.GetValue<string>("full_name");
                pushEvent.Repository.SshUrl = repoSection.GetValue<string>("ssh_url");
            }

            return pushEvent;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// 添加JSON字符串
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddJsonString(this IConfigurationBuilder builder, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content");
            }

            MemoryStream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream, Encoding.UTF8);

            sw.Write(content);
            sw.Flush();

            stream.Position = 0;

            return builder.AddJsonStream(stream);
        }
    }
}

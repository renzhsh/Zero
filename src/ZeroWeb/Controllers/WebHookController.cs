using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ZeroConsole.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public WebHookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("push")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:不需要赋值", Justification = "<挂起>")]
        public IActionResult Push(object payload)
        {
            var agent = Request.Headers["User-Agent"].ToString();

            string type = "";

            if (!string.IsNullOrEmpty(agent))
            {
                if (agent.StartsWith("GitHub-Hookshot"))
                {
                    type = "github";
                    if (!CheckGitHubSignature(payload))
                    {
                        return BadRequest();
                    }
                }
                else if (agent.Equals("git-oschina-hook"))
                {
                    type = "gitee";
                    if (!CheckGiteeSignature())
                    {
                        return BadRequest();
                    }
                }
            }

            return Ok();
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        private bool CheckGitHubSignature(object payload)
        {
            string secret = _configuration.GetValue<string>("GitHub:Secret");
            string content = payload.ToString();

            var encoding = Encoding.UTF8;
            byte[] key = encoding.GetBytes(secret);
            byte[] data = encoding.GetBytes(content);

            var origin = Request.Headers["X-Hub-Signature"].ToString();
            var current = "";

            using (var hmac = new HMACSHA1(key))
            {
                byte[] hash = hmac.ComputeHash(data);

                current = "sha1=" + BitConverter.ToString(hash).Replace("-", "").ToLower();
            }

            return origin.Equals(current);
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        private bool CheckGiteeSignature()
        {
            var token = Request.Headers["X-Gitee-Token"].ToString();
            var timestamp = Request.Headers["X-Gitee-Timestamp"].ToString();

            string secret = _configuration.GetValue<string>("Gitee:Secret");
            string content = $"{timestamp}\n{secret}";

            var encoding = Encoding.UTF8;
            byte[] key = encoding.GetBytes(secret);
            byte[] data = encoding.GetBytes(content);


            var signature = "";

            using (var hmac = new HMACSHA256(key))
            {
                byte[] hash = hmac.ComputeHash(data);

                signature = Convert.ToBase64String(hash);
            }

            return signature.Equals(token);
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SampleFunctionApp.HttpTrigger
{
    public class BaseHttpTrigger
    {
        public RequestContext Request(HttpRequest req, ILogger logger)
        {
            return new RequestContext(req, logger);
        }

        public bool ValidateCustomKey(string customKey)
        {
            string expectedKey = Environment.GetEnvironmentVariable("CustomAuthKey");
            return customKey == expectedKey;
        }
    }

    public class RequestContext
    {
        private readonly HttpRequest _httpRequest;
        private readonly ILogger _logger;
        private AuthorizationLevel _authorizationLevel;
        private string _customKey;

        public RequestContext(HttpRequest httpRequest, ILogger logger)
        {
            _httpRequest = httpRequest;
            _logger = logger;
        }

        public RequestContext Authorize(AuthorizationLevel authorizationLevel, string customKey)
        {
            _authorizationLevel = authorizationLevel;
            _customKey = customKey;
            if (!new BaseHttpTrigger().ValidateCustomKey(customKey))
            {
                throw new UnauthorizedAccessException("Invalid custom key");
            }
            return this;
        }

        public async Task<IActionResult> Run(Func<HttpRequest, ILogger, Task<IActionResult>> func)
        {
            try
            {
                if (_authorizationLevel != AuthorizationLevel.Anonymous && !new BaseHttpTrigger().ValidateCustomKey(_customKey))
                {
                    return new ForbidResult();
                }

                return await func(_httpRequest, _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled error processing request with url {_httpRequest.GetDisplayUrl()}, method {_httpRequest.Method}");

                return new ObjectResult($"Unhandled error processing request. Error: {ex}")
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}

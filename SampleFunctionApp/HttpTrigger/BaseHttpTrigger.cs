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
    }

    public class RequestContext
    {
        private readonly HttpRequest _httpRequest;
        private readonly ILogger _logger;
        private AuthorizationLevel _authorizationLevel;
        public RequestContext(HttpRequest httpRequest, ILogger logger)
        {
            _httpRequest = httpRequest;
            _logger = logger;
        }

        public RequestContext Authorize(AuthorizationLevel authorizationLevel)
        {
            //TODO: Load required data to perform authenticationa nd authorization
            // For now just to have a place holder I created this
            _authorizationLevel = authorizationLevel;
            return this;
        }

        public async Task<IActionResult> Run(Func<HttpRequest, ILogger, Task<IActionResult>> func)
        {
            try
            {
                //TODO: Perform custom authentication and authorization logic
                // For now just to have a place holder I created this
                if (_authorizationLevel != AuthorizationLevel.Anonymous)
                {
                    return new ForbidResult();
                }

                //Call the actual WORK method to perform the task
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

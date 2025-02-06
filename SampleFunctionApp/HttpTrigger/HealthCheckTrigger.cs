using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SampleFunctionApp.HttpTrigger
{
    public static class HealthCheckTrigger
    {
        [FunctionName("HealthCheckTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HealthCheckTrigger function processed a request.");

            return new OkObjectResult("App is healthy");
        }
    }
}

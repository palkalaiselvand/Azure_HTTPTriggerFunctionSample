using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SampleApp.Domain.Engine;
using SampleApp.Domain.Repository.Abstraction;
using SampleApp.Shared;
using SampleApp.Shared.AzureAssets.Abstraction;
using SampleApp.Shared.Constants;
using SampleApp.Shared.Data;
using SampleApp.Shared.Enums;
using SampleApp.Shared.Extension;
using SampleFunctionApp.HttpTrigger;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SampleFunctionApp
{
    public class UserDetailsTrigger : BaseHttpTrigger
    {
        private readonly IRequestAuditRepository _audit;
        private readonly IUserDetailsEngine _engine;
        private readonly IServiceBusFactory _serviceBus;
        private readonly IStorageQueueFactory _storageQueue;

        public UserDetailsTrigger(IRequestAuditRepository audit,
                                  IUserDetailsEngine engine,
                                  IServiceBusFactory serviceBus,
                                  IStorageQueueFactory storageQueue)
        {
            _audit = audit;
            _engine = engine;
            _serviceBus = serviceBus;
            _storageQueue = storageQueue;
        }
        [FunctionName(nameof(UserDetailsTrigger))]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/userdetails")] HttpRequest req,
            ILogger logger)
        {
            return await Request(req, logger)
                .Authorize(AuthorizationLevel.Anonymous)
                .Run(Work);
        }

        public async Task<IActionResult> Work(HttpRequest req, ILogger logger)
        {
            Guid requestId = Guid.NewGuid();
            try
            {

                //logger.LogInformation($"{nameof(UserDetailsTrigger)} function started processing the request. Request");                          
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                if (string.IsNullOrEmpty(requestBody))
                {
                    return new BadRequestObjectResult($"POST request must have request body to process");
                }

                //Audit the request for suppoert purpose

                //await _audit.Create(new RequestAudit { RequestId = requestId, Data = requestBody });

                //var data = JsonConvert.DeserializeObject<UserDetails>(requestBody);

                //data.Id = requestId;
                //await _engine.Process(data);

                var azureContext = AzureAssets.GetAzureAssets(Environment.GetEnvironmentVariable(AppSettingsKey.AzureQueueAssets));
                var data = requestBody;

                if (azureContext.AssetsType == AssetsType.ServiceBus)
                {
                    await _serviceBus.SendMessage(data, azureContext);
                }
                else
                {
                    await _storageQueue.SendMessage(data, azureContext);
                }

                await _audit.Update(requestId, ProcessStatus.COMPLETED);

                return new OkObjectResult(requestBody);

            }
            catch (Exception ex)
            {
                await _audit.Update(requestId, ProcessStatus.ERROR);
                return new BadRequestObjectResult($"Un handled exception happened. Exception: {ex.ToString()}");
            }

        }
    }
}

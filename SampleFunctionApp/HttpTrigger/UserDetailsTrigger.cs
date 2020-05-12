using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SampleFunctionApp.HttpTrigger;
using SampleApp.Domain.Repository.Abstraction;
using SampleApp.Shared.Data;
using SampleApp.Shared.Constants;
using SampleApp.Domain.Engine;
using SampleApp.Shared.AzureAssets.Abstraction;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Neleus.DependencyInjection.Extensions;

namespace SampleFunctionApp
{
    public class UserDetailsTrigger : BaseHttpTrigger
    {
        private readonly IRequestAuditRepository _audit;
        private readonly IUserDetailsEngine _engine;
        private readonly IAzureStorageFactory _service;

        public UserDetailsTrigger(IRequestAuditRepository audit,
                                  IUserDetailsEngine engine,
                                  IAzureStorageFactory service)
        {
            _audit = audit;
            _engine = engine;
            _service = service;
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

                await _audit.Create(
                    new RequestAudit
                    {
                        RequestId = requestId,
                        Data = requestBody,
                        Status = ProcessStatus.NEW,
                        META_Application = MetaData.ApplicationName,
                        META_CreatedBy = MetaData.UserName,
                        META_DateCreated = DateTime.UtcNow
                    });

                var data = JsonConvert.DeserializeObject<UserDetails>(requestBody);
                //add meta info to db records
                data.META_Application = MetaData.ApplicationName;
                data.META_CreatedBy = MetaData.UserName;
                data.META_DateCreated = DateTime.UtcNow;

                await _engine.Process(data);

                await _service.SendMessage(data, "userdetailsqueue");

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

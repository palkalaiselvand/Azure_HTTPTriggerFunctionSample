using SampleApp.Shared.Data;
using System;
using System.Threading.Tasks;

namespace SampleApp.Domain.Repository.Abstraction
{
    public interface IRequestAuditRepository
    {
        Task Create(RequestAudit request);
        Task Update(Guid requestId, string status);
        Task<RequestAudit> Get(Guid requestId);
    }
}

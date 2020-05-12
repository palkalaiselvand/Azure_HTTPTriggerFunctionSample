using Microsoft.EntityFrameworkCore;
using SampleApp.Domain.Repository.Abstraction;
using SampleApp.Shared.Data;
using SampleApp.Shared.Data.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApp.Domain.Repository
{
    public class RequestAuditRepository : IRequestAuditRepository
    {
        private readonly ISampleAppDBContext _context;
        public RequestAuditRepository(ISampleAppDBContext context)
        {
            _context = context;
        }
        public async Task Create(RequestAudit request)
        {
            try
            {
                _context.RequestAudits.Add(request);
                await _context.SaveChangesAsync(new CancellationToken());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<RequestAudit> Get(Guid requestId)
        {
            return _context.RequestAudits.FirstOrDefaultAsync(f => f.RequestId == requestId);
        }

        public async Task Update(Guid requestId, string status)
        {
            var data = await _context.RequestAudits.FirstOrDefaultAsync(f => f.RequestId == requestId);
            if (data != null)
            {
                data.Status = status;
                data.META_DateUpdated = DateTime.UtcNow;
                data.META_UpdatedBy = Environment.UserName;
                await _context.SaveChangesAsync(new CancellationToken());
            }
        }
    }
}

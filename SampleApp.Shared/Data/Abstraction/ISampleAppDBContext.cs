using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApp.Shared.Data.Abstraction
{
    public interface ISampleAppDBContext
    {
        DbSet<UserDetails> UserDetails { get; set; }
        DbSet<RequestAudit> RequestAudits { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

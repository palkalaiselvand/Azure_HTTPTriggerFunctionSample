using Microsoft.EntityFrameworkCore;
using SampleApp.Shared.Data.Abstraction;

namespace SampleApp.Shared.Data
{
    public class SampleAppDBContext : DbContext, ISampleAppDBContext
    {
        public SampleAppDBContext(DbContextOptions<SampleAppDBContext> options) : base(options)
        {

        }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<RequestAudit> RequestAudits { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}

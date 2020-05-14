using Microsoft.EntityFrameworkCore;
using SampleApp.Domain.Repository.Abstraction;
using SampleApp.Shared.Constants;
using SampleApp.Shared.Data;
using SampleApp.Shared.Data.Abstraction;
using System;
using System.Threading.Tasks;

namespace SampleApp.Domain.Repository
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly ISampleAppDBContext _context;
        public UserDetailsRepository(ISampleAppDBContext context)
        {
            _context = context;
        }
        public async Task<UserDetails> Create(UserDetails userDetails)
        {
            userDetails.META_Application = MetaData.ApplicationName;
            userDetails.META_CreatedBy = MetaData.UserName;
            userDetails.META_DateCreated = DateTime.UtcNow;

            _context.UserDetails.Add(userDetails);
            await _context.SaveChangesAsync(new System.Threading.CancellationToken());
            return userDetails;
        }

        public async Task<bool> HasUserExists(UserDetails userDetails)
        {
            return await _context.UserDetails.AnyAsync(a => a.UserName == userDetails.UserName);
        }
    }
}

using SampleApp.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApp.Domain.Repository.Abstraction
{
    public interface IUserDetailsRepository
    {
        Task<bool> HasUserExists(UserDetails userDetails);
        Task<UserDetails> Create(UserDetails userDetails);
    }
}

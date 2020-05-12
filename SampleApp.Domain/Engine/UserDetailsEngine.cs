using SampleApp.Domain.Repository.Abstraction;
using SampleApp.Shared.Data;
using System;
using System.Threading.Tasks;

namespace SampleApp.Domain.Engine
{
    public class UserDetailsEngine : IUserDetailsEngine
    {
        private readonly IUserDetailsRepository _repository;
        public UserDetailsEngine(IUserDetailsRepository repository)
        {
            _repository = repository;
        }
        public async Task Process(UserDetails userDetails)
        {
            try
            {
                await _repository.Create(userDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

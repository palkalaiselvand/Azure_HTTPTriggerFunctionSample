using SampleApp.Shared.Data;
using System.Threading.Tasks;

namespace SampleApp.Domain.Engine
{
    public interface IUserDetailsEngine
    {
        Task Process(UserDetails userDetails);
    }
}

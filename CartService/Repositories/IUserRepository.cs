using System.Collections.Generic;
using System.Threading.Tasks;
using CartService.Data.Model.IdentityServiceModel;

namespace CartService.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
    }
}

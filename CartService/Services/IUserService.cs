using System.Collections.Generic;
using System.Threading.Tasks;
using CartService.Data.Model.IdentityServiceModel;

namespace CartService.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
    }
}

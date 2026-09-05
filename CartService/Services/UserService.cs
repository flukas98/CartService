using System.Collections.Generic;
using System.Threading.Tasks;
using CartService.Data.Model.IdentityServiceModel;
using CartService.Repositories;

namespace CartService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _repo.GetAllUsersAsync();
        }
    }
}

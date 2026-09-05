using System.Collections.Generic;
using System.Threading.Tasks;
using CartService.Data;
using CartService.Data.Model.IdentityServiceModel;
using Microsoft.EntityFrameworkCore;

namespace CartService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CartDbContext _db;

        public UserRepository(CartDbContext db)
        {
            _db = db;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }
    }
}

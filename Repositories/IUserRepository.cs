using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoBackend.Entities;

namespace ToDoBackend.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<User> GetUserByIdAsync(Guid id);
        public Task<User> GetUserByEmailAsync(string email);
        public Task CreateUserAsync(User user);
        public Task UpdateUserAsync(User user);
        public Task DeleteUserAsync(Guid id);
    }
}
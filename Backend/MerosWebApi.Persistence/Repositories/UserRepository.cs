using MerosWebApi.Core.Models;
using MerosWebApi.Core.Repository;
using MerosWebApi.Persistence.Entites;
using MerosWebApi.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MerosDbContext _dbContext;

        public UserRepository(MerosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUser(User user)
        {
            var dbUser = PropertyAssigner.Map<DatabaseUser, User>(user);

            await _dbContext.Users.AddAsync(dbUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(User user)
        {
            var dbUser = PropertyAssigner.Map<DatabaseUser, User>(user);

            _dbContext.Users.Remove(dbUser);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var dbUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser == null)
                return null;

            return PropertyAssigner.Map<User, DatabaseUser>(dbUser) ;
        }

        public async Task<User> GetUserById(Guid id)
        {
            var dbUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (dbUser == null)
                return null;

            return PropertyAssigner.Map<User, DatabaseUser>(dbUser);
        }

        public async Task<User> GetUserByUnconfirmedCode(string unconfirmedCode)
        {
            var dbUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UnconfirmedEmailCode == unconfirmedCode);

            if (dbUser == null)
                return null;

            return PropertyAssigner.Map<User, DatabaseUser>(dbUser);
        }

        public async Task<User> UpdateUser(User user)
        {
            var dbUserToUpdate = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            PropertyAssigner.AssignPropertyValues<DatabaseUser, User>(dbUserToUpdate, user);

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null)
                return default;

            return PropertyAssigner.Map<User, DatabaseUser>(user);
        }
    }
}

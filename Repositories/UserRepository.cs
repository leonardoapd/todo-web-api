using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoBackend.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ToDoBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string databaseName = "UsersDB";
        private const string collectionName = "Users";
        private readonly IMongoCollection<User> _usersCollection;
        private readonly FilterDefinitionBuilder<User> filterBuilder = Builders<User>.Filter;

        

        public UserRepository(IMongoClient mongoClient)
        {
            IMongoDatabase? db = mongoClient.GetDatabase(databaseName);
            _usersCollection = db.GetCollection<User>(collectionName);
        }

        public async Task CreateUserAsync(User user)
        {
            // Hash the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            try
            {
                await _usersCollection.InsertOneAsync(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Task DeleteUserAsync(Guid id)
        {
            try
            {
                var filter = filterBuilder.Eq(existingUser => existingUser.Id, id);
                return _usersCollection.DeleteOneAsync(filter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _usersCollection.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var filter = filterBuilder.Eq(existingUser => existingUser.Email, email);
                return _usersCollection.Find(filter).SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task<User> GetUserByIdAsync(Guid id)
        {
            try
            {
                var filter = filterBuilder.Eq(existingUser => existingUser.Id, id);
                return _usersCollection.Find(filter).SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task UpdateUserAsync(User user)
        {
            try
            {
                var filter = filterBuilder.Eq(existingUser => existingUser.Id, user.Id);
                return _usersCollection.ReplaceOneAsync(filter, user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


    }
}
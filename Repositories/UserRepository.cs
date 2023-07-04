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
            await _usersCollection.InsertOneAsync(user);
        }

        public Task DeleteUserAsync(int id)
        {
            var filter = filterBuilder.Eq(existingUser => existingUser.Id, id);
            return _usersCollection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _usersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            var filter = filterBuilder.Eq(existingUser => existingUser.Email, email);
            return _usersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            var filter = filterBuilder.Eq(existingUser => existingUser.Id, id);
            return _usersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public Task UpdateUserAsync(User user)
        {
            var filter = filterBuilder.Eq(existingUser => existingUser.Id, user.Id);
            return _usersCollection.ReplaceOneAsync(filter, user);
        }
    }
}
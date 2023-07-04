using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ToDoBackend.Entities;

namespace ToDoBackend.Repositories
{
    public class ToDoRepository : IToDoRepository
    {

        private const string databaseName = "ToDoDB";
        private const string collectionName = "ToDos";
        private readonly IMongoCollection<ToDo> _toDosCollection;
        private readonly FilterDefinitionBuilder<ToDo> filterBuilder = Builders<ToDo>.Filter;

        public ToDoRepository(IMongoClient mongoClient)
        {
            IMongoDatabase? db = mongoClient.GetDatabase(databaseName);
            _toDosCollection = db.GetCollection<ToDo>(collectionName);
        }

        public async Task CompleteToDoAsync(Guid id, ToDo toDo)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.Id, id);
            await _toDosCollection.ReplaceOneAsync(filter, toDo);
        }

        public async Task CreateToDoAsync(ToDo toDo)
        {
            await _toDosCollection.InsertOneAsync(toDo);
        }

        public Task DeleteToDoAsync(Guid id)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.Id, id);
            return _toDosCollection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<ToDo>> GetAllToDosAsync()
        {
            return await _toDosCollection.Find(new BsonDocument()).ToListAsync();
        }

        public Task<ToDo> GetToDoByIdAsync(Guid id)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.Id, id);
            return _toDosCollection.Find(filter).SingleOrDefaultAsync();
        }

        public Task<ToDo> GetToDoByUserEmailAsync(string userEmail)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.userEmail, userEmail);
            return _toDosCollection.Find(filter).SingleOrDefaultAsync();
        }

        public Task UpdateToDoAsync(ToDo toDo)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.Id, toDo.Id);
            return _toDosCollection.ReplaceOneAsync(filter, toDo);
        }
    }
}
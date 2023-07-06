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
            var update = Builders<ToDo>.Update.Set(existingToDo => existingToDo.IsCompleted, toDo.IsCompleted);
            await _toDosCollection.UpdateOneAsync(filter, update);
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

        public Task<List<ToDo>> GetAllToDosByUserEmailAsync(string userEmail)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.UserEmail, userEmail);
            
            return _toDosCollection.Find(filter).ToListAsync();
        }

        public Task UpdateToDoAsync(Guid id, ToDo toDo)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.Id, id);
            var update = Builders<ToDo>.Update
                .Set(existingToDo => existingToDo.Title, toDo.Title);
            return _toDosCollection.UpdateOneAsync(filter, update);
        }
    }
}
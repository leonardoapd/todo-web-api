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

        private const string databaseName = "ToDoBD";
        private const string collectionName = "ToDos";
        private readonly IMongoCollection<ToDo> toDosCollection;
        private readonly FilterDefinitionBuilder<ToDo> filterBuilder = Builders<ToDo>.Filter;

        public ToDoRepository(IMongoClient mongoClient)
        {
            IMongoDatabase? db = mongoClient.GetDatabase(databaseName);
            toDosCollection = db.GetCollection<ToDo>(collectionName);
        }

        public async Task CompleteToDoAsync(Guid id, ToDo toDo)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.Id, id);
            await toDosCollection.ReplaceOneAsync(filter, toDo);
        }

        public async Task CreateToDoAsync(ToDo toDo)
        {
            await toDosCollection.InsertOneAsync(toDo);
        }

        public Task DeleteToDoAsync(Guid id)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.Id, id);
            return toDosCollection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<ToDo>> GetAllToDosAsync()
        {
            return await toDosCollection.Find(new BsonDocument()).ToListAsync();
        }

        public Task<ToDo> GetToDoByIdAsync(Guid id)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.Id, id);
            return toDosCollection.Find(filter).SingleOrDefaultAsync();
        }

        public Task<ToDo> GetToDoByUserEmailAsync(string userEmail)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.userEmail, userEmail);
            return toDosCollection.Find(filter).SingleOrDefaultAsync();
        }

        public Task UpdateToDoAsync(ToDo toDo)
        {
            var filter = filterBuilder.Eq(existingToDo => existingToDo.Id, toDo.Id);
            return toDosCollection.ReplaceOneAsync(filter, toDo);
        }
    }
}
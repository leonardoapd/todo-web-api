using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoBackend.Entities;

namespace ToDoBackend.Repositories
{
    public interface IToDoRepository
    {
        Task<IEnumerable<ToDo>> GetAllToDosAsync();
        Task<ToDo> GetToDoByIdAsync(Guid id);
        Task<ToDo> GetToDoByUserEmailAsync(string userEmail);
        Task CreateToDoAsync(ToDo toDo);
        Task UpdateToDoAsync(ToDo toDo);
        Task DeleteToDoAsync(Guid id);
        Task CompleteToDoAsync(Guid id, ToDo toDo);

    }
}
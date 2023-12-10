using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoBackend.DTOs.TodoDTOs
{
    public record GetTodosDTO
    {
        public required string Email { get; init; }
    }
}
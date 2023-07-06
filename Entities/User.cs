using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ToDoBackend.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public string? Name { get; set; }
        public required string Password { get; set; }

        // Navigation properties
        public ICollection<ToDo>? ToDos { get; set; }

    }
}
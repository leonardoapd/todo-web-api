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
        public string? Photo { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public required string Email { get; set; }
        public string? Password { get; set; }

        // Navigation properties
        public ICollection<ToDo>? ToDos { get; set; }

    }
}
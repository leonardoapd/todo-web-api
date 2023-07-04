using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ToDoBackend.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [JsonIgnore]
        public required string PasswordHash { get; set; }

        // Navigation properties
        public ICollection<ToDo>? ToDos { get; set; }

    }
}
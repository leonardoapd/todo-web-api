using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoBackend.Entities
{
    public class ToDo
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public bool IsCompleted { get; set; }

        // Navigation properties
        public string? UserEmail { get; set; }
    }
}
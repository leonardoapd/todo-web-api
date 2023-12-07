using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoBackend.DTOs.UserDTOs
{
    public class UserResponseDTO
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Photo { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
    }
}
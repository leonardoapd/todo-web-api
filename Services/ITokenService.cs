using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoBackend.Services
{
    public interface ITokenService
    {
        public string GenerateToken(string email);
        public string GetEmailFromToken(string token);
        public bool ValidateEmailFromToken(string token, string email);
    }
}
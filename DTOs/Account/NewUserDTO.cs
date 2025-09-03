using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Account
{
    public class NewUserDTO
    {
        public string? username { get; set; }
        public string? email { get; set; }
        public List<string>? roles { get; set; }
        public string? token { get; set; }
    }
}
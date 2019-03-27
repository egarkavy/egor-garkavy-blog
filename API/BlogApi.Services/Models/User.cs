using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApi.Services.Models
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Role2 { get; set; }
    }
}

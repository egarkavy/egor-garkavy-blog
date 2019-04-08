using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApi.Common.Tokens
{
    public class UserRefreshToken
    {
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
    }
}

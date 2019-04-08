using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApi.Common.Tokens
{
    public interface IRefreshTokenRepository
    {
        string Get(string username);
        void Delete(string username, string refreshToken);
        void Save(string username, string refreshToken);
    }
}

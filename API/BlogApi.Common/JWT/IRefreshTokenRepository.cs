using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApi.Common.JWT
{
    public interface IRefreshTokenRepository
    {
        void Get(string username);
        void Delete(string username, string refreshToken);
        void Save(string username, string refreshToken);
    }
}

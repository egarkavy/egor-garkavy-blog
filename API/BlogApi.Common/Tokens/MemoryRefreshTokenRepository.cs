using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogApi.Common.Tokens
{
    public class MemoryRefreshTokenRepository : IRefreshTokenRepository
    {
        private static BlockingCollection<UserRefreshToken> _userRefreshTokens;

        public void Delete(string username, string refreshToken)
        {
            var tokenToDelete = _userRefreshTokens.FirstOrDefault(x => x.UserName == username);

            if (tokenToDelete != null && tokenToDelete.RefreshToken == refreshToken)
            {
                _userRefreshTokens.TryTake(out tokenToDelete);
            }
        }

        public string Get(string username)
        {
            return _userRefreshTokens.FirstOrDefault(x => x.UserName == username)?.RefreshToken;
        }

        public void Save(string username, string refreshToken)
        {
            if (IsTokenExists(username))
            {
                throw new ArgumentException("There is already token for this user. You have to delete existing one first.");
            }

            _userRefreshTokens.Add(new UserRefreshToken
            {
                UserName = username,
                RefreshToken = refreshToken
            });
        }

        private bool IsTokenExists(string username)
        {
            var token = _userRefreshTokens.FirstOrDefault(x => x.UserName == username);

            return token != null;
        }
    }
}

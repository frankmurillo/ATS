using ATS.Controllers;
using ATS.Entities.Google;
using ATS.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ATS.Service.Google
{
    public class AuthRepository : BaseApiController, IDisposable
    {
        private UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _dbContext;

        public AuthRepository()
        {
            _dbContext = new ApplicationDbContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_dbContext));
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }

        public Client FindClient(string clientId)
        {
            var client = _dbContext.Clients.Find(clientId);

            return client;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        #region AddRefreshToken
        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = _dbContext.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _dbContext.RefreshTokens.Add(token);

            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region RemoveRefreshToken(String refreshTokenId)
        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _dbContext.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _dbContext.RefreshTokens.Remove(refreshToken);
                return await _dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }
        #endregion

        #region RemoveRefreshToken(RefreshToken refreshToken)
        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region FindRefreshToken
        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _dbContext.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }
        #endregion

        #region GetAllRefreshTokens
        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _dbContext.RefreshTokens.ToList();
        }
        #endregion

        public void Dispose()
        {
            _userManager.Dispose();
        }
    }
}
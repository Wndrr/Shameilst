using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApp.Data.Entities;

namespace WebApp.Data.Services
{
    public abstract class BaseDataProvider<T>
    {
        protected readonly ShameilstDbContext Context;
        private readonly UserManager<UserEntity> _userManager;
        private UserEntity _user;

        protected async Task<UserEntity> GetUser(ClaimsPrincipal claimsPrincipal)
        {
            return _user ??= await _userManager.GetUserAsync(claimsPrincipal);
        }

        protected BaseDataProvider(UserManager<UserEntity> userManager, ShameilstDbContext context)
        {
            _userManager = userManager;
            Context = context;
        }

        public abstract Task<T> Get(ClaimsPrincipal claimsPrincipal);
    }
}
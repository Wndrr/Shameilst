using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApp.Data.Entities;
using WebApp.Services;

namespace WebApp.Data.Services
{
    public abstract class BaseDataProvider
    {
        protected readonly ShameilstDbContext Context;
        private readonly UserManager<UserEntity> _userManager;
        private UserEntity _user;
        protected UiMessagingPipeline UiMessagingPipeline;

        protected async Task<UserEntity> GetUser(ClaimsPrincipal claimsPrincipal)
        {
            return _user ??= await _userManager.GetUserAsync(claimsPrincipal);
        }

        protected BaseDataProvider(UserManager<UserEntity> userManager, ShameilstDbContext context, UiMessagingPipeline uiMessagingPipeline)
        {
            _userManager = userManager;
            Context = context;
            UiMessagingPipeline = uiMessagingPipeline;
        }
    }
}
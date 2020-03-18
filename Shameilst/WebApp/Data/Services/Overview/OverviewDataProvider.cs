using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;
using WebApp.Services;

namespace WebApp.Data.Services.Overview
{
    public class OverviewDataProvider : BaseDataProvider
    {
        public OverviewDataProvider(UserManager<UserEntity> userManager, ShameilstDbContext context, UiMessagingPipeline uiMessagingPipeline) : base(userManager, context, uiMessagingPipeline)
        {
        }

        public async Task<OverviewModel> Get(ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var userAndRelatedEntities = await Context.Users.Include(u => u.Lists).ThenInclude(l => l.Tasks).SingleAsync(u => u.Id == user.Id);

                var overviewData = new OverviewModel(userAndRelatedEntities);
                return overviewData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
                return null;
            }
        }
    }
}
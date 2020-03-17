using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;

namespace WebApp.Data.Services.Overview
{
    public class OverviewDataProvider : BaseDataProvider<OverviewModel>
    {


        public OverviewDataProvider(UserManager<UserEntity> userManager, ShameilstDbContext context) : base(userManager, context)
        {
        }

        public override async Task<OverviewModel> Get(ClaimsPrincipal claimsPrincipal)
        {
            var user = await GetUser(claimsPrincipal);
            var userAndRelatedEntities = await Context.Users.Include(u => u.Lists).ThenInclude(l => l.Tasks).SingleAsync(u => u.Id == user.Id);

            var overviewData = new OverviewModel(userAndRelatedEntities);

            return overviewData;
        }
    }
}
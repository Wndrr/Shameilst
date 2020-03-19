using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;
using WebApp.Services;

namespace WebApp.Data.Services.Users
{
    public class UsersDataProvider : BaseDataProvider
    {

        public UsersDataProvider(UserManager<UserEntity> userManager, ShameilstDbContext context, UiMessagingPipeline uiMessagingPipeline) : base(userManager, context, uiMessagingPipeline)
        {
        }
        
        public async Task<List<ShareeModel>> GetAvailableShareesForList(ClaimsPrincipal claimsPrincipal, int listId)
        {
            try
            {    
                var user = await GetUser(claimsPrincipal);
                var idOfUsersAlreadySharedWith = await GetUsersAlreadyAddedAsShareesToThisList(listId, user);
                var allPossibleSharees = await GetUsersEligibleForShareeingEntities(user.Id, idOfUsersAlreadySharedWith);
                return allPossibleSharees.Select(s => new ShareeModel(s)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
                return null;
            }
        }

        private async Task<List<UserEntity>> GetUsersEligibleForShareeingEntities(string currentUserId, List<string> idOfUsersAlreadySharedWith)
        {
            var allPossibleSharees = await Context.Users.Where(u => !idOfUsersAlreadySharedWith.Contains(u.Id) && u.Id != currentUserId).ToListAsync();
            return allPossibleSharees;
        }

        private async Task<List<string>> GetUsersAlreadyAddedAsShareesToThisList(int listId, UserEntity user)
        {
            var userAndRelatedEntities = await Context.Users
                .Include(u => u.Lists).ThenInclude(l => l.Tasks)
                .Include(u => u.Lists).ThenInclude(l => l.Sharees)
                .SingleAsync(u => u.Id == user.Id);
            var currentList = userAndRelatedEntities.Lists.Single(l => l.Id == listId);
            var idOfUsersAlreadySharedWith = currentList.Sharees.Select(s => s.UserId).ToList();
            return idOfUsersAlreadySharedWith;
        }
    }
}
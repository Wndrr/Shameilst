using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;
using System.Threading.Tasks;
using WebApp.Services;

namespace WebApp.Data.Services.TaskListDataProvider
{
    public class TaskListDataProvider : BaseDataProvider
    {
        public TaskListDataProvider(UserManager<UserEntity> userManager, ShameilstDbContext context, UiMessagingPipeline uiMessagingPipeline) : base(userManager, context, uiMessagingPipeline)
        {
        }

        public async Task<TaskListForUserModel> Get(ClaimsPrincipal claimsPrincipal, int listId)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var userAndRelatedEntities = await Context.Users
                    .Include(u => u.Lists).ThenInclude(l => l.Tasks)
                    .Include(u => u.Lists).ThenInclude(l => l.Sharees).ThenInclude(s => s.User)
                    .SingleAsync(u => u.Id == user.Id);
                var list = userAndRelatedEntities.Lists.Single(l => l.Id == listId);
                var listsForUseModel = new TaskListForUserModel(list);

                return listsForUseModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
                return null;
            }
        }
        
        public async Task<TaskListForUserModel> GetAsSharee(ClaimsPrincipal claimsPrincipal, int listId)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var userAndRelatedEntities = await Context.Users
                    .Include(u => u.ListsSharedWithThisUser).ThenInclude(l => l.List).ThenInclude(l => l.Tasks)
                    .SingleAsync(u => u.Id == user.Id);
                var list = userAndRelatedEntities.ListsSharedWithThisUser.Single(l => l.ListId == listId);
                var listsForUseModel = new TaskListForUserModel(list.List);

                return listsForUseModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
                return null;
            }
        }

        public async System.Threading.Tasks.Task Add(ClaimsPrincipal claimsPrincipal, int listId, string name)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var currentUser = await Context.Users.Include(s => s.Lists).ThenInclude(l => l.Tasks).SingleAsync(u => u.Id == user.Id);
                var tasks = currentUser.Lists.Single(l => l.Id == listId).Tasks;
                var listToAdd = new TaskEntity(name);
                tasks.Add(listToAdd);

                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
            }
        }

        public async System.Threading.Tasks.Task Remove(ClaimsPrincipal claimsPrincipal, int listId, int idOfTaskToRemove)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var currentUser = await Context.Users.Include(s => s.Lists).ThenInclude(l => l.Tasks).SingleAsync(u => u.Id == user.Id);
                var tasks = currentUser.Lists.Single(l => l.Id == listId).Tasks;
                tasks.RemoveAll(r => r.Id == idOfTaskToRemove);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
            }
        }
        
        public async System.Threading.Tasks.Task RemoveSharee(ClaimsPrincipal claimsPrincipal, int listId, string idOfShareeToRemove)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var currentUser = await Context.Users.Include(s => s.Lists).ThenInclude(l => l.Tasks).SingleAsync(u => u.Id == user.Id);
                var sharees = currentUser.Lists.Single(l => l.Id == listId).Sharees;
                sharees.RemoveAll(r => r.UserId == idOfShareeToRemove);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
            }
        }

        public async System.Threading.Tasks.Task AddSharee(ClaimsPrincipal claimsPrincipal, int listId, string idOfShareeToAdd)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var currentUser = await Context.Users.Include(s => s.Lists).ThenInclude(l => l.Tasks).SingleAsync(u => u.Id == user.Id);
                var list = currentUser.Lists.Single(l => l.Id == listId);

                var shareeToAdd = await Context.Users.SingleAsync(u => u.Id == idOfShareeToAdd);
                var listShareeMappingToAdd = new ListShareeMappingEntity(shareeToAdd, list);
                list.Sharees.Add(listShareeMappingToAdd);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
            }
        }

        public async Task<bool> UserOwnsThisList(ClaimsPrincipal claimsPrincipal, int listId)
        { 
            try
            {
                var user = await GetUser(claimsPrincipal);
                var currentUser = await Context.Users
                    .Include(s => s.Lists).ThenInclude(l => l.Tasks)
                    .Include(l => l.ListsSharedWithThisUser)
                    .SingleAsync(u => u.Id == user.Id);
                var isListOwnedByCurrentUser = currentUser.Lists.Any(l => l.Id == listId);
                var isListSharedWithUser = currentUser.ListsSharedWithThisUser.Any(l => l.ListId == listId);

                if (isListOwnedByCurrentUser)
                    return true;

                if (isListSharedWithUser)
                    return false;
                
                throw new InvalidOperationException($"The list with id {listId} is neither owned by nor shared with user {user.UserName}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
                return false;
            }
        }
    }
}
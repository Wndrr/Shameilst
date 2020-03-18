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
                var userAndRelatedEntities = await Context.Users.Include(u => u.Lists).ThenInclude(l => l.Tasks).SingleAsync(u => u.Id == user.Id);
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
    }
}
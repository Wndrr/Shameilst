using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;
using WebApp.Services;

namespace WebApp.Data.Services.TaskLists
{
    public class TaskListsDataProvider : BaseDataProvider
    {
        public TaskListsDataProvider(UserManager<UserEntity> userManager, ShameilstDbContext context, UiMessagingPipeline uiMessagingPipeline) : base(userManager, context, uiMessagingPipeline)
        {
        }

        public async Task<TaskListsForUserModel> Get(ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var userAndRelatedEntities = await Context.Users.Include(u => u.Lists).ThenInclude(l => l.Tasks).SingleAsync(u => u.Id == user.Id);

                var listsForUseModel = new TaskListsForUserModel(userAndRelatedEntities);

                return listsForUseModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
                return null;
            }
        }

        public async System.Threading.Tasks.Task Add(ClaimsPrincipal claimsPrincipal, string name)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var currentUser = await Context.Users.Include(s => s.Lists).SingleAsync(u => u.Id == user.Id);
                var listToAdd = new TaskListEntity(name);
                currentUser.Lists.Add(listToAdd);

                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
            }
        }

        public async System.Threading.Tasks.Task Remove(ClaimsPrincipal claimsPrincipal, int idOfListToRemove)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var currentUser = await Context.Users.Include(s => s.Lists).SingleAsync(u => u.Id == user.Id);
                currentUser.Lists.RemoveAll(r => r.Id == idOfListToRemove);
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
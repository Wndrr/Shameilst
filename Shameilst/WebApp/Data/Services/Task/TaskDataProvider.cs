using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;
using WebApp.Data.Services.TaskLists;
using WebApp.Services;

namespace WebApp.Data.Services.Task
{
    public class TaskDataProvider : BaseDataProvider
    {
        public TaskDataProvider(UserManager<UserEntity> userManager, ShameilstDbContext context, UiMessagingPipeline uiMessagingPipeline) : base(userManager, context, uiMessagingPipeline)
        {
        }

        public async Task<int> GetParentListIdForTask(ClaimsPrincipal claimsPrincipal, int taskId)
        {
            try
            {
                var task = await Context.Tasks.Include(i => i.ParentList).SingleAsync(t => t.Id == taskId);
                var parentListId = task.ParentList.Id;

                return parentListId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
                return -1;
            }
        }

        public async Task<TaskForUserModel> Get(ClaimsPrincipal claimsPrincipal, int taskId)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var userAndRelatedEntities = await Context.Users.Include(u => u.Lists).ThenInclude(l => l.Tasks).ThenInclude(l => l.ParentList).SingleAsync(u => u.Id == user.Id);
                var task = userAndRelatedEntities.Lists.SelectMany(l => l.Tasks).Single(l => l.Id == taskId);

                var listsForUseModel = new TaskForUserModel(task);

                return listsForUseModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
                return null;
            }
        }
        
        public async Task<TaskForUserModel> GetShared(ClaimsPrincipal claimsPrincipal, int taskId)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var userAndRelatedEntities = await Context.Users.Include(u => u.ListsSharedWithThisUser).ThenInclude(l => l.List).ThenInclude(l => l.Tasks).SingleAsync(u => u.Id == user.Id);
                var task = userAndRelatedEntities.ListsSharedWithThisUser.SelectMany(l => l.List.Tasks).Single(l => l.Id == taskId);

                var listsForUseModel = new TaskForUserModel(task);

                return listsForUseModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
                return null;
            }
        }

        public async System.Threading.Tasks.Task Remove(ClaimsPrincipal claimsPrincipal, int idOfTaskToRemove)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var currentUser = await Context.Users.Include(s => s.Lists).SingleAsync(u => u.Id == user.Id);
                var task = currentUser.Lists.SelectMany(l => l.Tasks).Single(l => l.Id == idOfTaskToRemove);
                var list = currentUser.Lists.Single(l => l.Id == task.ParentList.Id);
                list.Tasks.RemoveAll(r => r.Id == idOfTaskToRemove);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                UiMessagingPipeline.AddUiMessageForUser(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), new UiMessage("An error occured"));
            }
        }

        public async System.Threading.Tasks.Task Update(ClaimsPrincipal claimsPrincipal, int idOfTaskToRemove, TaskForUserModel model)
        {
            try
            {
                var user = await GetUser(claimsPrincipal);
                var currentUser = await Context.Users.Include(s => s.Lists).SingleAsync(u => u.Id == user.Id);
                var task = currentUser.Lists.SelectMany(l => l.Tasks).Single(l => l.Id == idOfTaskToRemove);
                task.Update(model);
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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;

namespace WebApp.Data.Services.TaskLists
{
    public class TaskListsDataProvider : BaseDataProvider<TaskListsForUserModel>
    {
        public TaskListsDataProvider(UserManager<UserEntity> userManager, ShameilstDbContext context) : base(userManager, context)
        {
        }

        public override async Task<TaskListsForUserModel> Get(ClaimsPrincipal claimsPrincipal)
        {
            var user = await GetUser(claimsPrincipal);
            var userAndRelatedEntities = await Context.Users.Include(u => u.Lists).ThenInclude(l => l.Tasks).SingleAsync(u => u.Id == user.Id);
            
            var listsForUseModel = new TaskListsForUserModel(userAndRelatedEntities);

            return listsForUseModel;
        }

        public async Task Add(ClaimsPrincipal claimsPrincipal, string name)
        {
            var user = await GetUser(claimsPrincipal);
            var currentUser = await Context.Users.Include(s => s.Lists).SingleAsync(u => u.Id == user.Id);
            var listToAdd = new TaskListEntity(name);
            currentUser.Lists.Add(listToAdd);

            await Context.SaveChangesAsync();
        }

        public async Task Remove(ClaimsPrincipal claimsPrincipal, int idOfListToRemove)
        {
            var user = await GetUser(claimsPrincipal);
            var currentUser = await Context.Users.Include(s => s.Lists).SingleAsync(u => u.Id == user.Id);
            currentUser.Lists.RemoveAll(r => r.Id == idOfListToRemove);
            await Context.SaveChangesAsync();
        }
    }
}
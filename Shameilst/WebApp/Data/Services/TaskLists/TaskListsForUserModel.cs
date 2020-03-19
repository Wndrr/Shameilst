using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebApp.Data.Entities;

namespace WebApp.Data.Services.TaskLists
{
    public class TaskListsForUserModel
    {
        public TaskListsForUserModel(UserEntity data)
        {
            ListsOwnedByThisUser = data.Lists.Select(l => new TaskListModel(l));
            ListsSharedWithThisUser = data.ListsSharedWithThisUser.Select(l => new SharedTaskListModel(l.List));
            
        }

        public IEnumerable<TaskListModel> ListsOwnedByThisUser { get; set; }
        public IEnumerable<SharedTaskListModel> ListsSharedWithThisUser { get; set; }
    }
}
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
            Lists = data.Lists.Select(l => new TaskListModel(l));
        }
        
        public IEnumerable<TaskListModel> Lists { get; set; }
    }
}
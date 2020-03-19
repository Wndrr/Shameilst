using System.Collections.Generic;
using System.Linq;
using WebApp.Data.Entities;
using WebApp.Data.Services.Users;

namespace WebApp.Data.Services.TaskListDataProvider
{
    public class TaskListForUserModel
    {
        public TaskListForUserModel(TaskListEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Tasks = entity.Tasks.Select(t => new ListViewTaskModel(t)).ToList();
            Sharees = entity.Sharees.Select(s => new ShareeModel(s)).ToList();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<ListViewTaskModel> Tasks { get; set; }
        public List<ShareeModel> Sharees { get; set; }
    }
}
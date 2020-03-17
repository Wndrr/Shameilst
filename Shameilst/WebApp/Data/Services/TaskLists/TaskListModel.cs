using System.Linq;
using WebApp.Data.Entities;

namespace WebApp.Data.Services.TaskLists
{
    public class TaskListModel
    {
        public TaskListModel(TaskListEntity taskListEntity)
        {
            Id = taskListEntity.Id;
            Name = taskListEntity.Name;
            TotalTasksCount = taskListEntity.Tasks?.Count() ?? 0;
            OpenTasksCount = taskListEntity.Tasks?.Count(t => !t.IsClosed) ?? 0;
            ShareesCount = taskListEntity.Sharees?.Count() ?? 0;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalTasksCount { get; set; }
        public int OpenTasksCount { get; set; }
        public int ShareesCount { get; set; }
    }
}
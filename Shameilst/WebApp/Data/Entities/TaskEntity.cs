using System;
using WebApp.Data.Services.Task;
using WebApp.Data.Services.TaskLists;

namespace WebApp.Data.Entities
{
    public class TaskEntity
    {
        public TaskEntity()
        {
        }

        public TaskEntity(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual TaskListEntity ParentList { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now;
        public bool IsClosed { get; set; } = false;

        public void Update(TaskForUserModel model)
        {
            Id = model.Id;
            Name = model.Name;
            DueDate = model.DueDate;
            IsClosed = model.IsClosed;
        }
    }
}
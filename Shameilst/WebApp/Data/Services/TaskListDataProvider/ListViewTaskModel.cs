using System;
using WebApp.Data.Entities;

namespace WebApp.Data.Services.TaskListDataProvider
{
    public class ListViewTaskModel
    {
        public ListViewTaskModel(TaskEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            DueDate = entity.DueDate;
            IsClosed = entity.IsClosed;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        
        public DateTime DueDate { get; set; }
        public bool IsClosed { get; set; }
    }
}
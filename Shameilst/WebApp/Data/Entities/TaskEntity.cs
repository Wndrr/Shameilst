using System;

namespace WebApp.Data.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual TaskListEntity ParentList { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now;
        public bool IsClosed { get; set; } = false;
    }
}
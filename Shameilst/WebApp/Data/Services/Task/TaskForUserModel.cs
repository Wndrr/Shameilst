using System;
using System.ComponentModel.DataAnnotations;
using WebApp.Data.Entities;

namespace WebApp.Data.Services.Task
{
    public class TaskForUserModel
    {
        public TaskForUserModel(TaskEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            DueDate = entity.DueDate;
            IsClosed = entity.IsClosed;
            ParentListId = entity.ParentList.Id;
        }

        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public DateTime DueDate { get; set; }
        
        public bool IsClosed { get; set; }
        
        public int ParentListId { get; set; }
    }
}
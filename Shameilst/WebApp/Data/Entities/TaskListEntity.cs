using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Data.Entities
{
    public class TaskListEntity
    {
        public TaskListEntity()
        {
        }

        public TaskListEntity(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual UserEntity Owner { get; set; }
        public virtual List<ListShareeMappingEntity> Sharees { get; set; } = new List<ListShareeMappingEntity>();
        public virtual List<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}
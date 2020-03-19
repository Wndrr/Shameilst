using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Data.Entities
{
    public sealed class ListShareeMappingEntity
    {
        public ListShareeMappingEntity()
        {
        }

        public ListShareeMappingEntity(UserEntity user, TaskListEntity list)
        {
            if(user == null)
                throw new ArgumentNullException(nameof(user));
            
            if(list == null)
                throw new ArgumentNullException(nameof(list));
            
            UserId = user.Id;
            User = user;
            ListId = list.Id;
            List = list;
        }

        public string UserId { get; set; }
        public UserEntity User { get; set; }
        
        public int ListId { get; set; }
        public TaskListEntity List { get; set; }
    }
}
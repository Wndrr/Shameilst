using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Data.Entities
{
    public class UserEntity : IdentityUser
    {
        public virtual List<TaskListEntity> Lists { get; set; } 
    }
}
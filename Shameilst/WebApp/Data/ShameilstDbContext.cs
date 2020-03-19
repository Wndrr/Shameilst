using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;

namespace WebApp.Data
{
    public class ShameilstDbContext : IdentityDbContext<UserEntity>
    {
        public ShameilstDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserEntity>().HasMany(u => u.Lists).WithOne(l => l.Owner).IsRequired();
            base.OnModelCreating(builder);
        }

        public DbSet<TaskListEntity> Lists { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
    }
}
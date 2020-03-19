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
            builder.Entity<UserEntity>().HasMany(u => u.ListsSharedWithThisUser).WithOne(l => l.User).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
            
            
            builder.Entity<ListShareeMappingEntity>().HasKey(l => new {l.ListId, l.UserId});
            
            builder.Entity<ListShareeMappingEntity>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.ListsSharedWithThisUser)
                .HasForeignKey(pt => pt.UserId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            builder.Entity<ListShareeMappingEntity>()
                .HasOne(pt => pt.List)
                .WithMany(t => t.Sharees)
                .HasForeignKey(pt => pt.ListId)
                .OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(builder);
        }

        public DbSet<TaskListEntity> Lists { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<TaskEntity> ListShareeMappingEntity { get; set; }
    }
}
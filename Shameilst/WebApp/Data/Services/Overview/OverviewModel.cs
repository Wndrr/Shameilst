using System;
using System.Linq;
using WebApp.Data.Entities;
// ReSharper disable PossibleMultipleEnumeration

namespace WebApp.Data.Services.Overview
{
    public class OverviewModel
    {
        public OverviewModel(UserEntity userAndRelatedEntities)
        {
            var tasks = userAndRelatedEntities.Lists.SelectMany(s => s.Tasks);
            var taskWithDateInThePast = tasks.Where(t => t.DueDate < DateTime.Today);
            var overdueTasks = taskWithDateInThePast.Where(t => !t.IsClosed);
            TasksDueTodayCount = overdueTasks.Count();
            TasksOverdueCount = tasks.Count(t => t.DueDate < DateTime.Today  && !t.IsClosed);
            TotalListsCount = userAndRelatedEntities.Lists.Count();
            TotalOpenTasksCount = tasks.Count(t => !t.IsClosed);
            ListsSharedWithThisUserCount = userAndRelatedEntities.ListsSharedWithThisUser.Count();
            
            // Add 1 to both to ensure we don't divide by zero !
            var shameValue = overdueTasks.Sum(t => t.PrideShameValue) + 1;
            var prideValue = taskWithDateInThePast.Where(t => !t.IsClosed).Sum(t => t.PrideShameValue) + 1;
            PrideShameRatioValue = prideValue / shameValue;
        }

        public int TasksDueTodayCount { get; set; }
        public int TasksOverdueCount { get; set; }
        public int PrideShameRatioValue { get; set; }
        public int Progression { get; set; }
        public int TotalListsCount { get; set; }
        public int TotalOpenTasksCount { get; set; }
        public int ListsSharedWithThisUserCount { get; set; }
    }
}
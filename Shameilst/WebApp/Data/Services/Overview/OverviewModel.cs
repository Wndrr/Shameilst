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
            TasksDueTodayCount = tasks.Count(t => t.DueDate > DateTime.Today && t.DueDate < DateTime.Today.AddDays(1) && !t.IsClosed);
            TasksOverdueCount = tasks.Count(t => t.DueDate < DateTime.Today  && !t.IsClosed);
            TotalListsCount = userAndRelatedEntities.Lists.Count();
            TotalOpenTasksCount = tasks.Count(t => !t.IsClosed);
            ListsSharedWithThisUserCount = userAndRelatedEntities.ListsSharedWithThisUser.Count();
        }

        public int TasksDueTodayCount { get; set; }
        public int TasksOverdueCount { get; set; }
        public int ShameRatioValue { get; set; }
        public int Progression { get; set; }
        public int TotalListsCount { get; set; }
        public int TotalOpenTasksCount { get; set; }
        public int ListsSharedWithThisUserCount { get; set; }
    }
}
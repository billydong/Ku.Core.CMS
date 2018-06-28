﻿using Ku.Core.TimedTask.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ku.Core.CMS.Data.EntityFramework
{
    public partial class KuDbContext : DbContext, IDbContext, ITimedTaskContext
    {
        public KuDbContext(DbContextOptions<KuDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //创建UserRole关系
            modelBuilder.Entity<Domain.Entity.UserCenter.UserRole>().HasKey(t => new { t.UserId, t.RoleId });
            //创建RoleFunction关系
            modelBuilder.Entity<Domain.Entity.UserCenter.RoleFunction>().HasKey(t => new { t.RoleId, t.FunctionId });

            //菜单
            modelBuilder.Entity<Domain.Entity.System.Menu>().HasMany(m => m.SubMenus).WithOne(m => m.Parent);

            modelBuilder.Entity<Domain.Entity.Mall.BrandCategoryRef>().HasKey(t => new { t.BrandId, t.ProductCategoryId });

            base.OnModelCreating(modelBuilder);

            //定时任务相关
            modelBuilder.SetupTimedTask();
        }

        /// <summary>
        /// 异步保存
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        //定时任务相关
        public DbSet<TimedTask.TimedTask> TimedTasks { get; set; }
        public DbSet<TimedTask.TimedTaskLog> TimedTaskLogs { get; set; }
    }
}

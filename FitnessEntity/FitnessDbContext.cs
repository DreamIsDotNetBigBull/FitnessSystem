using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessEntity
{
    public class FitnessDbContext : DbContext
    {
        public FitnessDbContext(DbContextOptions<FitnessDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating( ModelBuilder modelBuilder)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //配置连接字符串
            //optionsBuilder.UseSqlServer(@"Server=.;Database=Test;Trusted_Connection=True;");

            //打印ef执行sql
            //optionsBuilder.LogTo(Console.WriteLine);
        }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<AttendanceInfo> AttendanceInfo { get; set; }
        public DbSet<RechargeInfo> RechargeInfo { get; set; }
        public DbSet<GoodsInfo> GoodsInfo { get; set; }
        public DbSet<GoodsRecord> GoodsRecord { get; set; }
        public DbSet<GoodsType> GoodsType { get; set; }
        public DbSet<RoleInfo> RoleInfo { get; set; }
        public DbSet<MenuInfo> MenuInfo { get; set; }
        public DbSet<R_RoleInfo_MenuInfo> R_RoleInfo_MenuInfo { get; set; }
        public DbSet<R_UserInfo_RoleInfo> R_UserInfo_RoleInfo { get; set; }
        public DbSet<FileInfo> FileInfo { get; set; }
        public DbSet<CourseInfo> CourseInfo { get; set; }
        public DbSet<CourseTypeInfo> CourseTypeInfo { get; set; }
        public DbSet<Coursedetailed> Coursedetailed { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}

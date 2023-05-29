using LichTruc.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffHasPermission> StaffHasPermissions { get; set; }
        public DbSet<StaffHasRole> StaffHasRoles { get; set; }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Shifts> Shifts { get; set; }
        public DbSet<ShiftsType> ShiftsTypes { get; set; }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleHasPermission> RoleHasPermissions { get; set; }

        public DbSet<PickList> PickLists { get; set; }
        public DbSet<PickListMenu> PickListMenus { get; set; }

        public DbSet<Schedule> Schedules { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>().ToTable("Area");
            modelBuilder.Entity<PickList>().ToTable("PickList");
            modelBuilder.Entity<ShiftsType>().ToTable("ShiftsType");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<Area>().HasIndex(sc => sc.Id).IsUnique();
            modelBuilder.Entity<Area>().HasQueryFilter(x => x.DeletedAt == null);

            modelBuilder.Entity<Role>().HasIndex(sc => sc.name).IsUnique();
            modelBuilder.Entity<Role>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<RoleHasPermission>().HasKey(sc => new { sc.roleId, sc.PermissionId });

            modelBuilder.Entity<Permission>().HasIndex(sc => sc.code).IsUnique();
            modelBuilder.Entity<PermissionGroup>().Property(b => b.hasPermission).HasDefaultValue(false);

            modelBuilder.Entity<Staff>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<StaffHasPermission>().HasKey(sc => new {sc.staffId, sc.permissionId});

            modelBuilder.Entity<PickList>().Property(sc => sc.Used).HasDefaultValue(true);

            modelBuilder.Entity<Shifts>()
            .HasOne<Staff>(a => a.Staff)
            .WithMany(s => s.Shifts)
            .HasForeignKey(a => a.Id);

            modelBuilder.Entity<Shifts>()
            .HasOne<Area>(a => a.Area)
            .WithMany(s => s.Shifts)
            .HasForeignKey(a => a.Id);

            // Cấu hình các mối quan hệ giữa các bảng trong cơ sở dữ liệu
            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Area)
                .WithMany(a => a.Staff)
                .HasForeignKey(s => s.areaId);

            modelBuilder.Entity<StaffHasRole>()
          .HasKey(sr => new { sr.staffId, sr.roleId });

            modelBuilder.Entity<StaffHasRole>()
                .HasOne(sr => sr.Staff)
                .WithMany(s => s.StaffHasRoles)
                .HasForeignKey(sr => sr.staffId);

            modelBuilder.Entity<StaffHasRole>()
                .HasOne(sr => sr.Role)
                .WithMany(r => r.StaffHasRoles)
                .HasForeignKey(sr => sr.roleId);

            modelBuilder.Entity<StaffHasArea>()
                .HasKey(sa => new { sa.staffId, sa.areaId });

            modelBuilder.Entity<StaffHasArea>()
                .HasOne(sa => sa.Staff)
                .WithMany(s => s.StaffHasAreas)
                .HasForeignKey(sa => sa.staffId);

            modelBuilder.Entity<StaffHasArea>()
                .HasOne(sa => sa.Area)
                .WithMany(a => a.StaffHasAreas)
                .HasForeignKey(sa => sa.areaId);

        }

    }
}

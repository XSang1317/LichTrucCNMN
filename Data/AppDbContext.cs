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
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<Area>().HasIndex(sc => sc.Name).IsUnique();
            modelBuilder.Entity<Area>().HasQueryFilter(x => x.DeletedAt == null);

            modelBuilder.Entity<Role>().HasIndex(sc => sc.name).IsUnique();
            modelBuilder.Entity<Role>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<RoleHasPermission>().HasKey(sc => new { sc.roleId, sc.PermissionId });

            modelBuilder.Entity<Permission>().HasIndex(sc => sc.code).IsUnique();
            modelBuilder.Entity<PermissionGroup>().Property(b => b.hasPermission).HasDefaultValue(false);

            modelBuilder.Entity<Staff>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<StaffHasRole>().HasKey(sc => new { sc.staffId, sc.roleId });
            modelBuilder.Entity<StaffHasPermission>().HasKey(sc => new {sc.staffId, sc.permissionId});

            modelBuilder.Entity<PickList>().Property(sc => sc.Used).HasDefaultValue(true);

            modelBuilder.Entity<Staff>()
            .HasOne(s => s.Area)
            .WithMany(a => a.Staff)
            .HasForeignKey(s => s.areaId);

            modelBuilder.Entity<Shifts>()
            .HasOne<Staff>(a => a.Staff)
            .WithMany(s => s.Shifts)
            .HasForeignKey(a => a.Id);

            modelBuilder.Entity<Shifts>()
            .HasOne<Area>(a => a.Area)
            .WithMany(s => s.Shifts)
            .HasForeignKey(a => a.Id);

        }

    }
}

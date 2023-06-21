using Microsoft.EntityFrameworkCore;
using API.Models.Tables;
using API.Models;

namespace API.Data;

public class BookingDBContext : DbContext
{
    public BookingDBContext(DbContextOptions<BookingDBContext> options) : base(options)
    {

    }

    // Table
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<University> Universities { get; set;}
    public DbSet<Role> Roles { get; set; }
    public DbSet<Room> Rooms { get; set; }

    // Ohter Configuration or Fluent API

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Constrains Unique
        modelBuilder.Entity<BaseEntity>()
            .HasIndex(base_entity => base_entity.Guid).IsUnique();

        modelBuilder.Entity<Booking>()
            .HasIndex(booking => new
            {
                booking.RoomGuid,
                booking.EmployeeGuid
            }).IsUnique();

        modelBuilder.Entity<AccountRole>()
            .HasIndex(account_role => new
            {
                account_role.AccountGuid,
                account_role.RoleGuid
            }).IsUnique();

        modelBuilder.Entity<Employee>()
            .HasIndex(employee => new
            {
                employee.NIK,
                employee.Email,
                employee.PhoneNumber
            }).IsUnique();

        modelBuilder.Entity<Education>()
            .HasIndex(education => education.UniversityGuid).IsUnique();

        //Relationship

        // Role - Account Role (One to Many)
        modelBuilder.Entity<Role>()
            .HasMany(role => role.AccountRoles)
            .WithOne(account_role => account_role.Role)
            .HasForeignKey(account_role => account_role.RoleGuid);

        // Account - Account Role (One to Many)
        modelBuilder.Entity<Account>()
            .HasMany(account => account.AccountRoles)
            .WithOne(account_role => account_role.Account)
            .HasForeignKey(account_role => account_role.AccountGuid);

        // Employee - Account (One to One)
        modelBuilder.Entity<Employee>()
            .HasOne(employee => employee.Account)
            .WithOne(account => account.Employee)
            .HasForeignKey<Account>(account => account.Guid);

        // Employee - Booking (One to Many)
        modelBuilder.Entity<Employee>()
            .HasMany(employee => employee.Bookings)
            .WithOne(booking => booking.Employee)
            .HasForeignKey(booking => booking.EmployeeGuid);

        // Employee - Education (One to One)
        modelBuilder.Entity<Employee>()
            .HasOne(employee => employee.Education)
            .WithOne(education => education.Employee)
            .HasForeignKey<Education>(education => education.Guid);

        // University - Education (One to Many)
        modelBuilder.Entity<University>()
            .HasMany(university => university.Educations)
            .WithOne(education => education.University)
            .HasForeignKey(education => education.UniversityGuid);

        // Room - Booking (One to Many)
        modelBuilder.Entity<Room>()
            .HasMany(room => room.Bookings)
            .WithOne(booking => booking.Room)
            .HasForeignKey(booking => booking.RoomGuid);
    }


}

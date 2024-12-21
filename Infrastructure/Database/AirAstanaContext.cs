using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public class AirAstanaContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Flight> Flights { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        public AirAstanaContext(DbContextOptions<AirAstanaContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.Id);
                user.Property(u => u.Id).IsRequired();
                user.Property(u => u.Id).ValueGeneratedOnAdd();
                user.Property(u => u.Username).HasMaxLength(256).IsRequired();
                user.Property(u => u.Password).HasMaxLength(256).IsRequired();
                user.Property(u => u.Salt).HasMaxLength(256).IsRequired();
                user.Property(u => u.RoleId).IsRequired();
                user.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);

                user.HasIndex(u => u.Username).IsUnique(true);
            });

            modelBuilder.Entity<Flight>(flight =>
            {
                flight.HasKey(f => f.Id);
                flight.Property(f => f.Id).IsRequired();
                flight.Property(f => f.Id).ValueGeneratedOnAdd();
                flight.Property(f => f.Destination).HasMaxLength(256).IsRequired();
                flight.Property(f => f.Origin).HasMaxLength(256).IsRequired();
                flight.Property(f => f.Departure).IsRequired();
                flight.Property(f => f.Arrival).IsRequired();
                flight.Property(f => f.Status).IsRequired().HasConversion<string>(
                    s => s.ToString(),
                    s => (FlightStatus)Enum.Parse(typeof(FlightStatus), s));
            });

            modelBuilder.Entity<Role>(role =>
            {
                role.HasKey(r => r.Id);
                role.Property(r => r.Id).IsRequired();
                role.Property(r => r.Id).ValueGeneratedOnAdd();
                role.Property(r => r.Code).HasMaxLength(256).IsRequired();
                role.HasData(
                    new Role() { Id = 1, Code = RoleNames.Moderator },
                    new Role() { Id = 2, Code = RoleNames.User });
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

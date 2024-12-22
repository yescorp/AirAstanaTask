using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
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
        private ILogger _logger;
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Flight> Flights { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        public AirAstanaContext(DbContextOptions<AirAstanaContext> options, ILogger<AirAstanaContext> logger)
            : base(options)
        {
            _logger = logger;
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

        public override int SaveChanges()
        {
            LogChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            LogChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void LogChanges()
        {
            var changes = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

            foreach (var entry in changes)
            {
                var entityName = entry.Entity.GetType().Name;
                var state = entry.State.ToString();

                if (entry.State == EntityState.Modified)
                {
                    var originalValues = entry.OriginalValues.Properties
                        .ToDictionary(p => p.Name, p => entry.OriginalValues[p.Name]?.ToString());

                    var currentValues = entry.CurrentValues.Properties
                        .ToDictionary(p => p.Name, p => entry.CurrentValues[p.Name]?.ToString());

                    _logger.LogInformation("Entity {EntityName} modified. Original: {@OriginalValues}, Current: {@CurrentValues}",
                        entityName, originalValues, currentValues);
                }
                else if (entry.State == EntityState.Added)
                {
                    var newValues = entry.CurrentValues.Properties
                        .ToDictionary(p => p.Name, p => entry.CurrentValues[p.Name]?.ToString());

                    _logger.LogInformation("Entity {EntityName} added. Values: {@NewValues}", entityName, newValues);
                }
                else if (entry.State == EntityState.Deleted)
                {
                    var deletedValues = entry.OriginalValues.Properties
                        .ToDictionary(p => p.Name, p => entry.OriginalValues[p.Name]?.ToString());

                    _logger.LogInformation("Entity {EntityName} deleted. Values: {@DeletedValues}", entityName, deletedValues);
                }
            }
        }
    }
}

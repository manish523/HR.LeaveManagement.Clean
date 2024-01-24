using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Domain.Common;
using HR.LeaveManagement.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.DatabaseContext;

public class HrDatabaseContext : DbContext
{
    private readonly IUserService _userService;

    public HrDatabaseContext(DbContextOptions<HrDatabaseContext> options, IUserService userService) : base(options)
    {
        _userService = userService;
    }

    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Different ways of applying entity type configuration

        // 1- This will look for all configurations and will apply them automatically
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(HrDatabaseContext).Assembly);

        // 2- This is the way where we need to register all separately written configurations one by one
        //modelBuilder.ApplyConfiguration(new LeaveTypeConfiguration());

        // 3- We don't need this type of configuration all at once.
        //    we have to write configuration for all entities here 1 by 1
        //    We don't need it if we are writing configurations in separate files like we are doing for this project
        //modelBuilder.Entity<LeaveType>().HasData(
        //    new LeaveType
        //    {
        //        Id = 1,
        //        Name = "Vacation",
        //        DefaultDays = 10,
        //        DateCreated = DateTime.Now,
        //        DateModified = DateTime.Now,
        //    }
        //);

        // This when runs looks for all configurations be it of any type and executes them
        //base.OnModelCreating(modelBuilder);

        // So this is the most cleaner and preferred way of doing it and seed data properly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HrDatabaseContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
            .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.DateModified = DateTime.Now;
            entry.Entity.ModifiedBy = _userService.UserId;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
                entry.Entity.CreatedBy = _userService.UserId;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

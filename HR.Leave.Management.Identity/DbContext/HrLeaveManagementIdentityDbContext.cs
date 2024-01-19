using HR.Leave.Management.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Leave.Management.Identity.DbContext
{
    public class HrLeaveManagementIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public HrLeaveManagementIdentityDbContext(
            DbContextOptions<HrLeaveManagementIdentityDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(HrLeaveManagementIdentityDbContext).Assembly);
        }
    }
}

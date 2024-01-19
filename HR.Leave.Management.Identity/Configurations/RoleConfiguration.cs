using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Leave.Management.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "d73dee98-08d7-466d-8303-f7670e5e9172",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },
                new IdentityRole
                {
                    Id = "3a5689e0-4184-453b-914a-68b73a90f1d1",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
            );

        }
    }
}

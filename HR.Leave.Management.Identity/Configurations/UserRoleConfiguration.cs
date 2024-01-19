using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Leave.Management.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "3a5689e0-4184-453b-914a-68b73a90f1d1",
                    UserId = "040e29bf-356c-4022-a2c5-d36427246d74"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "d73dee98-08d7-466d-8303-f7670e5e9172",
                    UserId = "30286fd8-4d2f-4fa3-99ae-eb9e162bb4ef"
                }
            );
        }
    }
}

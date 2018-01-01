using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace GatesmanStorage.Service.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }


    internal class ApplicationUserMap
    {
        public ApplicationUserMap(EntityTypeBuilder<ApplicationUser> entityBuilder)
        {
            entityBuilder.Property(t => t.Name).IsRequired();
        }
    }
}

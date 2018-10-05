using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace rentalBackEnd_Web_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string   fullName       { get; set; }
        public DateTime dateOfBirth    { get; set; }
        public string   sex            { get; set; }
        public byte[]   profilePicture { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection" , throwIfV1Schema:false)
        {
        }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //AspNetUsers => Users
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            //AspNetRoles => Roles
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            //AspNetUserRoles => UserRoles
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            //AspNetUserClaims => UserClaims
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            //AspNetUserLogin => UserLogin
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");

        }
    }
}
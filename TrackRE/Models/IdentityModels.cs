using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TrackRE.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<PropertyRE> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // https://articles.runtings.co.uk/2014/12/solved-aspnet-identity-2-throws.html
            /*
             *  Solves this issue: 
             *  
                Exception Details: System.Data.Entity.ModelConfiguration.ModelValidationException: One or more validation errors were detected during model generation:
                TrackRE.Models.IdentityUserLogin: : EntityType 'IdentityUserLogin' has no key defined. Define the key for this EntityType.
                TrackRE.Models.IdentityUserRole: : EntityType 'IdentityUserRole' has no key defined. Define the key for this EntityType.
                IdentityUserLogins: EntityType: EntitySet 'IdentityUserLogins' is based on type 'IdentityUserLogin' that has no keys defined.
                IdentityUserRoles: EntityType: EntitySet 'IdentityUserRoles' is based on type 'IdentityUserRole' that has no keys defined.            
             */
            base.OnModelCreating(modelBuilder);
        }       
    }
}
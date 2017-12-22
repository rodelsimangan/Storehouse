using Microsoft.AspNet.Identity.EntityFramework;

namespace StorehouseAdmin.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }
    public class ApplicationRole : IdentityRole
    { }
    public class ApplicationUserRole : IdentityUserRole { }
    public class UsersDbContext : IdentityDbContext<ApplicationUser>
    {
        public UsersDbContext()
            : base("StorehouseDBContext")
        {
        }
    }
}
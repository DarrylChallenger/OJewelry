using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using OJewelry.Models;
using OJewelry.Classes;
using Owin;
using System.Diagnostics;

[assembly: OwinStartupAttribute(typeof(OJewelry.Startup))]
namespace OJewelry
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Trace.TraceError("Starting...");
            ConfigureAuth(app);
            createRolesandUsers();
            ConfigureStorage();
        }

        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "Darryl";
                user.Email = "Darryl.Challenger@Outlook.com";
                string userPWD = "Administrator1!";
                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
                var user2 = new ApplicationUser();
                user2.UserName = "Olivia";
                user2.Email = "Olee65@gmail.com";
                userPWD = "Administrator1!";
                chkUser = UserManager.Create(user2, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user2.Id, "Admin");

                }
            }

            // Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);
            }

            // Creating User role    
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }

            // Creating Guest role    
            if (!roleManager.RoleExists("Guest"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Guest";
                roleManager.Create(role);
            }
        }

        private void ConfigureStorage()
        {
            string ojStoreConnStr = ConfigurationManager.AppSettings["StorageConnectionString"];
            Singletons.azureBlobStorage.Init(ojStoreConnStr, "ojewelry");
        }
    }
}

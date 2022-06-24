using KGB_Dev_.Data.KGB_Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KGB_Dev_.Data
{
    public class ApplicationDbContext : IdentityDbContext<KGB_User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<KGB_User> KGB_Users => Set<KGB_User>();
        public DbSet<KGB_Knowledge> KGB_Knowledge => Set<KGB_Knowledge>();
        public DbSet<KGB_Oj> KGB_OrgJed => Set<KGB_Oj>();
        public DbSet<KGB_Role> KGB_Role => Set<KGB_Role>();
        public DbSet<KGB_Category> KGB_Category => Set<KGB_Category>();
        public DbSet<KGB_Subcategory> KGB_Subcategory => Set<KGB_Subcategory>();




    }
}
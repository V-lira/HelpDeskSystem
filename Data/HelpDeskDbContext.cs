//using HelpDeskSystem.Models;
//using System.Data.Entity;

//namespace HelpDeskSystem.Data
//{
//    public class HelpDeskDbContext : DbContext
//    {
//        public HelpDeskDbContext() : base("HelpDeskConnection")
//        {
//        }
//        public DbSet<Incident> Incidents { get; set; }
//    }
//}
using HelpDeskSystem.Models;
using System.Data.Entity;

namespace HelpDeskSystem.Data
{
    public class HelpDeskDbContext : DbContext
    {
        public HelpDeskDbContext()
            : base("HelpDeskConnection")
        {
             //Database.SetInitializer(new CreateDatabaseIfNotExists<HelpDeskDbContext>());
            // Database.SetInitializer(new HelpDeskDbInitializer());
        }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentComment> Comments
        {
            get;
            set;
        }
        public DbSet<User> Users { get; set; }
        
    }
}
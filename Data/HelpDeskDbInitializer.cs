using System.Data.Entity;

namespace HelpDeskSystem.Data
{
    public class HelpDeskDbInitializer
        : CreateDatabaseIfNotExists<HelpDeskDbContext>
    {
    }
}
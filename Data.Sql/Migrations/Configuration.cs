using Data.Sql.Models;

namespace Data.Sql.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PiiaDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PiiaDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            SeedConfiguration(context);
        }

        private void SeedConfiguration(PiiaDb context)
        {
            ConfigurationAddIfNotExists(context, new Models.Configuration {
                ConfigurationID = ConfigurationKeys.RandomizationLocked,
                ConfigurationKey = "Randomization Locked",
                ConfigurationValue = "1"
                });
            ConfigurationAddIfNotExists(context, new Models.Configuration
            {
                ConfigurationID = ConfigurationKeys.AdminPassword,
                ConfigurationKey = "Admin Password Hash",
                ConfigurationValue = "TODO: create something here"
            });
        }

        private void ConfigurationAddIfNotExists(PiiaDb db, Models.Configuration config)
        {
            if (!db.Configurations.Any(x => x.ConfigurationID == config.ConfigurationID))
            {
                db.Configurations.Add(config);
            }
        }
    }
}

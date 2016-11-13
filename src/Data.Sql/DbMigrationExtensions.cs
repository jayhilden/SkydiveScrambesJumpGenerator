using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Sql
{
    public static class DbMigrationExtensions
    {
        public enum ProviderTypesEnum
        {
            SqlServer,
            MySQL
        }
        public static ProviderTypesEnum GetProvider()
        {
            var providerName = ConfigurationManager.ConnectionStrings["piia"].ProviderName;
            return providerName.ToLower().Contains("mysql") ? ProviderTypesEnum.MySQL : ProviderTypesEnum.SqlServer;
        }
    }
}

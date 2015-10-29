using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Sql.Models;

namespace Data.Sql
{
    public class PiiaDb : DbContext
    {
        public DbSet<Jumper> Jumpers { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<RoundJumperMap> RoundJumperMaps { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

        static PiiaDb()
        {
            Database.SetInitializer<PiiaDb>(null);
        }

        public PiiaDb()
            : base("Name=piia")
        {
        }

        public int? GetCommandTimeout()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
        }

        public void SetCommandTimeout(int? timeoutInSeconds)
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeoutInSeconds;
        }

        public void ResetCommandTimeout()
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = null;
        }

        public DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sql, parameters);
        }

        public DbRawSqlQuery<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return Database.SqlQuery<TElement>(sql, parameters);
        }

        public Task<T> SingleOrDefaultAsync<T>(string sql, params object[] parameters)
        {
            return Database.SqlQuery<T>(sql, parameters).SingleOrDefaultAsync();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}

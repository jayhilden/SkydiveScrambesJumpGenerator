namespace Data.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigurationTable_24 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Configuration",
                c => new
                    {
                        ConfigurationID = c.Int(nullable: false),
                        ConfigurationKey = c.String(),
                        ConfigurationValue = c.String(),
                    })
                .PrimaryKey(t => t.ConfigurationID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Configuration");
        }
    }
}

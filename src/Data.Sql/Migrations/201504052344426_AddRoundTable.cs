namespace Data.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoundTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Round",
                c => new
                    {
                        RoundID = c.Int(nullable: false, identity: true),
                        RoundNumber = c.Int(nullable: false),
                        Formations = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.RoundID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Round");
        }
    }
}

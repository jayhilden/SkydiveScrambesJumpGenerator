namespace Data.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoundJumperMap : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoundJumperMap",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoundID = c.Int(nullable: false),
                        UpJumper1ID = c.Int(nullable: false),
                        UpJumper2ID = c.Int(nullable: false),
                        DownJumper1ID = c.Int(nullable: false),
                        DownJumper2ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Jumper", t => t.DownJumper1ID)
                .ForeignKey("dbo.Jumper", t => t.DownJumper2ID)
                .ForeignKey("dbo.Round", t => t.RoundID)
                .ForeignKey("dbo.Jumper", t => t.UpJumper1ID)
                .ForeignKey("dbo.Jumper", t => t.UpJumper2ID)
                .Index(t => t.RoundID)
                .Index(t => t.UpJumper1ID)
                .Index(t => t.UpJumper2ID)
                .Index(t => t.DownJumper1ID)
                .Index(t => t.DownJumper2ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoundJumperMap", "UpJumper2ID", "dbo.Jumper");
            DropForeignKey("dbo.RoundJumperMap", "UpJumper1ID", "dbo.Jumper");
            DropForeignKey("dbo.RoundJumperMap", "RoundID", "dbo.Round");
            DropForeignKey("dbo.RoundJumperMap", "DownJumper2ID", "dbo.Jumper");
            DropForeignKey("dbo.RoundJumperMap", "DownJumper1ID", "dbo.Jumper");
            DropIndex("dbo.RoundJumperMap", new[] { "DownJumper2ID" });
            DropIndex("dbo.RoundJumperMap", new[] { "DownJumper1ID" });
            DropIndex("dbo.RoundJumperMap", new[] { "UpJumper2ID" });
            DropIndex("dbo.RoundJumperMap", new[] { "UpJumper1ID" });
            DropIndex("dbo.RoundJumperMap", new[] { "RoundID" });
            DropTable("dbo.RoundJumperMap");
        }
    }
}

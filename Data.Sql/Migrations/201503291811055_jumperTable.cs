namespace Data.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jumperTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jumper",
                c => new
                    {
                        JumperID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        NumberOfJumps = c.Int(nullable: false),
                        Organizer = c.Boolean(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        Comment = c.String(),
                        RandomizedUpDown = c.Int(),
                        RandomizedLetter = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.JumperID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Jumper");
        }
    }
}

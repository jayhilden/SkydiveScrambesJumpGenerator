namespace Data.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        NumberOfJumps = c.Int(nullable: false),
                        Organizer = c.Boolean(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        Comment = c.String(),
                        RandomizedUpDown = c.Int(),
                        RandomizedLetter = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
        }
    }
}

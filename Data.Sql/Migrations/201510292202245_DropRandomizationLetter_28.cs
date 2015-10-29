namespace Data.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropRandomizationLetter_28 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Jumper", "RandomizedLetter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jumper", "RandomizedLetter", c => c.String(maxLength: 1));
        }
    }
}

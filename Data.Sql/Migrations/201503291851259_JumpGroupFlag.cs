namespace Data.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JumpGroupFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jumper", "JumpGroup", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jumper", "JumpGroup");
        }
    }
}

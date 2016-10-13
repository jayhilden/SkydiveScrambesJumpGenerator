namespace Data.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoundJumperMapGroupFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoundJumperMap", "JumpGroup", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoundJumperMap", "JumpGroup");
        }
    }
}

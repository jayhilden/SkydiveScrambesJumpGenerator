namespace Data.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JumpScore_17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoundJumperMap", "Score", c => c.Int());
            AddColumn("dbo.RoundJumperMap", "Camera", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoundJumperMap", "Camera");
            DropColumn("dbo.RoundJumperMap", "Score");
        }
    }
}

namespace Data.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoundJumperMap", "VideoUrl", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoundJumperMap", "VideoUrl");
        }
    }
}

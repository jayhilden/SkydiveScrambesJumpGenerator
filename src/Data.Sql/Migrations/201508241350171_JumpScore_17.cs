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
            var provider = this.GetProvider();
            switch (provider)
            {
                case DbMigrationExtensions.ProviderTypesEnum.SqlServer:
                    CreateFunctionInSqlServer();
                    break;
                case DbMigrationExtensions.ProviderTypesEnum.MySQL:
                    CreateFunctionInMySQL();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoundJumperMap", "Camera");
            DropColumn("dbo.RoundJumperMap", "Score");
            Sql("DROP FUNCTION f_JumperName");
        }

        private void CreateFunctionInSqlServer()
        {
            Sql(@"
-- =============================================
-- Author:		Jay Hilden
-- Create date: 24-Aug-2015
-- Description:	Get the name of the jumper as: LastName, FirstName
-- =============================================
CREATE FUNCTION f_JumperName 
(
	@jumperID int
)
RETURNS varchar(500)
AS
BEGIN
	DECLARE @Result varchar(500);

	SELECT @Result = j.LastName + ', ' + j.FirstName
	FROM dbo.Jumper j
	WHERE j.JumperID = @jumperID

	RETURN @Result
END
");
        }

        private void CreateFunctionInMySQL()
        {
            Sql(@"DROP function IF EXISTS `f_JumperName`;");
            Sql(@"CREATE FUNCTION `f_JumperName` 
        (
            JumperID int
        )
        RETURNS varchar(500)
        BEGIN
            DECLARE result VARCHAR(500);
            SELECT j.LastName + ', ' + j.FirstName
            INTO result
            FROM Jumper j
            WHERE j.JumperID = JumperID;

            RETURN x;
        END;");
        }
    }
}

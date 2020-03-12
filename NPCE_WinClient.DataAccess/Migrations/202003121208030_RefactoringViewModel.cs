namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoringViewModel : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Service");
            AlterColumn("dbo.Service", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Service", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Service");
            AlterColumn("dbo.Service", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Service", "Id");
        }
    }
}

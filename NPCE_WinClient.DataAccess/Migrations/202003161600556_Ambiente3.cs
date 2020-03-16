namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ambiente3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ambiente", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ambiente", "Description", c => c.String());
        }
    }
}

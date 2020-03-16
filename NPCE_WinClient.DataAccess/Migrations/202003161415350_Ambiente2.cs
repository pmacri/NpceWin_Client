namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ambiente2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ambiente", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ambiente", "Description");
        }
    }
}

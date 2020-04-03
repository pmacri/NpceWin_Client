namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PageinaBollettinoDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaginaBollettino", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaginaBollettino", "Description");
        }
    }
}

namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdOrdine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Visura", "IdOrdine", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Visura", "IdOrdine");
        }
    }
}

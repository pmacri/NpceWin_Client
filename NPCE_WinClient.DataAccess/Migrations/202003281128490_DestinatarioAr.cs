namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DestinatarioAr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Anagrafica", "IsDestinatarioAR", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Anagrafica", "IsDestinatarioAR");
        }
    }
}

namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CognomeRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Anagrafica", "Cognome", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Anagrafica", "Cognome", c => c.String());
        }
    }
}

namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServizioIdordine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servizio", "GuidUtente", c => c.String());
            AddColumn("dbo.Servizio", "IdOrdine", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servizio", "IdOrdine");
            DropColumn("dbo.Servizio", "GuidUtente");
        }
    }
}

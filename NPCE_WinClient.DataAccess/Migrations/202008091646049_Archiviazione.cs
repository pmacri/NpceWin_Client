namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Archiviazione : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servizio", "TipoArchiviazione", c => c.String());
            AddColumn("dbo.Servizio", "AnniArchiviazione", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servizio", "AnniArchiviazione");
            DropColumn("dbo.Servizio", "TipoArchiviazione");
        }
    }
}

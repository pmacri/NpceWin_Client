namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoRecapito : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Visura", name: "VisureTipoRecapito_Id", newName: "VisureTipoRecapitoId");
            RenameIndex(table: "dbo.Visura", name: "IX_VisureTipoRecapito_Id", newName: "IX_VisureTipoRecapitoId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Visura", name: "IX_VisureTipoRecapitoId", newName: "IX_VisureTipoRecapito_Id");
            RenameColumn(table: "dbo.Visura", name: "VisureTipoRecapitoId", newName: "VisureTipoRecapito_Id");
        }
    }
}

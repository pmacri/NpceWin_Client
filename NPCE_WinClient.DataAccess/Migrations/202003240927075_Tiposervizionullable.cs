namespace NPCE_WinClient.DataAccess.Migrations
    {
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tiposervizionullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Servizio", "TipoServizioId", "dbo.TipoServizio");
            DropIndex("dbo.Servizio", new[] { "TipoServizioId" });
            AlterColumn("dbo.Servizio", "TipoServizioId", c => c.Int());
            CreateIndex("dbo.Servizio", "TipoServizioId");
            AddForeignKey("dbo.Servizio", "TipoServizioId", "dbo.TipoServizio", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servizio", "TipoServizioId", "dbo.TipoServizio");
            DropIndex("dbo.Servizio", new[] { "TipoServizioId" });
            AlterColumn("dbo.Servizio", "TipoServizioId", c => c.Int(nullable: false));
            CreateIndex("dbo.Servizio", "TipoServizioId");
            AddForeignKey("dbo.Servizio", "TipoServizioId", "dbo.TipoServizio", "Id", cascadeDelete: true);
        }
    }
}

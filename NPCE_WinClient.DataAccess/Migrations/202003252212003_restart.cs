namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class restart : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Servizio", "StatoServizioId", "dbo.StatoServizio");
            DropIndex("dbo.Servizio", new[] { "StatoServizioId" });
            AlterColumn("dbo.Servizio", "StatoServizioId", c => c.Int(nullable: false));
            CreateIndex("dbo.Servizio", "StatoServizioId");
            AddForeignKey("dbo.Servizio", "StatoServizioId", "dbo.StatoServizio", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servizio", "StatoServizioId", "dbo.StatoServizio");
            DropIndex("dbo.Servizio", new[] { "StatoServizioId" });
            AlterColumn("dbo.Servizio", "StatoServizioId", c => c.Int());
            CreateIndex("dbo.Servizio", "StatoServizioId");
            AddForeignKey("dbo.Servizio", "StatoServizioId", "dbo.StatoServizio", "Id");
        }
    }
}

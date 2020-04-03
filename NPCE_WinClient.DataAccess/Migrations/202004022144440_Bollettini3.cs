namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bollettini3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bollettino", "PaginaBollettinoId", "dbo.PaginaBollettino");
            DropIndex("dbo.Bollettino", new[] { "PaginaBollettinoId" });
            //DropTable("dbo.Bollettino");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Bollettino",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tipo = c.String(),
                        Logo = c.Binary(),
                        NumeroContoCorrente = c.String(),
                        IntestatoA = c.String(),
                        AdditionalInfo = c.String(),
                        NumeroAutorizzazioneStampaInProprio = c.String(),
                        IBan = c.String(),
                        EseguitoDaNominativo = c.String(),
                        EseguitoDaIndirizzo = c.String(),
                        EseguitoDaCap = c.String(),
                        EseguitoDaLocalita = c.String(),
                        ImportoEuro = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CodiceCliente = c.String(),
                        PaginaBollettinoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Bollettino", "PaginaBollettinoId");
            AddForeignKey("dbo.Bollettino", "PaginaBollettinoId", "dbo.PaginaBollettino", "Id", cascadeDelete: true);
        }
    }
}

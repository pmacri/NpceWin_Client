namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Servizio", "MittenteId", "dbo.Anagrafica");
            DropForeignKey("dbo.Anagrafica", "Servizio_Id", "dbo.Servizio");
            DropForeignKey("dbo.Servizio", "Anagrafica_Id", "dbo.Anagrafica");
            DropIndex("dbo.Anagrafica", new[] { "Servizio_Id" });
            DropIndex("dbo.Servizio", new[] { "MittenteId" });
            DropIndex("dbo.Servizio", new[] { "Anagrafica_Id" });
            CreateTable(
                "dbo.ServizioAnagrafica",
                c => new
                    {
                        Servizio_Id = c.Int(nullable: false),
                        Anagrafica_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Servizio_Id, t.Anagrafica_Id })
                .ForeignKey("dbo.Servizio", t => t.Servizio_Id, cascadeDelete: true)
                .ForeignKey("dbo.Anagrafica", t => t.Anagrafica_Id, cascadeDelete: true)
                .Index(t => t.Servizio_Id)
                .Index(t => t.Anagrafica_Id);
            
            DropColumn("dbo.Anagrafica", "Servizio_Id");
            DropColumn("dbo.Servizio", "MittenteId");
            DropColumn("dbo.Servizio", "Anagrafica_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servizio", "Anagrafica_Id", c => c.Int());
            AddColumn("dbo.Servizio", "MittenteId", c => c.Int(nullable: false));
            AddColumn("dbo.Anagrafica", "Servizio_Id", c => c.Int());
            DropForeignKey("dbo.ServizioAnagrafica", "Anagrafica_Id", "dbo.Anagrafica");
            DropForeignKey("dbo.ServizioAnagrafica", "Servizio_Id", "dbo.Servizio");
            DropIndex("dbo.ServizioAnagrafica", new[] { "Anagrafica_Id" });
            DropIndex("dbo.ServizioAnagrafica", new[] { "Servizio_Id" });
            DropTable("dbo.ServizioAnagrafica");
            CreateIndex("dbo.Servizio", "Anagrafica_Id");
            CreateIndex("dbo.Servizio", "MittenteId");
            CreateIndex("dbo.Anagrafica", "Servizio_Id");
            AddForeignKey("dbo.Servizio", "Anagrafica_Id", "dbo.Anagrafica", "Id");
            AddForeignKey("dbo.Anagrafica", "Servizio_Id", "dbo.Servizio", "Id");
            AddForeignKey("dbo.Servizio", "MittenteId", "dbo.Anagrafica", "Id", cascadeDelete: true);
        }
    }
}

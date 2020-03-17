namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documento", "Servizio_Id", "dbo.Servizio");
            DropIndex("dbo.Documento", new[] { "Servizio_Id" });
            CreateTable(
                "dbo.DocumentoServizio",
                c => new
                    {
                        Documento_Id = c.Int(nullable: false),
                        Servizio_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Documento_Id, t.Servizio_Id })
                .ForeignKey("dbo.Documento", t => t.Documento_Id, cascadeDelete: true)
                .ForeignKey("dbo.Servizio", t => t.Servizio_Id, cascadeDelete: true)
                .Index(t => t.Documento_Id)
                .Index(t => t.Servizio_Id);
            
            DropColumn("dbo.Documento", "Servizio_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documento", "Servizio_Id", c => c.Int());
            DropForeignKey("dbo.DocumentoServizio", "Servizio_Id", "dbo.Servizio");
            DropForeignKey("dbo.DocumentoServizio", "Documento_Id", "dbo.Documento");
            DropIndex("dbo.DocumentoServizio", new[] { "Servizio_Id" });
            DropIndex("dbo.DocumentoServizio", new[] { "Documento_Id" });
            DropTable("dbo.DocumentoServizio");
            CreateIndex("dbo.Documento", "Servizio_Id");
            AddForeignKey("dbo.Documento", "Servizio_Id", "dbo.Servizio", "Id");
        }
    }
}

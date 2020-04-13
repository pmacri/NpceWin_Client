namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Visure4 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Visura", "DocumentoCodiceDocumento_Id", "dbo.VisureCodiceDocumento");
            //DropForeignKey("dbo.Visura", "DocumentoFormatoDocumento_Id", "dbo.VisureFormatoDocumento");
            //DropForeignKey("dbo.Visura", "DocumentoTipoDocumento_Id", "dbo.VisureTipoDocumento");
            //DropForeignKey("dbo.Visura", "VisureTipoRecapito_Id", "dbo.VisureTipoRecapito");
            //DropIndex("dbo.Visura", new[] { "DocumentoCodiceDocumento_Id" });
            //DropIndex("dbo.Visura", new[] { "DocumentoFormatoDocumento_Id" });
            //DropIndex("dbo.Visura", new[] { "DocumentoTipoDocumento_Id" });
            //DropIndex("dbo.Visura", new[] { "VisureTipoRecapito_Id" });
            //DropTable("dbo.Visura");
            //DropTable("dbo.VisureCodiceDocumento");
            //DropTable("dbo.VisureFormatoDocumento");
            //DropTable("dbo.VisureTipoDocumento");
            //DropTable("dbo.VisureTipoRecapito");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.VisureTipoRecapito",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VisureTipoDocumento",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VisureFormatoDocumento",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VisureCodiceDocumento",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Visura",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RichiedenteNome = c.String(),
                        RichiedenteCognome = c.String(),
                        RichiedenteIndirizzo = c.String(),
                        RichiedenteLocalita = c.String(),
                        RichiedenteCap = c.String(),
                        RichiedentePrefissoTelefono = c.String(),
                        RichiedenteTelefono = c.String(),
                        DestinatarioNominativo = c.String(),
                        DestinatarioLocalita = c.String(),
                        DestinatarioCap = c.String(),
                        DestinatarioEmail = c.String(),
                        DocumentoIntestatarioNome = c.String(),
                        DocumentoIntestatarioCognome = c.String(),
                        DocumentoIntestatarioRagioneSociale = c.String(),
                        DocumentoIntestatarioCCIAA = c.String(),
                        DocumentoIntestatarioNREA = c.String(),
                        DocumentoIntestatarioCodiceFiscale = c.String(),
                        DocumentoCodiceDocumento_Id = c.String(maxLength: 128),
                        DocumentoFormatoDocumento_Id = c.String(maxLength: 128),
                        DocumentoTipoDocumento_Id = c.String(maxLength: 128),
                        VisureTipoRecapito_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Visura", "VisureTipoRecapito_Id");
            CreateIndex("dbo.Visura", "DocumentoTipoDocumento_Id");
            CreateIndex("dbo.Visura", "DocumentoFormatoDocumento_Id");
            CreateIndex("dbo.Visura", "DocumentoCodiceDocumento_Id");
            AddForeignKey("dbo.Visura", "VisureTipoRecapito_Id", "dbo.VisureTipoRecapito", "Id");
            AddForeignKey("dbo.Visura", "DocumentoTipoDocumento_Id", "dbo.VisureTipoDocumento", "Id");
            AddForeignKey("dbo.Visura", "DocumentoFormatoDocumento_Id", "dbo.VisureFormatoDocumento", "Id");
            AddForeignKey("dbo.Visura", "DocumentoCodiceDocumento_Id", "dbo.VisureCodiceDocumento", "Id");
        }
    }
}

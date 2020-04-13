namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Visure5 : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VisureCodiceDocumento", t => t.DocumentoCodiceDocumento_Id)
                .ForeignKey("dbo.VisureFormatoDocumento", t => t.DocumentoFormatoDocumento_Id)
                .ForeignKey("dbo.VisureTipoDocumento", t => t.DocumentoTipoDocumento_Id)
                .ForeignKey("dbo.VisureTipoRecapito", t => t.VisureTipoRecapito_Id)
                .Index(t => t.DocumentoCodiceDocumento_Id)
                .Index(t => t.DocumentoFormatoDocumento_Id)
                .Index(t => t.DocumentoTipoDocumento_Id)
                .Index(t => t.VisureTipoRecapito_Id);
            
            CreateTable(
                "dbo.VisureCodiceDocumento",
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
                "dbo.VisureTipoDocumento",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VisureTipoRecapito",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visura", "VisureTipoRecapito_Id", "dbo.VisureTipoRecapito");
            DropForeignKey("dbo.Visura", "DocumentoTipoDocumento_Id", "dbo.VisureTipoDocumento");
            DropForeignKey("dbo.Visura", "DocumentoFormatoDocumento_Id", "dbo.VisureFormatoDocumento");
            DropForeignKey("dbo.Visura", "DocumentoCodiceDocumento_Id", "dbo.VisureCodiceDocumento");
            DropIndex("dbo.Visura", new[] { "VisureTipoRecapito_Id" });
            DropIndex("dbo.Visura", new[] { "DocumentoTipoDocumento_Id" });
            DropIndex("dbo.Visura", new[] { "DocumentoFormatoDocumento_Id" });
            DropIndex("dbo.Visura", new[] { "DocumentoCodiceDocumento_Id" });
            DropTable("dbo.VisureTipoRecapito");
            DropTable("dbo.VisureTipoDocumento");
            DropTable("dbo.VisureFormatoDocumento");
            DropTable("dbo.VisureCodiceDocumento");
            DropTable("dbo.Visura");
        }
    }
}

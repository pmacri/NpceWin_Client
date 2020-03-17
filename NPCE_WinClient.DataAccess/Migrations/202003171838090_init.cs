namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ambiente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        customerid = c.String(),
                        costcenter = c.String(),
                        billingcenter = c.String(),
                        idsender = c.String(),
                        sendersystem = c.String(nullable: false),
                        smuser = c.String(nullable: false),
                        contracttype = c.String(),
                        usertype = c.String(),
                        codicefiscale = c.String(),
                        partitaiva = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        LolUri = c.String(),
                        RolUri = c.String(),
                        ColUri = c.String(),
                        MolUri = c.String(),
                        Contratto = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Anagrafica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cognome = c.String(nullable: false),
                        Nome = c.String(),
                        RagioneSociale = c.String(),
                        ComplementoNominativo = c.String(),
                        DUG = c.String(),
                        Toponimo = c.String(),
                        Esponente = c.String(),
                        NumeroCivico = c.String(),
                        ComplementoIndirizzo = c.String(),
                        Frazione = c.String(),
                        Citta = c.String(),
                        Cap = c.String(),
                        Telefono = c.String(),
                        CasellaPostale = c.String(),
                        Provincia = c.String(),
                        Stato = c.String(),
                        IsMittente = c.Boolean(nullable: false),
                        IsDefaultMittente = c.Boolean(nullable: false),
                        Servizio_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servizio", t => t.Servizio_Id)
                .Index(t => t.Servizio_Id);
            
            CreateTable(
                "dbo.Servizio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AvvisoRicevimento = c.Boolean(nullable: false),
                        FronteRetro = c.Boolean(nullable: false),
                        Colore = c.Boolean(nullable: false),
                        ArchiviazioneDigitale = c.Boolean(nullable: false),
                        Autoconferma = c.Boolean(nullable: false),
                        TipoServizioId = c.Int(nullable: false),
                        MittenteId = c.Int(nullable: false),
                        Anagrafica_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Anagrafica", t => t.MittenteId, cascadeDelete: true)
                .ForeignKey("dbo.TipoServizio", t => t.TipoServizioId, cascadeDelete: true)
                .ForeignKey("dbo.Anagrafica", t => t.Anagrafica_Id)
                .Index(t => t.TipoServizioId)
                .Index(t => t.MittenteId)
                .Index(t => t.Anagrafica_Id);
            
            CreateTable(
                "dbo.Documento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Size = c.Long(nullable: false),
                        Extension = c.String(),
                        Content = c.Binary(),
                        Tag = c.String(nullable: false),
                        Servizio_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servizio", t => t.Servizio_Id)
                .Index(t => t.Servizio_Id);
            
            CreateTable(
                "dbo.TipoServizio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servizio", "Anagrafica_Id", "dbo.Anagrafica");
            DropForeignKey("dbo.Servizio", "TipoServizioId", "dbo.TipoServizio");
            DropForeignKey("dbo.Anagrafica", "Servizio_Id", "dbo.Servizio");
            DropForeignKey("dbo.Servizio", "MittenteId", "dbo.Anagrafica");
            DropForeignKey("dbo.Documento", "Servizio_Id", "dbo.Servizio");
            DropIndex("dbo.Documento", new[] { "Servizio_Id" });
            DropIndex("dbo.Servizio", new[] { "Anagrafica_Id" });
            DropIndex("dbo.Servizio", new[] { "MittenteId" });
            DropIndex("dbo.Servizio", new[] { "TipoServizioId" });
            DropIndex("dbo.Anagrafica", new[] { "Servizio_Id" });
            DropTable("dbo.TipoServizio");
            DropTable("dbo.Documento");
            DropTable("dbo.Servizio");
            DropTable("dbo.Anagrafica");
            DropTable("dbo.Ambiente");
        }
    }
}

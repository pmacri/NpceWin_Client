namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mittente : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mittente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cognome = c.String(),
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
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Mittente");
        }
    }
}

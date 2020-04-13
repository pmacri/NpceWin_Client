namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Visure : DbMigration
    {
        public override void Up()
        {
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
            DropTable("dbo.VisureTipoRecapito");
            DropTable("dbo.VisureTipoDocumento");
            DropTable("dbo.VisureFormatoDocumento");
            DropTable("dbo.VisureCodiceDocumento");
        }
    }
}

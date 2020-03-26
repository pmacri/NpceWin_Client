namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Opzioni : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Opzioni",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttestazioneConsegna = c.Boolean(nullable: false),
                        FronteRetro = c.Boolean(nullable: false),
                        Colore = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Servizio", "OpzioniId", c => c.Int());
            CreateIndex("dbo.Servizio", "OpzioniId");
            AddForeignKey("dbo.Servizio", "OpzioniId", "dbo.Opzioni", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servizio", "OpzioniId", "dbo.Opzioni");
            DropIndex("dbo.Servizio", new[] { "OpzioniId" });
            DropColumn("dbo.Servizio", "OpzioniId");
            DropTable("dbo.Opzioni");
        }
    }
}

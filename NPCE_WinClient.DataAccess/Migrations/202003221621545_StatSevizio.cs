namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StatSevizio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatoServizio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Servizio", "StatoServizioId", c => c.Int(nullable: false));
            CreateIndex("dbo.Servizio", "StatoServizioId");
            AddForeignKey("dbo.Servizio", "StatoServizioId", "dbo.StatoServizio", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servizio", "StatoServizioId", "dbo.StatoServizio");
            DropIndex("dbo.Servizio", new[] { "StatoServizioId" });
            DropColumn("dbo.Servizio", "StatoServizioId");
            DropTable("dbo.StatoServizio");
        }
    }
}

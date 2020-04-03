namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bollettini : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaginaBollettino",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Servizio_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servizio", t => t.Servizio_Id)
                .Index(t => t.Servizio_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaginaBollettino", "Servizio_Id", "dbo.Servizio");
            DropIndex("dbo.PaginaBollettino", new[] { "Servizio_Id" });
            DropTable("dbo.PaginaBollettino");
        }
    }
}

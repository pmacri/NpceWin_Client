namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKVisure2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Visura", "StatoServizioId", c => c.Int(nullable: false));
            CreateIndex("dbo.Visura", "StatoServizioId");
            AddForeignKey("dbo.Visura", "StatoServizioId", "dbo.StatoServizio", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visura", "StatoServizioId", "dbo.StatoServizio");
            DropIndex("dbo.Visura", new[] { "StatoServizioId" });
            DropColumn("dbo.Visura", "StatoServizioId");
        }
    }
}

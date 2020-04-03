namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bollettini2 : DbMigration
    {
        public override void Up()
        {
            //CreateIndex("dbo.Bollettino", "PaginaBollettinoId");
            //AddForeignKey("dbo.Bollettino", "PaginaBollettinoId", "dbo.PaginaBollettino", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bollettino", "PaginaBollettinoId", "dbo.PaginaBollettino");
            DropIndex("dbo.Bollettino", new[] { "PaginaBollettinoId" });
        }
    }
}

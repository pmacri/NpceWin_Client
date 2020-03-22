namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IMittente2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Anagrafica", "IsMittente", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Anagrafica", "IsMittente");
        }
    }
}

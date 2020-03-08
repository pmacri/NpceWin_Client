namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsMittente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Anagrafica", "IsMittente", c => c.Boolean(nullable: false));
            AddColumn("dbo.Anagrafica", "IsDefaultMittente", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Anagrafica", "IsDefaultMittente");
            DropColumn("dbo.Anagrafica", "IsMittente");
        }
    }
}

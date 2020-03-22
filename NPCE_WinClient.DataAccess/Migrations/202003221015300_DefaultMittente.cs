namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultMittente : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Anagrafica", "IsDefaultMittente");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Anagrafica", "IsDefaultMittente", c => c.Boolean(nullable: false));
        }
    }
}

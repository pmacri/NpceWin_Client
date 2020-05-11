namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondoTentativo2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Anagrafica", "IsMittente");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Anagrafica", "IsMittente", c => c.Boolean(nullable: false));
        }
    }
}

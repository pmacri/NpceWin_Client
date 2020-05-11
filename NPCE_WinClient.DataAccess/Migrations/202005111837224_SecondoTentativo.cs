namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondoTentativo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servizio", "SecondoTentativoRecapito", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servizio", "SecondoTentativoRecapito");
        }
    }
}

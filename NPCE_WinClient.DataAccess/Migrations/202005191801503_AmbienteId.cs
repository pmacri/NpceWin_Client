namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AmbienteId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servizio", "AmbienteId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servizio", "AmbienteId");
        }
    }
}

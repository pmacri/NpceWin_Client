namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AmbienteId2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Visura", "AmbienteId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Visura", "AmbienteId");
        }
    }
}

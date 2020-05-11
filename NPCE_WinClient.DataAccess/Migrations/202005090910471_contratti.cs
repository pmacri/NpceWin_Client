namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contratti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ambiente", "ContrattoMOL", c => c.String());
            AddColumn("dbo.Ambiente", "ContrattoCOL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ambiente", "ContrattoCOL");
            DropColumn("dbo.Ambiente", "ContrattoMOL");
        }
    }
}

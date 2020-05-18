namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdRichiesta1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Visura", "IdRichiesta", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Visura", "IdRichiesta");
        }
    }
}

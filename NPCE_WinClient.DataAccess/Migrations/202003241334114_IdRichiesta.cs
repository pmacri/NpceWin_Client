namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdRichiesta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servizio", "IdRichiesta", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servizio", "IdRichiesta");
        }
    }
}

namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AmbienteOtherHeaders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ambiente", "contractid", c => c.String());
            AddColumn("dbo.Ambiente", "customer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ambiente", "customer");
            DropColumn("dbo.Ambiente", "contractid");
        }
    }
}

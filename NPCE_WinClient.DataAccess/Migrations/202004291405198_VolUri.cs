namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolUri : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ambiente", "VolUri", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ambiente", "VolUri");
        }
    }
}

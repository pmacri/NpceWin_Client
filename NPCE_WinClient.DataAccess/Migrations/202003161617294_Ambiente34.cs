namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ambiente34 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ambiente", "Uri", c => c.String());
            AddColumn("dbo.Ambiente", "Username", c => c.String());
            AddColumn("dbo.Ambiente", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ambiente", "Password");
            DropColumn("dbo.Ambiente", "Username");
            DropColumn("dbo.Ambiente", "Uri");
        }
    }
}

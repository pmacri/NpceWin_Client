namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentoAdded3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Documento", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documento", "Description", c => c.String());
        }
    }
}

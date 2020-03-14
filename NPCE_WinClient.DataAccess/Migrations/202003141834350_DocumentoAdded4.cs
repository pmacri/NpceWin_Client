namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentoAdded4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documento", "Tag", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Documento", "Tag", c => c.String());
        }
    }
}

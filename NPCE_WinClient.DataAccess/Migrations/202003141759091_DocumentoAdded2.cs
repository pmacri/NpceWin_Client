namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentoAdded2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documento", "Tag", c => c.String());
            DropColumn("dbo.Documento", "CreationTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documento", "CreationTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Documento", "Tag");
        }
    }
}

namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentoAdded1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Size = c.Long(nullable: false),
                        Extension = c.String(),
                        Content = c.Binary(),
                        Description = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Documento");
        }
    }
}

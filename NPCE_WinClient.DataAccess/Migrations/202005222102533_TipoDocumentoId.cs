namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoDocumentoId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisureCodiceDocumento", "VisureTipoDocumentoId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisureCodiceDocumento", "VisureTipoDocumentoId");
        }
    }
}

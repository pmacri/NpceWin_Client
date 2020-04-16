namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKVisure : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Visura", name: "DocumentoCodiceDocumento_Id", newName: "VisureCodiceDocumentoId");
            RenameColumn(table: "dbo.Visura", name: "DocumentoFormatoDocumento_Id", newName: "VisureFormatoDocumentoId");
            RenameColumn(table: "dbo.Visura", name: "DocumentoTipoDocumento_Id", newName: "VisureTipoDocumentoId");
            RenameIndex(table: "dbo.Visura", name: "IX_DocumentoTipoDocumento_Id", newName: "IX_VisureTipoDocumentoId");
            RenameIndex(table: "dbo.Visura", name: "IX_DocumentoCodiceDocumento_Id", newName: "IX_VisureCodiceDocumentoId");
            RenameIndex(table: "dbo.Visura", name: "IX_DocumentoFormatoDocumento_Id", newName: "IX_VisureFormatoDocumentoId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Visura", name: "IX_VisureFormatoDocumentoId", newName: "IX_DocumentoFormatoDocumento_Id");
            RenameIndex(table: "dbo.Visura", name: "IX_VisureCodiceDocumentoId", newName: "IX_DocumentoCodiceDocumento_Id");
            RenameIndex(table: "dbo.Visura", name: "IX_VisureTipoDocumentoId", newName: "IX_DocumentoTipoDocumento_Id");
            RenameColumn(table: "dbo.Visura", name: "VisureTipoDocumentoId", newName: "DocumentoTipoDocumento_Id");
            RenameColumn(table: "dbo.Visura", name: "VisureFormatoDocumentoId", newName: "DocumentoFormatoDocumento_Id");
            RenameColumn(table: "dbo.Visura", name: "VisureCodiceDocumentoId", newName: "DocumentoCodiceDocumento_Id");
        }
    }
}

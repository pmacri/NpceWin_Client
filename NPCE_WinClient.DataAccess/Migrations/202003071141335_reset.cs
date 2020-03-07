namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reset : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.Mittente", newName: "Anagrafica");
        }

        public override void Down()
        {
            RenameTable(name: "dbo.Anagrafica", newName: "Mittente");
        }
    }
}

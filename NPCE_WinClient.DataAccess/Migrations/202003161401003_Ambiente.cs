namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ambiente : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ambiente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        customerid = c.String(),
                        costcenter = c.String(),
                        billingcenter = c.String(),
                        idsender = c.String(),
                        sendersystem = c.String(nullable: false),
                        smuser = c.String(nullable: false),
                        contracttype = c.String(),
                        usertype = c.String(),
                        codicefiscale = c.String(),
                        partitaiva = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ambiente");
        }
    }
}

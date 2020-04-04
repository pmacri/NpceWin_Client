﻿namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BollettinoCausale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bollettino", "Causale", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bollettino", "Causale");
        }
    }
}

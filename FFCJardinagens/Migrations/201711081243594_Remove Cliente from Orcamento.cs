namespace FFCJardinagens.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveClientefromOrcamento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orcamentoes", "ClienteID", "dbo.Clientes");
            DropIndex("dbo.Orcamentoes", new[] { "ClienteID" });
            AddColumn("dbo.Orcamentoes", "ClienteNome", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orcamentoes", "ClienteNome");
            CreateIndex("dbo.Orcamentoes", "ClienteID");
            AddForeignKey("dbo.Orcamentoes", "ClienteID", "dbo.Clientes", "ID", cascadeDelete: true);
        }
    }
}

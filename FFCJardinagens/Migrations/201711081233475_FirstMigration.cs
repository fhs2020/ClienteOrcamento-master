namespace FFCJardinagens.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClienteID = c.Int(nullable: false),
                        OrcamentoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Empresa = c.String(),
                        Nome = c.String(),
                        Email = c.String(),
                        Telefone = c.String(),
                        Celular = c.String(),
                        CNPJ = c.String(),
                        Endereco = c.String(),
                        Cidade = c.String(),
                        UF = c.String(),
                        CEP = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Orcamentoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClienteID = c.Int(nullable: false),
                        TotalOrcamentoID = c.Int(nullable: false),
                        Quantidade = c.Int(nullable: false),
                        Descriminação = c.String(),
                        ProdutoUnidade = c.Int(nullable: false),
                        ProdutoTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorTotal = c.Decimal(precision: 18, scale: 2),
                        DataOrcamento = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clientes", t => t.ClienteID, cascadeDelete: true)
                .ForeignKey("dbo.TotalOrcamentoes", t => t.TotalOrcamentoID, cascadeDelete: true)
                .Index(t => t.ClienteID)
                .Index(t => t.TotalOrcamentoID);
            
            CreateTable(
                "dbo.TotalOrcamentoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClienteID = c.Int(nullable: false),
                        ClienteNome = c.String(),
                        ValorTotal = c.Decimal(precision: 18, scale: 2),
                        DataOrcamento = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orcamentoes", "TotalOrcamentoID", "dbo.TotalOrcamentoes");
            DropForeignKey("dbo.Orcamentoes", "ClienteID", "dbo.Clientes");
            DropIndex("dbo.Orcamentoes", new[] { "TotalOrcamentoID" });
            DropIndex("dbo.Orcamentoes", new[] { "ClienteID" });
            DropTable("dbo.TotalOrcamentoes");
            DropTable("dbo.Orcamentoes");
            DropTable("dbo.Clientes");
            DropTable("dbo.Carts");
        }
    }
}

namespace CardGameP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Email = c.String(),
                        Senha = c.String(),
                        Telefone = c.String(),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdCliente);
            
            CreateTable(
                "dbo.Pedidoes",
                c => new
                    {
                        IdPedido = c.Int(nullable: false, identity: true),
                        DataPedido = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        IdCliente = c.Int(nullable: false),
                        IdFuncionario = c.Int(nullable: false),
                        Funcionario_id_funcionario = c.Int(),
                    })
                .PrimaryKey(t => t.IdPedido)
                .ForeignKey("dbo.Clientes", t => t.IdCliente, cascadeDelete: true)
                .ForeignKey("dbo.Funcionarios", t => t.Funcionario_id_funcionario)
                .Index(t => t.IdCliente)
                .Index(t => t.Funcionario_id_funcionario);
            
            CreateTable(
                "dbo.Funcionarios",
                c => new
                    {
                        id_funcionario = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 100),
                        email = c.String(nullable: false, maxLength: 150),
                        senha = c.String(nullable: false, maxLength: 100),
                        telefone = c.String(maxLength: 20),
                        cargo = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.id_funcionario);
            
            CreateTable(
                "dbo.ItemPedidoes",
                c => new
                    {
                        IdItem = c.Int(nullable: false, identity: true),
                        Quantidade = c.Int(nullable: false),
                        PrecoUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdPedido = c.Int(nullable: false),
                        IdProduto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdItem)
                .ForeignKey("dbo.Pedidoes", t => t.IdPedido, cascadeDelete: true)
                .ForeignKey("dbo.Produtoes", t => t.IdProduto, cascadeDelete: true)
                .Index(t => t.IdPedido)
                .Index(t => t.IdProduto);
            
            CreateTable(
                "dbo.Produtoes",
                c => new
                    {
                        IdProduto = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Jogo = c.String(),
                        Raridade = c.String(),
                        Preco = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estoque = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProduto);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemPedidoes", "IdProduto", "dbo.Produtoes");
            DropForeignKey("dbo.ItemPedidoes", "IdPedido", "dbo.Pedidoes");
            DropForeignKey("dbo.Pedidoes", "Funcionario_id_funcionario", "dbo.Funcionarios");
            DropForeignKey("dbo.Pedidoes", "IdCliente", "dbo.Clientes");
            DropIndex("dbo.ItemPedidoes", new[] { "IdProduto" });
            DropIndex("dbo.ItemPedidoes", new[] { "IdPedido" });
            DropIndex("dbo.Pedidoes", new[] { "Funcionario_id_funcionario" });
            DropIndex("dbo.Pedidoes", new[] { "IdCliente" });
            DropTable("dbo.Produtoes");
            DropTable("dbo.ItemPedidoes");
            DropTable("dbo.Funcionarios");
            DropTable("dbo.Pedidoes");
            DropTable("dbo.Clientes");
        }
    }
}

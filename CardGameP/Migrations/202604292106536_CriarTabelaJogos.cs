namespace CardGameP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriarTabelaJogos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jogoes",
                c => new
                    {
                        IdJogo = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdJogo);
            
            AddColumn("dbo.Produtoes", "IdJogo", c => c.Int(nullable: false));
            CreateIndex("dbo.Produtoes", "IdJogo");
            AddForeignKey("dbo.Produtoes", "IdJogo", "dbo.Jogoes", "IdJogo", cascadeDelete: true);
            DropColumn("dbo.Produtoes", "Jogo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Produtoes", "Jogo", c => c.String());
            DropForeignKey("dbo.Produtoes", "IdJogo", "dbo.Jogoes");
            DropIndex("dbo.Produtoes", new[] { "IdJogo" });
            DropColumn("dbo.Produtoes", "IdJogo");
            DropTable("dbo.Jogoes");
        }
    }
}

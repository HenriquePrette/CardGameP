namespace CardGameP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImagemProduto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produtoes", "imagem", c => c.String());
            AlterColumn("dbo.Produtoes", "Nome", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Produtoes", "Nome", c => c.String());
            DropColumn("dbo.Produtoes", "imagem");
        }
    }
}

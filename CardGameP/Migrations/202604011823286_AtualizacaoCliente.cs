namespace CardGameP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizacaoCliente : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clientes", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "Senha", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clientes", "Senha", c => c.String());
            AlterColumn("dbo.Clientes", "Email", c => c.String());
        }
    }
}

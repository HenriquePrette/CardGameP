namespace CardGameP.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CardGameP.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<CardGameP.Models.LojaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CardGameP.Models.LojaContext context)
        {
            context.Funcionarios.AddOrUpdate(
        f => f.email,
        new Funcionario
        {
            nome = "Henrique",
            email = "admin@cardgame.com",
            senha = "123",
            telefone = "(18) 99999-9999",
            cargo = "Administrador"
        }
    );
        }
    }
}

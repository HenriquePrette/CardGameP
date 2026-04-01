# 🃏 CardGameP - Loja de Card Games

Este é um projeto de e-commerce focado na venda de Card Games, desenvolvido como atividade acadêmica. O sistema conta com áreas para clientes e funcionários, controle de sessões e persistência de dados.

## 🚀 Tecnologias Utilizadas
* **C#** (Linguagem de programação)
* **ASP.NET MVC 5** (Padrão de arquitetura do projeto)
* **Entity Framework 6** (OR/M para persistência de dados e Migrations)
* **Bootstrap 5** (Estilização e responsividade)
* **SQL Server** (Banco de dados relacional)

## 🛡️ Diferenciais Implementados
* **Validação de Modelos:** Trava de segurança para impedir cadastros com senhas fracas (mínimo 6 caracteres) e e-mails inválidos.
* **Tratamento Rigoroso de Exceções:** Uso estratégico de blocos `try-catch` capturando e tratando erros de banco de dados (`DbEntityValidationException`) e amarrando regras de negócio diretamente no controlador.
* **Controle de Sessão:** Autenticação separada para Clientes e Funcionários usando variáveis de `Session`.

## ⚙️ Como Executar o Projeto
1. Clone este repositório em sua máquina.
2. Abra a solução (`.sln`) no **Visual Studio**.
3. No Console do Gerenciador de Pacotes, execute o comando `Update-Database` para gerar as tabelas do banco de dados local.
4. Pressione `F5` para compilar e rodar o projeto no navegador.

---
*Desenvolvido por Henrique Prette como critério de avaliação acadêmica.*

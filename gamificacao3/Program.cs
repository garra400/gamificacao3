using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using gamificacao3.UI;
using gamificacao3.DbContext;
using gamificacao3;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "server=localhost;user=root;database=db_GerenciadordeProjetos"; // Deixar a senha vazia

        DbContext.CriarBancoDeDados(connectionString);

        Console.WriteLine("Bem-vindo ao e-commerce!");

        while (true)
        {
            Console.WriteLine("Selecione uma opção:");
            Console.WriteLine("1. Listar todos os produtos");
            Console.WriteLine("2. Comprar um produto");
            Console.WriteLine("3. Adicionar um novo produto");
            Console.WriteLine("4. Atualizar informações de um produto");
            Console.WriteLine("5. Remover um produto");
            Console.WriteLine("0. Sair");

            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    IProduto.ListarProdutos(connectionString);
                    break;
                case "2":
                    IProduto.ComprarProduto(connectionString);
                    break;
                case "3":
                    IProduto.AdicionarProduto(connectionString);
                    break;
                case "4":
                    IProduto.AtualizarProduto(connectionString);
                    break;
                case "5":
                    IProduto.RemoverProduto(connectionString);
                    break;
                case "0":
                    Console.WriteLine("Obrigado por utilizar o e-commerce. Até logo!");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

            Console.WriteLine();
        }
    }
}

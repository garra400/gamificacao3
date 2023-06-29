using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using gamificacao3.DbContext;

namespace gamificacao3.UI;
public class IProduto{
    public static void ListarProdutos(string connectionString)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT * FROM produtos";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("Lista de produtos:");
                    while (reader.Read())
                    {
                        long produtoID = reader.GetInt64("ProdutoID");
                        string nome = reader.GetString("Nome");
                        string descricao = reader.GetString("Descricao");
                        decimal preco = reader.GetDecimal("Preco");
                        double quantidade = reader.GetDouble("Quantidade");

                        Console.WriteLine($"ID: {produtoID}, Nome: {nome}, Descrição: {descricao}, Preço: {preco}, Quantidade: {quantidade}");
                    }
                }
            }
        }
    }

    public static void ComprarProduto(string connectionString)
    {
        Console.WriteLine("Digite o ID do produto que deseja comprar:");
        int produtoID = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Digite a quantidade desejada:");
        int quantidade = Convert.ToInt32(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Verificar se o produto existe e possui quantidade suficiente
            string verificarProdutoQuery = "SELECT Quantidade FROM produtos WHERE ProdutoID = @produtoID";
            using (MySqlCommand command = new MySqlCommand(verificarProdutoQuery, connection))
            {
                command.Parameters.AddWithValue("@produtoID", produtoID);

                object result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    Console.WriteLine("O produto informado não existe.");
                    return;
                }

                double estoque = Convert.ToDouble(result);
                if (quantidade > estoque)
                {
                    Console.WriteLine("A quantidade desejada não está disponível no estoque.");
                    return;
                }
            }

            // Inserir o pedido
            string inserirPedidoQuery = "INSERT INTO pedidos (DataPedido, Cliente, Status) VALUES (@dataPedido, @cliente, @status)";
            using (MySqlCommand command = new MySqlCommand(inserirPedidoQuery, connection))
            {
                command.Parameters.AddWithValue("@dataPedido", DateTime.Now);
                command.Parameters.AddWithValue("@cliente", "Nome do cliente"); // Substituir pelo nome do cliente real
                command.Parameters.AddWithValue("@status", "Em processamento"); // Substituir pelo status real

                command.ExecuteNonQuery();
            }

            // Obter o ID do pedido inserido
            string obterUltimoPedidoQuery = "SELECT LAST_INSERT_ID()";
            int pedidoID;
            using (MySqlCommand command = new MySqlCommand(obterUltimoPedidoQuery, connection))
            {
                pedidoID = Convert.ToInt32(command.ExecuteScalar());
            }

            // Inserir o item do pedido
            string inserirItemPedidoQuery = "INSERT INTO itens_pedido (ProdutoID, Quantidade, PrecoUnitario, PedidoID) VALUES (@produtoID, @quantidade, @precoUnitario, @pedidoID)";
            using (MySqlCommand command = new MySqlCommand(inserirItemPedidoQuery, connection))
            {
                command.Parameters.AddWithValue("@produtoID", produtoID);
                command.Parameters.AddWithValue("@quantidade", quantidade);

                // Obter o preço unitário do produto
                string obterPrecoProdutoQuery = "SELECT Preco FROM produtos WHERE ProdutoID = @produtoID";
                using (MySqlCommand precoCommand = new MySqlCommand(obterPrecoProdutoQuery, connection))
                {
                    precoCommand.Parameters.AddWithValue("@produtoID", produtoID);
                    decimal precoUnitario = Convert.ToDecimal(precoCommand.ExecuteScalar());
                    command.Parameters.AddWithValue("@precoUnitario", precoUnitario);
                }

                command.Parameters.AddWithValue("@pedidoID", pedidoID);

                command.ExecuteNonQuery();
            }

            // Atualizar a quantidade do produto no estoque
            string atualizarEstoqueQuery = "UPDATE produtos SET Quantidade = Quantidade - @quantidade WHERE ProdutoID = @produtoID";
            using (MySqlCommand command = new MySqlCommand(atualizarEstoqueQuery, connection))
            {
                command.Parameters.AddWithValue("@quantidade", quantidade);
                command.Parameters.AddWithValue("@produtoID", produtoID);

                command.ExecuteNonQuery();
            }

            Console.WriteLine("Compra realizada com sucesso!");
        }
    }

    public static void AdicionarProduto(string connectionString)
    {
        Console.WriteLine("Digite o nome do produto:");
        string? nome = Console.ReadLine();

        Console.WriteLine("Digite a descrição do produto:");
        string? descricao = Console.ReadLine();

        Console.WriteLine("Digite o preço do produto:");
        decimal? preco = Convert.ToDecimal(Console.ReadLine());

        Console.WriteLine("Digite a quantidade do produto:");
        double? quantidade = Convert.ToDouble(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string inserirProdutoQuery = "INSERT INTO produtos (Nome, Descricao, Preco, Quantidade) VALUES (@nome, @descricao, @preco, @quantidade)";
            using (MySqlCommand command = new MySqlCommand(inserirProdutoQuery, connection))
            {
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@descricao", descricao);
                command.Parameters.AddWithValue("@preco", preco);
                command.Parameters.AddWithValue("@quantidade", quantidade);

                command.ExecuteNonQuery();
            }

            Console.WriteLine("Produto adicionado com sucesso!");
        }
    }

    public static void AtualizarProduto(string connectionString)
    {
        Console.WriteLine("Digite o ID do produto que deseja atualizar:");
        int produtoID = Convert.ToInt32(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Verificar se o produto existe
            string verificarProdutoQuery = "SELECT * FROM produtos WHERE ProdutoID = @produtoID";
            using (MySqlCommand command = new MySqlCommand(verificarProdutoQuery, connection))
            {
                command.Parameters.AddWithValue("@produtoID", produtoID);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Console.WriteLine("O produto informado não existe.");
                        return;
                    }
                }
            }

            // Ler os novos dados do produto
            Console.WriteLine("Digite o novo nome do produto:");
            string? novoNome = Console.ReadLine();

            Console.WriteLine("Digite a nova descrição do produto:");
            string? novaDescricao = Console.ReadLine();

            Console.WriteLine("Digite o novo preço do produto:");
            decimal novoPreco = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("Digite a nova quantidade do produto:");
            double novaQuantidade = Convert.ToDouble(Console.ReadLine());

            // Atualizar o produto no banco de dados
            string atualizarProdutoQuery = "UPDATE produtos SET Nome = @nome, Descricao = @descricao, Preco = @preco, Quantidade = @quantidade WHERE ProdutoID = @produtoID";
            using (MySqlCommand command = new MySqlCommand(atualizarProdutoQuery, connection))
            {
                command.Parameters.AddWithValue("@nome", novoNome);
                command.Parameters.AddWithValue("@descricao", novaDescricao);
                command.Parameters.AddWithValue("@preco", novoPreco);
                command.Parameters.AddWithValue("@quantidade", novaQuantidade);
                command.Parameters.AddWithValue("@produtoID", produtoID);

                command.ExecuteNonQuery();
            }

            Console.WriteLine("Produto atualizado com sucesso!");
        }
    }

    public static void RemoverProduto(string connectionString)
    {
        Console.WriteLine("Digite o ID do produto que deseja remover:");
        int produtoID = Convert.ToInt32(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Verificar se o produto existe
            string verificarProdutoQuery = "SELECT * FROM produtos WHERE ProdutoID = @produtoID";
            using (MySqlCommand command = new MySqlCommand(verificarProdutoQuery, connection))
            {
                command.Parameters.AddWithValue("@produtoID", produtoID);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Console.WriteLine("O produto informado não existe.");
                        return;
                    }
                }
            }

            // Remover o produto do banco de dados
            string removerProdutoQuery = "DELETE FROM produtos WHERE ProdutoID = @produtoID";
            using (MySqlCommand command = new MySqlCommand(removerProdutoQuery, connection))
            {
                command.Parameters.AddWithValue("@produtoID", produtoID);

                command.ExecuteNonQuery();
            }

            Console.WriteLine("Produto removido com sucesso!");
        }
    }
}
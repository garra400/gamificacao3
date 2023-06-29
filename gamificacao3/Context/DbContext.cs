using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using gamificacao3.Models;
using gamificacao3.UI;

namespace gamificacao3.DbContext;
public class DbContext{    
    public static void CriarBancoDeDados(string connectionString)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Criação da tabela "produtos" caso não exista
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS produtos (
                    ProdutoID INT PRIMARY KEY AUTO_INCREMENT,
                    Nome VARCHAR(100) NOT NULL,
                    Descricao VARCHAR(200) NOT NULL,
                    Preco DECIMAL(10, 2) NOT NULL,
                    Quantidade DOUBLE NOT NULL
                );";
            using (MySqlCommand command = new MySqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            // Criação da tabela "pedidos" caso não exista
            string createOrdersTableQuery = @"
                CREATE TABLE IF NOT EXISTS pedidos (
                    PedidoID INT PRIMARY KEY AUTO_INCREMENT,
                    DataPedido DATETIME NOT NULL,
                    Cliente VARCHAR(100),
                    Status VARCHAR(100)
                );";
            using (MySqlCommand command = new MySqlCommand(createOrdersTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            // Criação da tabela "itens_pedido" caso não exista
            string createOrderItemsTableQuery = @"
                CREATE TABLE IF NOT EXISTS itens_pedido (
                    ItemPedidoID INT PRIMARY KEY AUTO_INCREMENT,
                    ProdutoID INT NOT NULL,
                    Quantidade INT NOT NULL,
                    PrecoUnitario DECIMAL(10, 2) NOT NULL,
                    PedidoID INT NOT NULL,
                    FOREIGN KEY (ProdutoID) REFERENCES produtos(ProdutoID),
                    FOREIGN KEY (PedidoID) REFERENCES pedidos(PedidoID)
                );";
            using (MySqlCommand command = new MySqlCommand(createOrderItemsTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
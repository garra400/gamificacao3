using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using gamificacao3.Models;

namespace gamificacao3.Models;
public class Pedido
{
    public int? PedidoID { get; set; }
    private DateTime? _dataPedido { get; set; }
    private string? _cliente { get; set; }
    private string? _status { get; set; }
    
}
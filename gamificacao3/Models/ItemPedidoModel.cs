using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using gamificacao3.Models;

namespace gamificacao3.Models;

public class ItemPedido
{
    public int? ItemPedidoID { get; set; }
    private Produto? _produto { get; set; }
    private int? _quantidade { get; set; }
    private decimal? _precoUnitario { get; set; }
    private Pedido? _pedido { get; set; }
}

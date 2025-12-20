namespace ListaComprasAPI.Entities;

/// <summary>
/// Representa um item da lista de compras pertencente a um usuario
/// </summary>
public class ShoppingItem
{
    ///<summary>
    ///Identificador unico do item
    ///</summary>
    public int Id { get; set; }
    /// <summary>
    /// Nome do item (Ex: arroz, feijão)
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Quantidade de item compradas
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// Preço unitario do item
    /// </summary>
    public decimal UnitPrice { get; set; }
    /// <summary>
    /// Valor total do item na lista(Quantidade x preço unitario)
    /// Não é armazenado no banco, é calculado.
    /// </summary>
    public decimal TotalPrice => Quantity * UnitPrice;
    /// <summary>
    /// Data da criação do item
    /// </summary>
    public DateTime CreatedAT { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Id do usuario dono do item. 
    /// </summary>
    public int UserId { get; set; }
}

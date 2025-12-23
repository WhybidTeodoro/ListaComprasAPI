namespace ListaComprasAPI.DTOs.ShoppingItem;

/// <summary>
///DTO usado para retornar os dados do item da lista de compras
/// </summary>

public class ShoppingItemResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsPurchased { get; set; } = false;
    public decimal TotalPrice => Quantity * UnitPrice;
}

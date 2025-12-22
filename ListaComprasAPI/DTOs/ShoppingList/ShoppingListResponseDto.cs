namespace ListaComprasAPI.DTOs.ShoppingList;

/// <summary>
/// Dto usado para retorno da lista de compras
/// </summary>
public class ShoppingListResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //Valor total calculado dinamicamente
    public decimal TotalValue { get; set; }
}

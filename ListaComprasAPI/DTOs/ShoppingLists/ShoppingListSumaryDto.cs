namespace ListaComprasAPI.DTOs.ShoppingLists;

public class ShoppingListSumaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //valor total da lista
    public decimal TotalList { get; set; }
}

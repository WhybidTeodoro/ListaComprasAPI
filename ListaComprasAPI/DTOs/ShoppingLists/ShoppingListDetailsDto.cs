namespace ListaComprasAPI.DTOs.ShoppingLists;

public class ShoppingListDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public List<ShoppingItemDetailsDto> Items { get; set; } = new();

    //Total da lista (calculado dinamicamente)
    public decimal TotalList => Items.Sum(i => i.TotalItem);
}

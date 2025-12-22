using System.ComponentModel.DataAnnotations;

namespace ListaComprasAPI.DTOs.ShoppingItem;

public class ShoppingItemUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}

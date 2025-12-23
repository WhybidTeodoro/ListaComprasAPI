using System.ComponentModel.DataAnnotations;

namespace ListaComprasAPI.DTOs.ShoppingItem;

/// <summary>
///DTO usado para retornar os dados do item da lista de compras
/// </summary>

public class ShoppingItemResponseDto
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "O Nome do item é obrigatório")]
    [MaxLength(100, ErrorMessage = "O Nome do item deve ter no maximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    [Range(1, int.MaxValue, ErrorMessage = "A Quantidade deve ser maior que 0")]
    public int Quantity { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitario deve ser maior que 0")]
    public decimal UnitPrice { get; set; }
    public bool IsPurchased { get; set; } = false;
    public decimal TotalPrice => Quantity * UnitPrice;
}

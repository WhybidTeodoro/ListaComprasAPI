using System.ComponentModel.DataAnnotations;

namespace ListaComprasAPI.DTOs.ShoppingItem;

/// <summary>
/// DTO usado para criar um novo item na lista de compras
/// </summary>
public class CreateShoppingItemDto
{
    /// <summary>
    /// Nome do item a ser comprado
    /// </summary>
    [Required(ErrorMessage = "O Nome do item é obrigatório")]
    [MaxLength(100, ErrorMessage = "O Nome do item deve ter no maximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Quantidade de item
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "A Quantidade deve ser maior que 0")]
    public int Quantity { get; set; }

    /// <summary>
    /// Preço unitario do item
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitario deve ser maior que 0")]
    public decimal UnitPrice { get; set; }

    [Required]
    public int ShoppingListId { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace ListaComprasAPI.DTOs.ShoppingList;

/// <summary>
/// Dto responsavel por criar uma lista de compras
/// </summary>
public class CreateShoppingListDto
{
    //Nome da lista
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}

using System.ComponentModel.DataAnnotations;

namespace ListaComprasAPI.DTOs.ShoppingList;

/// <summary>
/// Dto responsavel por criar uma lista de compras
/// </summary>
public class CreateShoppingListDto
{
    //Nome da lista
    [Required(ErrorMessage = "O Nome da lista é obrigatório")]
    [MaxLength(100, ErrorMessage = "O Nome da lista pode ter no máximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;
}

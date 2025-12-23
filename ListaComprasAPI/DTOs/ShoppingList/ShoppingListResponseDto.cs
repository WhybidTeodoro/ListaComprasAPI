using System.ComponentModel.DataAnnotations;

namespace ListaComprasAPI.DTOs.ShoppingList;

/// <summary>
/// Dto usado para retorno da lista de compras
/// </summary>
public class ShoppingListResponseDto
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "O Nome da lista é obrigatório")]
    [MaxLength(100, ErrorMessage = "O Nome da lista pode ter no máximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //Valor total calculado dinamicamente
    public decimal TotalValue { get; set; }
}

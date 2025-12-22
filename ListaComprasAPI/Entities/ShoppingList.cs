using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListaComprasAPI.Entities;

/// <summary>
/// Representa a lista de compras pertencente a um usuario
/// </summary>
public class ShoppingList
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; }

    //Foreing key para o usuario
    [Required]
    public int UserId { get; set; }

    //propriedade de navegação
    [ForeignKey(nameof(UserId))]
    public User user { get; set; } = null!;

    public ICollection<ShoppingItem> Items { get; set; } = new List<ShoppingItem>();
}

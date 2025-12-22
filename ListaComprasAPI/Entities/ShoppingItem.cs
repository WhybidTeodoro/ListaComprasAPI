using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListaComprasAPI.Entities;

/// <summary>
/// Representa um item da lista de compras pertencente a um usuario
/// </summary>
public class ShoppingItem
{
    ///<summary>
    ///Identificador unico do item
    ///</summary>
    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Nome do item (Ex: arroz, feijão)
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Quantidade de item compradas
    /// </summary>
    [Required]
    [Range(1,int.MaxValue)]
    public int Quantity { get; set; }
    /// <summary>
    /// Preço unitario do item
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
    /// <summary>
    /// Valor total do item na lista(Quantidade x preço unitario)
    /// Não é armazenado no banco, é calculado.
    /// </summary>
    [NotMapped]
    public decimal TotalPrice => Quantity * UnitPrice;
    /// <summary>
    /// Data da criação do item
    /// </summary>
    [Required]
    public DateTime CreatedAT { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Id do usuario dono do item. 
    /// </summary>
    [Required]
    public int UserId { get; set; }
    /// <summary>
    /// Chave estrangeira da lista
    /// </summary>
    [Required]
    public int ShoppingListId { get; set; }
    //Propriedade de Navegação
    public ShoppingList shoppingList { get; set; } = null!;
}

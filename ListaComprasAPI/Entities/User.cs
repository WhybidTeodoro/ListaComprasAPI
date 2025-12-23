using System.ComponentModel.DataAnnotations;

namespace ListaComprasAPI.Entities;

/// <summary>
/// Representa o Usuario do sistema. Cada usuario possui sua lista de compras
/// </summary>
public class User
{
    /// <summary>
    /// Identificador unico do usuario no banco de dados
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Nome do Usuario
    /// </summary>
    [Required(ErrorMessage = "O Nome é obrigatório")]
    [MaxLength(100, ErrorMessage = "O Nome pode ter no maximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuario. Será usado para o login.
    /// </summary>
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "O email informado não é valido")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hash da senha do usuario
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatóriaa")]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Data da criação do usuario
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}

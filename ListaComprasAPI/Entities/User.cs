namespace ListaComprasAPI.Entities;

/// <summary>
/// Representa o Usuario do sistema. Cada usuario possui sua lista de compras
/// </summary>
public class User
{
    /// <summary>
    /// Identificador unico do usuario no banco de dados
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome do Usuario
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuario. Será usado para o login.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hash da senha do usuario
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Data da criação do usuario
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}

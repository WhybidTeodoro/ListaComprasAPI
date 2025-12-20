using System.ComponentModel.DataAnnotations;

namespace ListaComprasAPI.DTOs;

/// <summary>
/// Dto para registro de novos usuarios. 
/// Contem apenas os dados que o cliente pode enviar
/// </summary>
public class RegisterUserDto
{
    /// <summary>
    /// Nome do usuario
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuario
    /// </summary>
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "O email informado não é valido")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha enviado pelo usuario. 
    /// Será convertida em hash antes de salvar.
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatóriaa")]
    [MinLength(6, ErrorMessage = "A senha deve ter no minimo 6 caracteres")]
    public string Password { get; set; } = string.Empty;
}

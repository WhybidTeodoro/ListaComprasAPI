using System.ComponentModel.DataAnnotations;

namespace ListaComprasAPI.DTOs.User;

///<summary>
///DTO Utilizado para login do usuario.
///</summary>
public class LoginUserDto
{
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "O Email é invalido")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Password { get; set; } = string.Empty;

}

namespace ListaComprasAPI.Entities;

//Representa o Usuario do sistema. Cada usuario possui sua lista de compras
public class User
{
    //Identificador unico do usuario no banco de dados
    public int Id { get; set; }

    //Nome do Usuario
    public string Name { get; set; } = string.Empty;

    //Email do usuario. Será usado para o login.
    public string Email { get; set; } = string.Empty;

    //Hash da senha do usuario
    public string PasswordHash { get; set; } = string.Empty;

    //Data da criação do usuario
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}

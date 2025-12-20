using ListaComprasAPI.Data;
using ListaComprasAPI.DTOs;
using ListaComprasAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ListaComprasAPI.Controllers;

/// <summary>
/// Controller responsavel pela autenticação e cadastro de usuarios.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Construtor com injeção do DbContext
    /// </summary>
   
    public AuthController(AppDbContext context)
    {
        _context = context;
    }
    ///<summary>
    ///Endpoint para cadastro de novos usuarios.
    /// </summary>

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        //Validação automatica do DTO
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        //Verifica se o email ja esta cadastrado
        bool emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (emailExists)
            return BadRequest("Email já está em uso");

        //Gera hash da senha
        string passwordHash = HashPassword(dto.Password);

        //Cria a entidade User
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        //Salva no banco de dados
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        //Retorno dos dados necessarios
        return CreatedAtAction(nameof(Register), new { user.Id }, new { user.Id, user.Name, user.Email });        
    }
    ///<summary>
    ///Gera um hash SHA256 para a senha.
    /// </summary>
    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

        var builder = new StringBuilder();
        foreach(byte b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }

}

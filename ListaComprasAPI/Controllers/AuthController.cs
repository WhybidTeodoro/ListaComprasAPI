using ListaComprasAPI.Data;
using ListaComprasAPI.DTOs.User;
using ListaComprasAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Construtor com injeção do DbContext
    /// </summary>
   
    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
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
    ///Endpoint para login de usuarios
    ///</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            return Unauthorized("Email ou senha invalidos");

        var passwordHash = HashPassword(dto.Password);
        if (user.PasswordHash != passwordHash)
            return Unauthorized("Email ou senha invalidos");

        var token = GenerateJwtToken(user);

        return Ok(new {token});
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

    ///<summary>
    ///Gera o token JWT do usuario autenticado
    ///</summary>
    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("name", user.Name)
    };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(jwtSettings["ExpiresInMinutes"]!)
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }



}

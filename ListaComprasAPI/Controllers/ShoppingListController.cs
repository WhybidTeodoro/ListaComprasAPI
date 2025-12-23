using ListaComprasAPI.Data;
using ListaComprasAPI.DTOs.ShoppingItem;
using ListaComprasAPI.DTOs.ShoppingList;
using ListaComprasAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ListaComprasAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ShoppingListController : ControllerBase
{
    private readonly AppDbContext _context;

    public ShoppingListController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Cria uma lista de compras para um usuario autenticado
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateShoppingListDto dto)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //Obtem o id do usuario a partir do token JWT
        var userId = GetUserIdFromToken();

        var shoppingList = new ShoppingList
        {
            Name = dto.Name.Trim(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.ShoppingLists.Add(shoppingList);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = shoppingList.Id }, MapToResponseList(shoppingList));
    }
    /// <summary>
    /// Obtem a lista especifica do usuario
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserIdFromToken();

        var list = await _context.ShoppingLists
            .FirstOrDefaultAsync(l => l.UserId == userId && l.Id == id);

        if (list == null)
            return NotFound(new { message = "Nenhuma lista de compras foi encontrada" });

        var response = new ShoppingListResponseDto
        {
            Id = list.Id,
            Name = list.Name,
            CreatedAt = list.CreatedAt,
            TotalValue = list.TotalList
        };

        return Ok(response);
    }

    /// <summary>
    /// Atualiza o nome de uma lista
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateShoppingListDto dto)
    {

        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { message = "O Nome da lista não pode ser vazio" });

        var userId = GetUserIdFromToken();

        var list = await _context.ShoppingLists
            .FirstOrDefaultAsync(l => l.UserId == userId && l.Id == id);

        if (list == null)
            return NotFound(new { message = "Nenhuma lista de compras foi encontrada" });

        list.Name = dto.Name.Trim();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Remove a lista de um usuario
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserIdFromToken();

        var list = await _context.ShoppingLists
            .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);

        if (list == null)
            return NotFound(new { message = "Nenhuma lista de compras foi encontrada" });

        _context.ShoppingLists.Remove(list);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Verifica o id do usuario pelo token
    /// </summary>
    private int GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            throw new UnauthorizedAccessException("Token Invalido.");

        return int.Parse(userIdClaim.Value);
    }

    /// <summary>
    /// Retorna o response de shoppinglist
    /// </summary>
    private static ShoppingListResponseDto MapToResponseList(ShoppingList list)
    {
        return new ShoppingListResponseDto
        {
            Id = list.Id,
            Name = list.Name,
            CreatedAt = list.CreatedAt,
            TotalValue = list.TotalList
        };
    }
}

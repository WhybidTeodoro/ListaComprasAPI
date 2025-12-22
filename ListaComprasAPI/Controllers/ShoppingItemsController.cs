using ListaComprasAPI.Data;
using ListaComprasAPI.DTOs.ShoppingItem;
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
public class ShoppingItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    //Injeção de dependencia do dbContext
    public ShoppingItemsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna todos os itens de compra do usuario autenticado
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        int userId = GetUserIdFromToken();

        var items = await _context.ShoppingItems
            .Where(item => item.UserId == userId).Select(item => new ShoppingItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice,
                CreatedAt = DateTime.UtcNow,
            }).ToListAsync();

        return Ok(items);

    }
    /// <summary>
    /// Cria um novo item de compra para um usuario autenticado
    /// </summary>
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateShoppingItemDto dto)
    {
        int userId = GetUserIdFromToken();

        var item = new ShoppingItem
        {
            Name = dto.Name,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            UserId = userId,
            CreatedAT = DateTime.UtcNow
        };
        _context.ShoppingItems.Add(item);
        await _context.SaveChangesAsync();
                
        return CreatedAtAction(nameof(GetAll), new { id = item.Id }, MapToResponseItem(item));
    }

    /// <summary>
    /// Atualiza o item do usuario autenticado
    /// </summary>

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateShoppingItemDto dto)
    {
        int userId = GetUserIdFromToken();

        var item = await _context.ShoppingItems.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId); 

        if(item == null)
            return NotFound("Item não encontrado");

        item.Name = dto.Name;
        item.Quantity = dto.Quantity;
        item.UnitPrice = dto.UnitPrice;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = GetUserIdFromToken();

        var item = await _context.ShoppingItems.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

        if (item == null) 
            return NotFound("Item não encontrado");

        _context.ShoppingItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private int GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            throw new UnauthorizedAccessException("Token Invalido.");

        return int.Parse(userIdClaim.Value);
    }

    private static ShoppingItemResponseDto MapToResponseItem(ShoppingItem item)
    {
        return new ShoppingItemResponseDto
        {
            Id = item.Id,
            Name = item.Name,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            TotalPrice = item.Quantity * item.UnitPrice,
            CreatedAt = item.CreatedAT
        };
    }
}

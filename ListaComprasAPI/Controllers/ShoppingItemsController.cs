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
    public async Task<ActionResult<IEnumerable<ShoppingItemResponseDto>>> GetAll()
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
    public async Task<ActionResult> Create(CreateShoppingItemDto dto)
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

        var response = new ShoppingItemResponseDto
        {
            Id = item.Id,
            Name = item.Name,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            TotalPrice = item.Quantity * item.UnitPrice,
            CreatedAt = DateTime.UtcNow
        };

        return CreatedAtAction(nameof(GetAll), new { id = item.Id }, response);
    }

    private int GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            throw new UnauthorizedAccessException("Token Invalido.");

        return int.Parse(userIdClaim.Value);
    }
}

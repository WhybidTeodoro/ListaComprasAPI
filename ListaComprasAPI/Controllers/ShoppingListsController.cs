using ListaComprasAPI.Data;
using ListaComprasAPI.DTOs.ShoppingLists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ListaComprasAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ShoppingListsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ShoppingListsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserIdFromToken();

        var shoppingList = await _context.ShoppingLists
            .Include(sl => sl.Items)
            .Where(sl => sl.Id == id && sl.UserId == userId)
            .Select(sl => new ShoppingListDetailsDto
            {
                Id = sl.Id,
                Name = sl.Name,
                CreatedAt = sl.CreatedAt,
                Items = sl.Items.Select(item => new ShoppingItemDetailsDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                }).ToList()
            }).FirstOrDefaultAsync();

        if (shoppingList == null)
            return NotFound(new { message = "Lista não encontrada" });

        return Ok(shoppingList);
            
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserIdFromToken();

        var lists = await _context.ShoppingLists
            .Where(sl => sl.UserId == userId)
            .Select(sl => new ShoppingListSumaryDto
            {
                Id = sl.Id,
                Name = sl.Name,
                CreatedAt = sl.CreatedAt,
                TotalList = sl.Items.Any()
                            ? sl.Items.Sum(item => item.Quantity * item.UnitPrice) : 0
            }).ToListAsync();

        return Ok(lists);
    }

    private int GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            throw new UnauthorizedAccessException("Token Invalido.");

        return int.Parse(userIdClaim.Value);
    }
}

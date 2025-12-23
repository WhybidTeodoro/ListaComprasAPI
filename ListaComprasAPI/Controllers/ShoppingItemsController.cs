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
    /// Cria um novo item de compra para um usuario autenticado
    /// </summary>

    [HttpPost]
    public async Task<IActionResult> Create(CreateShoppingItemDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        int userId = GetUserIdFromToken();

        var shoppingList = await _context.ShoppingLists
            .FirstOrDefaultAsync(l => l.Id == dto.ShoppingListId && l.UserId == userId);

        if (shoppingList == null)
            return NotFound(new { message = "Lista de compras não encontrada" });

        var item = new ShoppingItem
        {
            Name = dto.Name,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            ShoppingListId = dto.ShoppingListId,
        };
        _context.ShoppingItems.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, MapToResponseItem(item));
    }

    /// <summary>
    /// Retorna o item pelo id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        int userId = GetUserIdFromToken();

        var item = await _context.ShoppingItems
            .Include(i => i.ShoppingList)
            .FirstOrDefaultAsync(i => i.Id == id && i.ShoppingList.UserId == userId);

        if (item == null)
            return NotFound(new { message = "Item não encontrado" });

        return Ok(MapToResponseItem(item));

    }
    /// <summary>
    /// Retorna itens pendentes de compra
    /// </summary>
    [HttpGet("pendentes")]
    public async Task<IActionResult> GetItensPendentes()
    {
        var userId = GetUserIdFromToken();

        var itensPendentes = await _context.ShoppingItems
            .Where(item => item.IsPurchased == false && item.ShoppingList.UserId == userId)
            .Select(item => new ShoppingItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                IsPurchased = item.IsPurchased
            }).ToListAsync();

        return Ok(itensPendentes);
    }


    /// <summary>
    /// Atualiza o item do usuario autenticado
    /// </summary>

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ShoppingItemUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        int userId = GetUserIdFromToken();

        var item = await _context.ShoppingItems.Include(i => i.ShoppingList)
            .FirstOrDefaultAsync(i => i.Id == id && i.ShoppingList.UserId == userId);

        if (item == null)
            return NotFound(new { message = "Item não encontrado" });

        item.Name = dto.Name;
        item.Quantity = dto.Quantity;
        item.UnitPrice = dto.UnitPrice;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    /// <summary>
    /// Retorna o item como comprado
    /// </summary>
    [HttpPut("{id:int}/purchased")]
    public async Task<IActionResult> MarcarComoComprado(int id)
    {
        var userId = GetUserIdFromToken();

        var item = await _context.ShoppingItems
            .Include(i => i.ShoppingList)
            .FirstOrDefaultAsync(i => i.Id == id && i.ShoppingList.UserId == userId);

        if (item == null)
            return NotFound(new { message = "Item não encontrado" }); 

        item.IsPurchased = !item.IsPurchased;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            item.Id,
            item.Name,
            item.IsPurchased
        });
    }
    /// <summary>
    /// Deleta o item pelo id
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = GetUserIdFromToken();

        var item = await _context.ShoppingItems.Include(i => i.ShoppingList)
            .FirstOrDefaultAsync(i => i.Id == id && i.ShoppingList.UserId == userId);

        if (item == null)
            return NotFound(new { message = "Item não encontrado" });

        _context.ShoppingItems.Remove(item);
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
    /// Retorna o response de shoppingitem
    /// </summary>
    private static ShoppingItemResponseDto MapToResponseItem(ShoppingItem item)
    {
        return new ShoppingItemResponseDto
        {
            Id = item.Id,
            Name = item.Name,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            IsPurchased = item.IsPurchased
        };
    }
}

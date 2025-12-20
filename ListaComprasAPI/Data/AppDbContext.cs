using ListaComprasAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ListaComprasAPI.Data;

/// <summary>
/// Dbcontext principal responsavel por gerenciar a conexão com o banco e mapear as entidades
/// </summary>
public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	/// <summary>
	/// Tabela de usuarios do sistema.
	/// </summary>
	DbSet<User> Users { get; set; }
}

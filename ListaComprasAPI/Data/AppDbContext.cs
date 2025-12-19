using ListaComprasAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ListaComprasAPI.Data;

//Dbcontext principal responsavel por gerenciar a conexão com o banco e mapear as entidades
public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	//Tabela de usuarios do sistema.
	DbSet<User> Users { get; set; }
}

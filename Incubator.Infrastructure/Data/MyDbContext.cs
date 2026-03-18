using Incubator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Incubator.Infrastructure.Data
{
    public class MyDbContext : DbContext
    {
        // Mapeamos la entidad Cliente a la tabla Clientes
        public DbSet<Client> Clients { get; set; }

        // El constructor recibe las opciones inyectadas (donde vendrá la cadena de conexión)
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
    }
}

using Incubator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Incubator.Infrastructure.Data
{
    public class MyDbContext : DbContext
    {
        // El constructor recibe las opciones inyectadas (donde vendrá la cadena de conexión)
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
        }

        public MyDbContext()
        {
                
        }

        // Mapeamos la entidad Cliente a la tabla Clientes
        public DbSet<Client> Clients { get; set; }
        public DbSet<IncubatorFrame> IncubatorFrames { get; set; }

        //Nugets: Microsoft.EntityFrameworkCore.SqlServer
        //Microsoft.EntityFrameworkCore.Tools
        //Microsoft.EntityFrameworkCore.Tools

        //Add-Migration Inicial -Project Incubator.Infrastructure -StartupProject Incubator.Desktop
        //Update-Database -Project Incubator.Infrastructure -StartupProject Incubator.Desktop
    }
}

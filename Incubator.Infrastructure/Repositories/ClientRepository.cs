using Incubator.Application.Interfaces;
using Incubator.Domain.Entities;
using Incubator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Incubator.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        //// Aquí inyectarías tu DbContext (Entity Framework) o HttpClient
        //// public ClienteRepository(MiDbContext dbContext) { ... }

        //public async Task<List<Client>> GetAllAsync()
        //{
        //    // Simulamos una consulta a la base de datos
        //    await Task.Delay(1000);
        //    return new List<Client>
        //    {
        //        new Client { Id = 1, Name = "Empresa Alpha", Email = "contacto@alpha.com" },
        //        new Client { Id = 2, Name = "Servicios Beta", Email = "info@beta.com" }
        //    };
        //}
        private readonly MyDbContext _context;

        // Inyectamos el DbContext
        public ClientRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetAllAsync()
        {
            // Esto ahora consulta a la base de datos real
            // Simulo un delay pequeño solo para que sigas viendo la animación fluida en la UI
            await Task.Delay(500);
            return await _context.Clients.ToListAsync();
        }
    }
}

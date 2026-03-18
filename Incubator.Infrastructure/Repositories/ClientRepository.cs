using Incubator.Application.Interfaces;
using Incubator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incubator.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        // Aquí inyectarías tu DbContext (Entity Framework) o HttpClient
        // public ClienteRepository(MiDbContext dbContext) { ... }

        public async Task<List<Client>> GetAllAsync()
        {
            // Simulamos una consulta a la base de datos
            await Task.Delay(1000);
            return new List<Client>
        {
            new Client { Id = 1, Name = "Empresa Alpha", Email = "contacto@alpha.com" },
            new Client { Id = 2, Name = "Servicios Beta", Email = "info@beta.com" }
        };
        }
    }
}

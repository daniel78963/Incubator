using System;
using System.Collections.Generic;
using System.Text;
using Incubator.Domain.Entities;

namespace Incubator.Application.Interfaces
{
    // La capa de aplicación dice: "Necesito que alguien me traiga clientes, no me importa cómo"
    public interface IClientRepository
    {
        Task<List<Client>> GetAllAsync();
    }
}

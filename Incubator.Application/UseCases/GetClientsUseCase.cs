using Incubator.Application.Interfaces;
using Incubator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incubator.Application.UseCases
{ 
    public interface IGetClientsUseCase
    {
        Task<List<Client>> EjecutarAsync();
    }

    public class GetClientsUseCase : IGetClientsUseCase
    {
        private readonly IClientRepository _repository;

        // Inyectamos la interfaz del repositorio
        public GetClientsUseCase(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Client>> EjecutarAsync()
        {
            // Aquí irían validaciones, transformaciones, o lógica de negocio antes de devolver los datos
            var clientes = await _repository.GetAllAsync();
            return clientes.Where(c => c.IsValid()).ToList();
        }
    }
}

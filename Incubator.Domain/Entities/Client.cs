using System;
using System.Collections.Generic;
using System.Text;

namespace Incubator.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Aquí también iría la lógica de negocio pura de la entidad
        public bool IsValid() => !string.IsNullOrWhiteSpace(Name);
    }
}

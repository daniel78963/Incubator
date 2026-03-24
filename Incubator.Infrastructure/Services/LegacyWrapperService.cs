using Incubator.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Incubator.Infrastructure.Services
{
    //public class LegacyWrapperService : ILegacyCalculatorService
    public class LegacyWrapperService  
    {
        public async Task<int> CalculateComplexValueAsync(int input)
        {
            // En lugar de cargar la DLL directamente, lanzamos nuestro mini-programa x86
            // de forma invisible y le pasamos el dato.

            var startInfo = new ProcessStartInfo
            {
                FileName = "Ruta/Al/MiniProgramaX86.exe", // Tu ejecutable puente
                Arguments = input.ToString(),
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true // Oculto para el usuario
            };

            using var process = Process.Start(startInfo);
            if (process == null) throw new Exception("No se pudo iniciar el proceso legacy.");

            // Leemos la respuesta que el mini-programa x86 imprime en su consola
            string output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (int.TryParse(output, out int result))
            {
                return result;
            }

            throw new Exception("Error procesando la DLL legacy.");
        }
    }
}

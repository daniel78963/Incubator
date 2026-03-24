using Incubator.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Incubator.Infrastructure.Services
{
    public class LegacyCalculatorService : ILegacyCalculatorService
    {
        public async Task<int> CalculateComplexValueAsync(int input, CancellationToken cancellationToken = default)
        {
            // 1. Buscamos el ejecutable en la misma carpeta donde corre la app WPF
            string exeName = "Incubator.LegacyBridge.exe";
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string exePath = Path.Combine(basePath, exeName);

            if (!File.Exists(exePath))
            {
                throw new FileNotFoundException($"No se encontró el ejecutable legacy en: {exePath}. Asegúrate de que el proyecto Incubator.LegacyBridge compila en la misma carpeta o cópialo.");
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = input.ToString(), // Pasamos el dato como argumento
                RedirectStandardOutput = true, // Capturar el Console.WriteLine
                RedirectStandardError = true,  // Capturar el Console.Error.WriteLine
                UseShellExecute = false,       // Requerido para redirigir salidas
                CreateNoWindow = true          // Totalmente invisible para el usuario
            };

            using var process = new Process { StartInfo = startInfo };

            try
            {
                process.Start();

                // 2. Leemos la salida y los errores de forma asíncrona
                Task<string> readOutputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
                Task<string> readErrorTask = process.StandardError.ReadToEndAsync(cancellationToken);

                // 3. Esperamos a que el proceso termine, respetando el CancellationToken
                await process.WaitForExitAsync(cancellationToken);

                string errorOutput = await readErrorTask;
                string standardOutput = await readOutputTask;

                // 4. Evaluamos cómo terminó el proceso
                if (process.ExitCode != 0 || !string.IsNullOrWhiteSpace(errorOutput))
                {
                    throw new Exception($"El proceso legacy falló. Detalle: {errorOutput.Trim()}");
                }

                // 5. Parseamos el resultado
                if (int.TryParse(standardOutput.Trim(), out int result))
                {
                    return result;
                }

                throw new Exception($"Formato de respuesta inválido desde la DLL vieja: '{standardOutput}'");
            }
            catch (OperationCanceledException)
            {
                // Si el usuario cancela o hay timeout, intentamos matar el proceso zombi
                if (!process.HasExited)
                {
                    process.Kill();
                }
                throw new TimeoutException("La operación legacy tardó demasiado y fue cancelada.");
            }
        }
    }
}

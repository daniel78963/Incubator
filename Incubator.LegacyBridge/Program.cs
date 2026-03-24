// Incubator.LegacyBridge/Program.cs (Compilado estrictamente en x86)
using System;
using System.Runtime.InteropServices;

//class Program
//{
//    // AQUÍ SÍ puedes importar la DLL vieja porque este proceso es de 32 bits.
//    [DllImport("LibreriaAntigua.dll", CallingConvention = CallingConvention.Cdecl)]
//    public static extern int FuncionViejaX86(int valor);

//    static void Main(string[] args)
//    {
//        if (args.Length > 0 && int.TryParse(args[0], out int input))
//        {
//            // Llamamos a la DLL
//            int resultado = FuncionViejaX86(input);

//            // Imprimimos el resultado (que será leído por la capa de Infrastructure)
//            Console.WriteLine(resultado);
//        }
//    }
//}
class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                // Si no hay argumentos, escribimos al StandardError (que será capturado por WPF)
                Console.Error.WriteLine("Error: No se proporcionó ningún parámetro de entrada.");
                Environment.Exit(1);
            }

            if (int.TryParse(args[0], out int input))
            {
                // SIMULACIÓN: Aquí llamarías a tu [DllImport] vieja.
                // Vamos a simular que el cálculo tarda 2 segundos.
                Thread.Sleep(2000);

                //int result = input * 100; // Cálculo ficticio

                //// Escribimos el resultado en el StandardOutput (que será leído por WPF)
                //Console.WriteLine(result);
                //Environment.Exit(0); // Éxito

                Console.Error.WriteLine($"Excepción fatal en LegacyBridge: DAR");
                Environment.Exit(1);
            }
            else
            {
                Console.Error.WriteLine("Error: El parámetro debe ser un número entero.");
                Environment.Exit(1);
            }
        }
        catch (Exception ex)
        {
            // Cualquier fallo inesperado se va al flujo de error
            Console.Error.WriteLine($"Excepción fatal en LegacyBridge: {ex.Message}");
            Environment.Exit(1);
        }
    }
}
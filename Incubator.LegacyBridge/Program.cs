// Incubator.LegacyBridge/Program.cs (Compilado estrictamente en x86)
using System;
using System.Runtime.InteropServices;

class Program
{
    // AQUÍ SÍ puedes importar la DLL vieja porque este proceso es de 32 bits.
    [DllImport("LibreriaAntigua.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int FuncionViejaX86(int valor);

    static void Main(string[] args)
    {
        if (args.Length > 0 && int.TryParse(args[0], out int input))
        {
            // Llamamos a la DLL
            int resultado = FuncionViejaX86(input);

            // Imprimimos el resultado (que será leído por la capa de Infrastructure)
            Console.WriteLine(resultado);
        }
    }
}
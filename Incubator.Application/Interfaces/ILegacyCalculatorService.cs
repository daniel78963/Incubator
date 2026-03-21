using System;
using System.Collections.Generic;
using System.Text;

namespace Incubator.Application.Interfaces
{
    // Tu aplicación solo sabe que necesita hacer un cálculo que, 
    // bajo el capó, requiere esa DLL antigua.
    public interface ILegacyCalculatorService
    {
        Task<int> CalculateComplexValueAsync(int input);
    }
}

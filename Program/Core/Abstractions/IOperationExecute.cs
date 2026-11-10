using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCLI.Program.Core.Abstractions
{
    internal interface IOperationExecute<T>
    {
        T Execute(T[] input);
        bool TryConvertToOprType(string raw, out T operands);
        bool TryConvertToOprType(List<string> raw, out List<T>? operands);
    }
}

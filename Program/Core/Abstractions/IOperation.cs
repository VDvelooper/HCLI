using HCLI.Program.Operations;

namespace HCLI.Program.Core.Abstractions
{
    internal interface IOperation
    {
        string OperationSymbol { get; }
        int OperandCount { get; }
        bool OperandBeforeSymbol { get; }
        OperationData? Execute(List<string> input);
        bool TryConvertToOprType(string raw, out OperationData operands);
        bool TryConvertToOprType(List<string> raw, out List<OperationData>? operands);
    }
}
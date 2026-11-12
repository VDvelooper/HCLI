namespace HCLI.Program.Core.Abstractions
{
    internal interface IOperationExecute<T> : IOperation
    {
        Action<T> ExecuteMethod { get; }
        T Execute(T[] input);
        bool TryConvertToOprType(string raw, out T operands);
        bool TryConvertToOprType(List<string> raw, out List<T>? operands);
    }
}

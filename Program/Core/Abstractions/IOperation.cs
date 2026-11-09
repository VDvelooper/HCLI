namespace HCLI.Program.Core.Abstractions
{
    internal interface IOperation
    {
        string OperationSymbol { get; }
        int OperandCount { get; }
    }
}
using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Operations.Types
{
    internal class Decrementation : IOperationExecute<int>, IOperation
    {
        public string OperationSymbol { get; private set; }
        public int OperandCount { get; private set; }

        public Decrementation()
        {
            OperationSymbol = "--";
            OperandCount = 1;
        }

        public int Execute(int[] input)
        {
            return input[0]--; 
        }
    }
}

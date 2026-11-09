using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Operations.Types
{
    internal class Incrementation : IOperationExecute<int>, IOperation
    {
        public string OperationSymbol { get; private set; }
        public int OperandCount { get; private set; }

        public Incrementation()
        {
            OperationSymbol = "++";
            OperandCount = 1;
        }

        public int Execute(int[] input)
        {
            return input[0]++;   
        }
    }
}

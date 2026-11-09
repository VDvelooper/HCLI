using HCLI.Program.Operations.Types;

namespace HCLI.Program.Operations
{
    internal class OperationsRegistry
    {
        public Dictionary<string, Core.Abstractions.IOperation> UnaryOps { get; set; }
        public Dictionary<string, Core.Abstractions.IOperation> BinaryOps { get; set; }
        public Dictionary<string, Core.Abstractions.IOperation> TrinaryOps { get; set; }

        internal OperationsRegistry()
        {
            UnaryOps = new Dictionary<string, Core.Abstractions.IOperation>();
            BinaryOps = new Dictionary<string, Core.Abstractions.IOperation>();
            TrinaryOps = new Dictionary<string, Core.Abstractions.IOperation>();

            Incrementation ops_incrementation = new Types.Incrementation();
            UnaryOps.Add(ops_incrementation.OperationSymbol, ops_incrementation);

            Decrementation ops_decrementation = new Types.Decrementation();
            UnaryOps.Add(ops_decrementation.OperationSymbol, ops_decrementation);

        }


        public bool TryGetOperation(string operation, out Core.Abstractions.IOperation? foundOp)
        {
            if (UnaryOps.ContainsKey(operation))
            {
                foundOp = UnaryOps[operation];
                return true;
            }
            else if (BinaryOps.ContainsKey(operation))
            {
                foundOp = BinaryOps[operation];
                return true;
            }
            else if (TrinaryOps.ContainsKey(operation))
            {
                foundOp = TrinaryOps[operation];
                return true;
            }

            foundOp = null;
            return false;
        }

        public bool IsCharPartOfMultiSymbOp(char c)
        {
            bool b1 = UnaryOps.Where(p => p.Value.OperationSymbol.Contains(c) && p.Value.OperationSymbol.Length > 1).Any();
            bool b2 = BinaryOps.Where(p => p.Value.OperationSymbol.Contains(c) && p.Value.OperationSymbol.Length > 1).Any();
            //bool b3 = TrinaryOps.Where(p => p.Value.OperationSymbol.Contains(c) && p.Value.OperationSymbol.Length > 1).Any();
            return b1 || b2 /*|| b3*/;
        }

    }
}

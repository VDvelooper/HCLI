using HCLI.Program.Abstractions;
using HCLI.Program.Core;
using HCLI.Program.Core.Shared.Feedback.Error;
using HCLI.Program.Core.Shared.Feedback.Expecteds;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Modules.ExampleModule
{
    public class ExampleModule : ModuleBase
    {
        public ExampleModule(bool isSeparateMode) : base("ModuleExample", "moduleexample", isSeparateMode) { }

        public override IFeedback ModuleBody(UserInput userInput) 
        {
            string newRawInput = ConsoleView.ReadUserInput(ModuleName);

            UserInput? newUserInput = Program.Core.CommandParser.ParseForModules(newRawInput, this);

            if (newUserInput != null)
            { 
                Program.Core.CommandExecuter.ExecuteCommand(userInput);
                return new ExpectedInput();
            }

            return new InternalNullExeption();
        }

        public override void Entry()
        {
            base.Entry();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

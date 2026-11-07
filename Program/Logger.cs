using ModuleContracts.Feedback;

namespace HCLI.Program
{
    internal class Logger
    {
        private string _logsPath { get; }

        internal List<IFeedback> SavedLogs { get; private set; }

        public Logger()
        {
            _logsPath = @$"{Runtime.ConfigManager.MainDirectoryPath}\Logs";
            
        }

        public void RegisterLog(IFeedback feedback)
        {
            SavedLogs.Add(feedback);
        }

    }
}

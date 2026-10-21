using HCLI.Program.Abstractions;

namespace HCLI.Program.Core
{
    public class ConsoleView
    {
        public void PresentFeedback(IFeedback feedback)
        {
            if (feedback.Silent) return;

            switch (feedback.ResultType)
            {
                case Shared.Feedback.FeedbackType.Expected:

                    if (feedback.ConsoleOutputOverride != null)
                        Message(feedback.ConsoleOutputOverride, true, feedback.ConsoleOutputStyle);
                    else if (feedback.Description != null)
                        Debug($"{feedback.FeedbackID}: {feedback.Description}");
                    break;


                case Shared.Feedback.FeedbackType.Warning:

                    if (feedback.Description != null)
                        Warn($"{feedback.Description} : {feedback.FeedbackID}");
                    else if (feedback.ConsoleOutputOverride != null)
                        Warn($"{feedback.ConsoleOutputOverride} (origin: {feedback.FeedbackID})");
                    else
                        Warn($"{feedback.FeedbackID}");
                    break;


                case Shared.Feedback.FeedbackType.Error:

                    if (feedback.Description != null)
                        Error($"{feedback.Description} : {feedback.FeedbackID}");
                    else if (feedback.ConsoleOutputOverride != null)
                        Error($"{feedback.ConsoleOutputOverride} (origin: {feedback.FeedbackID})");
                    else
                        Error($"{feedback.FeedbackID}");
                    break;


                default:
                    Debug($"MENTOOV: ResoultType was not found and was not able to be handeled by ConsoleView.PresentFeedback.");
                    break;
            }
        }

        public void Message(string? message = null, bool newLine = true, ConsoleOutputStyle? outputStyle = null)
        {
            if (outputStyle != null) {
                Console.ForegroundColor = outputStyle.ForegroundColor;
                Console.BackgroundColor = outputStyle.BackgroundColor;
            }
            Console.Write(message == null ? "" : message);
            Console.ResetColor();
            if (newLine) Console.WriteLine();
        }
        public void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine();
        }
        public void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine();
        }


        public string ReadUserInput(string senderName)
        {
            Message($"{senderName} > ", false);
            string? r = Console.ReadLine();

            Message();

            return r ?? string.Empty;
        }

    }
}

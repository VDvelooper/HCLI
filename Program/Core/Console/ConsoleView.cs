using System;
using ModuleContracts.ServiceContracts;
using ModuleContracts.Abstractions;
using ModuleContracts.Feedback;
using ModuleContracts.Console.Output.Tables;

namespace HCLI.Program.Core.Console
{
    internal class ConsoleView : IConsoleViewContract
    {
        public void PresentFeedback(IFeedback feedback)
        {
            if (feedback.Silent) return;

            switch (feedback.ResultType)
            {
                case FeedbackType.Expected:

                    if (feedback.ConsoleOutputOverride != null)
                        Message(feedback.ConsoleOutputOverride, true, feedback.ConsoleOutputStyle);
                    else if (feedback.Description != null)
                        Debug($"{feedback.FeedbackID}: {feedback.Description}");
                    break;


                case FeedbackType.Warning:

                    if (feedback.Description != null)
                        Warn($"{feedback.Description} : {feedback.FeedbackID}");
                    else if (feedback.ConsoleOutputOverride != null)
                        Warn($"{feedback.ConsoleOutputOverride} ({feedback.FeedbackID})");
                    else
                        Warn($"{feedback.FeedbackID}");
                    break;


                case FeedbackType.Error:

                    if (feedback.Description != null)
                    {
                        Error($"{feedback.Description} : {feedback.FeedbackID}");
                        Error($"Source: {feedback.Details.SourceName}\n" +
                            $"Source path: {feedback.Details.SourceFilePath}\n" +
                            $"Source line: {feedback.Details.SourceLineNumber}");
                    }
                    else if (feedback.ConsoleOutputOverride != null)
                        Error($"{feedback.ConsoleOutputOverride} ({feedback.FeedbackID})");
                    else
                    {
                        Error($"{feedback.FeedbackID}");
                        Error($"Source: {feedback.Details.SourceName}\n" +
                            $"Source path: {feedback.Details.SourceFilePath}\n" +
                            $"Source line: {feedback.Details.SourceLineNumber}");
                    }
                    break;


                default:
                    Debug($"MENTOOV: ResoultType was not found and was not able to be handeled by ConsoleView.PresentFeedback.");
                    break;
            }
        }

        public void Message(string? message = null, bool newLine = true, ConsoleOutputStyle? outputStyle = null)
        {
            if (outputStyle != null) {
                System.Console.ForegroundColor = outputStyle.ForegroundColor;
                System.Console.BackgroundColor = outputStyle.BackgroundColor;
            }
            System.Console.Write(message == null ? "" : message);
            System.Console.ResetColor();
            if (newLine) System.Console.WriteLine();
        }
        public void Debug(string message, bool newLine = true)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.WriteLine(message);
            System.Console.ResetColor();
            if (newLine) System.Console.WriteLine();
        }
        public void Debug(ConsoleTable table)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.BackgroundColor = ConsoleColor.Black;

            if (table.ContainsHeader)
            {
                for (int headerItem = 0; headerItem < table.Header.Length; headerItem++)
                {

                }
            }

            for (int row = 0; row < table.RowCount; row++)
            {
                for (int col = 0; col < table.RowCount; col++)
                {
                    (int, int) position = (row, col);

                    

                    Runtime.ConsoleView.Message($"{table.Body[]}");

                }
            }
            
            System.Console.ResetColor();
            if (newLine) System.Console.WriteLine();
        }
        public void Warn(string message, bool newLine = true)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.WriteLine(message);
            System.Console.ResetColor();
            if (newLine) System.Console.WriteLine();
        }
        public void Error(string message, bool newLine = true)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.WriteLine(message);
            System.Console.ResetColor();
            if (newLine) System.Console.WriteLine();
        }
        public void FatalError(string message, bool newLine = true)
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.BackgroundColor = ConsoleColor.DarkRed;
            System.Console.WriteLine(message);
            System.Console.ResetColor();
            if (newLine) System.Console.WriteLine();
        }

        public string ReadUserInput(string senderName)
        {
            Message($"{senderName} > ", false);
            string? r = System.Console.ReadLine();

            Message();

            return r ?? string.Empty;
        }


        public ConsoleTable CreateTable(string name, int rows, int colums, bool header)
        {
            ConsoleTable table = new ConsoleTable(name, rows, colums, true);
            return table;
        }
    }
}

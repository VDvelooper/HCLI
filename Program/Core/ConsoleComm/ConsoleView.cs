using ModuleContracts.ServiceContracts;
using ModuleContracts.Abstractions;
using ModuleContracts.Feedback;
using ModuleContracts.Console.Output.Tables;

namespace HCLI.Program.Core.ConsoleComm
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
                Console.ForegroundColor = outputStyle.ForegroundColor;
                Console.BackgroundColor = outputStyle.BackgroundColor;
            }

            Console.Write(message == null ? "" : message);

            Console.ResetColor();
            if (newLine) Console.WriteLine();
        }
        public void Message(ConsoleTable table, bool newLine = true, ConsoleOutputStyle? outputStyle = null)
        {
            if (outputStyle != null) {
                Console.ForegroundColor = outputStyle.ForegroundColor;
                Console.BackgroundColor = outputStyle.BackgroundColor;
            }

            PresentTable(table);

            Console.ResetColor();
            if (newLine) Console.WriteLine();
        }

        public void Debug(string message, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.WriteLine(message);

            Console.ResetColor();
            if (newLine) Console.WriteLine();
        }
        public void Debug(ConsoleTable table, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkGray;

            PresentTable(table);

            Console.ResetColor();
            if (newLine) Console.WriteLine();
        }

        public void Warn(string message, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.WriteLine(message);

            Console.ResetColor();
            if (newLine) Console.WriteLine();
        }
        public void Warn(ConsoleTable table, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.Black;

            PresentTable(table);

            Console.ResetColor();
            if (newLine) Console.WriteLine();
        }

        public void Error(string message, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.WriteLine(message);

            Console.ResetColor();
            if (newLine) Console.WriteLine();
        }
        public void Error(ConsoleTable table, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.Black;

            PresentTable(table);

            Console.ResetColor();
            if (newLine) Console.WriteLine();
        }

        public void FatalError(string message, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
            if (newLine) Console.WriteLine();
        }
        public void FatalError(ConsoleTable table, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;

            PresentTable(table);

            Console.ResetColor();
            if (newLine) Console.WriteLine();
        }


        public string ReadUserInput(string senderName)
        {
            Message($"{senderName} > ", false);
            string? r = Console.ReadLine();

            Message();

            return r ?? string.Empty;
        }


        public ConsoleTable CreateTable(int rows, int colums, bool header)
        {
            ConsoleTable table = new ConsoleTable(rows, colums, true);
            return table;
        }

        private void PresentTable(ConsoleTable table)
        {
            string padding = new string(' ', table.HorizontalPadding);

            Console.WriteLine($"|{new string('-', table.TableWidth)}|");
            for (int row = 0; row < table.RowCount; row++)
            {
                if (table.IsRowEmpty(row)) continue;

                for (int col = 0; col < table.ColCount; col++)
                {
                    padding = new string(' ', table.HorizontalPadding);

                    string currentItem = table.Body[(row, col)];

                    int maxItemLength = table.GetColMaxItemLength(col);

                    if (col == 0)
                    {
                        if (currentItem.Length == maxItemLength)
                        {
                            Console.Write($"|{currentItem}|");
                            continue;
                        }

                        int remainingSpaceToPlace = maxItemLength - currentItem.Length;

                        if (remainingSpaceToPlace % 2 == 0)
                        {
                            padding = new string(' ', table.HorizontalPadding + remainingSpaceToPlace / 2);
                            Console.Write($"|{padding}{currentItem}{padding}|");
                        }
                        else
                        {
                            padding = new string(' ', table.HorizontalPadding + remainingSpaceToPlace % 2);
                            string extraPadding = new string(' ', remainingSpaceToPlace - remainingSpaceToPlace % 2);
                            Console.Write($"|{padding}{currentItem}{extraPadding}|");
                        }

                        continue;
                    }
                    else
                    {
                        if (currentItem.Length == maxItemLength)
                        {
                            Console.Write($"{currentItem}|");
                            continue;
                        }

                        int remainingSpaceToPlace = maxItemLength - currentItem.Length;

                        if (remainingSpaceToPlace % 2 == 0)
                        {
                            padding = new string(' ', table.HorizontalPadding + remainingSpaceToPlace / 2);
                            Console.Write($"{padding}{currentItem}{padding}|");
                        }
                        else
                        {
                            padding = new string(' ', table.HorizontalPadding + remainingSpaceToPlace % 2);
                            string extraPadding = new string(' ', remainingSpaceToPlace - remainingSpaceToPlace % 2);
                            Console.Write($"{padding}{currentItem}{extraPadding}|");
                        }
                    }

                }
                Console.WriteLine($"\n|{new string('-', table.TableWidth)}|");
            }
        }
    }
}

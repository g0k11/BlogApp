using Serilog;
using System;
using System.Linq;

namespace BlogApp.Common.Logging
{
    public static class SeriLogger
    {
        public static void Information(string messageTemplate, params object[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    Log.Information(messageTemplate);
                }
                else
                {
                    Log.Information(messageTemplate, args);
                }
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while creating the log. Original message: {Message}, Error: {Error}",
                    messageTemplate, ex.Message);
            }
        }

        public static void Error(Exception ex, string messageTemplate, params object[] args)
        {
            try
            {
                var stackLines = ex.StackTrace?
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Take(2)
                    .ToArray();

                var shortStackTrace = stackLines != null && stackLines.Length > 0
                    ? string.Join(Environment.NewLine, stackLines)
                    : "Stack trace not found.";

                if (args == null || args.Length == 0)
                {
                    Log.Error(
                        "{ErrorType}: {LogMessage}\nError Message: {ExceptionMessage}\nStack Trace (First 2 lines):\n{StackTrace}",
                        ex.GetType().Name,
                        messageTemplate,
                        ex.Message,
                        shortStackTrace
                    );
                }
                else
                {
                    Log.Error(
                        "{ErrorType}: {LogMessage}\nError Message: {ExceptionMessage}\nStack Trace (First 2 lines):\n{StackTrace}",
                        ex.GetType().Name,
                        messageTemplate,
                        ex.Message,
                        shortStackTrace,
                        args
                    );
                }

                if (ex.InnerException != null)
                {
                    Log.Error("Inner Exception: {InnerMessage}", ex.InnerException.Message);
                }
            }
            catch (Exception logEx)
            {
                Log.Error("Logging error: {LogError}. Original error: {OriginalError}",
                    logEx.Message,
                    ex.Message);
            }
        }

        public static void Warning(string messageTemplate, params object[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    Log.Warning(messageTemplate);
                }
                else
                {
                    Log.Warning(messageTemplate, args);
                }
            }
            catch (Exception ex)
            {
                Log.Warning("An error occurred while creating the log. Original message: {Message}, Error: {Error}",
                    messageTemplate, ex.Message);
            }
        }

        public static void Debug(string messageTemplate, params object[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    Log.Debug(messageTemplate);
                }
                else
                {
                    Log.Debug(messageTemplate, args);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("An error occurred while creating the log. Original message: {Message}, Error: {Error}",
                    messageTemplate, ex.Message);
            }
        }
    }
}
using System.Runtime.CompilerServices;

namespace SYMB2C.Foundation.Logging.Repositories
{
    public interface ILogRepository
    {
        void Debug(string message);

        void Debug(string message, params object[] args);

        void Error(string message);

        void SingleError(string message);

        void SingleWarn(string message); 

        void Info(string message);

        void Info(string message, params object[] args);

        void Warn(string message);

        void Fatal(string message);

        void LogFormattedError(string stackTrace, [CallerMemberName] string MethodName = "",
       [CallerFilePath] string FilePath = "",
       [CallerLineNumber] int sourceLineNumber = 0);
    }
}

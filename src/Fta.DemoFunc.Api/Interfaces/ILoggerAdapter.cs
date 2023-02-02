using System;

namespace Fta.DemoFunc.Api.Interfaces
{
    public interface ILoggerAdapter<TType>
    {
        void LogError(Exception exception, string message);
        void LogError(string message);
        void LogInformation(string message);
    }
}

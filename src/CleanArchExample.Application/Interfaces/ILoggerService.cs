namespace CleanArchExample.Application.Interfaces.Services
{
    public interface ILoggerService<T>
    {
        void LogInfo(string message);
        void LogError(string message, Exception ex);
    }
}
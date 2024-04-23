namespace webapi_learning.Helpers.Core
{
    public interface ICustomLogger<T>
    {
        void LogInformation(string message);
        void LogError(Exception ex, string  message);
    }
}

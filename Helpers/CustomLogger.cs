using webapi_learning.Helpers.Core;

namespace webapi_learning.Helpers
{
    public class CustomLogger<T> : ICustomLogger<T>
    {
        private readonly ILogger<T> _logger;
        private readonly IWebHostEnvironment _env;

        public CustomLogger(ILogger<T> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public void LogInformation(string message)
        {
            if (_env.IsDevelopment())
            {
                _logger.LogInformation(message);
            }
        }

        public void LogError(Exception ex, string message)
        {
            if (_env.IsDevelopment())
            {
                _logger.LogError(ex, message);
            }
        }
    }
}

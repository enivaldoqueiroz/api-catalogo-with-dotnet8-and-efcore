using System.Collections.Concurrent;

namespace api_catalogo.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly CustomLoggerProviderConfiguration loggerConfig;
        readonly ConcurrentDictionary<string, CustomerLogger> loggers = new ConcurrentDictionary<string, CustomerLogger>();
        private CustomLoggerProviderConfiguration customLoggerProviderConfiguration;

        public CustomLoggerProvider(CustomLoggerProviderConfiguration customLoggerProviderConfiguration)
        {
            this.customLoggerProviderConfiguration = customLoggerProviderConfiguration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return (ILogger)loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerConfig));
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }
}

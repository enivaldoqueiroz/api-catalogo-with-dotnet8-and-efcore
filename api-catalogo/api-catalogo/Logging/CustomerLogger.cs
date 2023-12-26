
namespace api_catalogo.Logging
{
    public class CustomerLogger : ILogger
    {
        private string name;
        private CustomLoggerProviderConfiguration loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfiguration loggerConfig)
        {
            this.name = name;
            this.loggerConfig = loggerConfig;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

            EscreverTextNoArquivo(message);
        }

        private void EscreverTextNoArquivo(string message)
        {
            string caminhoArquivo = @"C:\dados\log\api_log.txt";
            using (StreamWriter streamWriter = new StreamWriter(caminhoArquivo, true))
            {
                try
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}

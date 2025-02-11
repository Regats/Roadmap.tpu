namespace RoadmapDesigner.Server.Decorators
{
    public class TimeStampLoggerDecorator<T> : ILogger<T>
    {
        private readonly ILogger<T> _innerLogger;

        public TimeStampLoggerDecorator(ILogger<T> innerLogger)
        {
            _innerLogger = innerLogger;
        }

        public IDisposable BeginScope<TState>(TState state) => _innerLogger.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel) => _innerLogger.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Добавляем метку времени к сообщению
            var timeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var message = $"[{timeStamp}] {formatter(state, exception)}";

            // Выводим сообщение в лог с добавлением времени
            _innerLogger.Log(logLevel, eventId, message, exception, (state, ex) => message);
        }
    }

}

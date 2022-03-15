using Cart.Api.Middlewares;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using ILogger = Serilog.ILogger;

namespace Cart.Api.Extensions;

public static class LoggerExtensions
{
    public static void AddLogger(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
            }).CreateBootstrapLogger();
    }
    
    public static ILogger GetLogger(this ILogger logger, LogModel model)
    {
        if (model == null)
            return logger;

        logger = logger
            .ForContext(nameof(model.MachineName), model.MachineName)
            .ForContext(nameof(model.RequestHost), model.RequestHost)
            .ForContext(nameof(model.RequestProtocol), model.RequestProtocol)
            .ForContext(nameof(model.RequestMethod), model.RequestMethod)
            .ForContext(nameof(model.ResponseStatusCode), model.ResponseStatusCode)
            .ForContext(nameof(model.RequestPath), model.RequestPath)
            .ForContext(nameof(model.RequestPathAndQuery), model.RequestPathAndQuery);

        if (model.RequestHeaders != null && model.RequestHeaders.Any())
            logger = logger.ForContext(nameof(model.RequestHeaders), model.RequestHeaders, true);

        if (model.ElapsedMilliseconds != null)
            logger = logger.ForContext(nameof(model.ElapsedMilliseconds), model.ElapsedMilliseconds);

        if (!string.IsNullOrEmpty(model.RequestBody))
            logger = logger.ForContext(nameof(model.RequestBody), model.RequestBody);

        if (model.Exception != null) logger = logger.ForContext(nameof(model.Exception), model.Exception, true);

        if (!string.IsNullOrEmpty(model.Data))
            logger = logger.ForContext(nameof(model.Data), model.Data);

        return logger;
    }
}
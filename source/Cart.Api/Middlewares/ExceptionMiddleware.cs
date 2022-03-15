using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cart.Api.Extensions;
using Cart.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Cart.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ILogger Logger = Log.ForContext<ExceptionMiddleware>();

    private const string ErrorTemplate = $"ERROR: HTTP {"{" + nameof(LogModel.RequestMethod) + "}"} / {"{" + nameof(LogModel.RequestPathAndQuery) + "}"} responded as {"{" + nameof(LogModel.ResponseStatusCode) + "}"}";
    private const string BadRequestTemplate = $"BAD REQUEST: HTTP {"{" + nameof(LogModel.RequestMethod) + "}"} / {"{" + nameof(LogModel.RequestPathAndQuery) + "}"} responded as {"{" + nameof(LogModel.ResponseStatusCode) + "}"}";

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await BadRequest(context, ex);
        }
        catch (Exception ex)
        {
            await InternalServerError(context, ex, ex.Message);
        }
    }

    private static async Task BadRequest(HttpContext context, ValidationException exception, string contentType = "application/json")
    {
        var responseBody = JsonSerializer.Serialize(exception.Errors, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        });
    
        await Task.Run(() =>
        {
            var request = context.Request;
            var encodedPathAndQuery = request.GetEncodedPathAndQuery();
    
            var logModel = new LogModel(request.Host.Host, request.Protocol, request.Method, request.Path, encodedPathAndQuery, StatusCodes.Status400BadRequest)
            {
                RequestHeaders = request.Headers.ToDictionary(x => x.Key, x => (object) x.Value.ToString()),
                RequestBody = string.Empty,
                Data = responseBody
            };
            
            Logger.GetLogger(logModel).Warning(BadRequestTemplate);
        });
    
        context.Response.Clear();
        context.Response.ContentType = contentType;
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(responseBody, Encoding.UTF8);
    }

    private static async Task InternalServerError(HttpContext context, Exception exception, string data, string contentType = "text/plain")
    {
        await Task.Run(() =>
        {
            var request = context.Request;
            var encodedPathAndQuery = request.GetEncodedPathAndQuery();
    
            var logModel = new LogModel(request.Host.Host, request.Protocol, request.Method, request.Path,
                encodedPathAndQuery, StatusCodes.Status500InternalServerError)
            {
                RequestHeaders = request.Headers.ToDictionary(x => x.Key, x => (object) x.Value.ToString()),
                RequestBody = string.Empty,
                Exception = exception,
                Data = data
            };
    
            Logger.GetLogger(logModel).Error(ErrorTemplate);
        });
    
        context.Response.Clear();
        context.Response.ContentType = contentType;
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync("An unexpected error has occurred on the server.", Encoding.UTF8);
    }
}

public class LogModel
{
    public LogModel(string requestHost, string requestProtocol, string requestMethod, string requestPath, string requestPathAndQuery, int responseStatusCode)
    {
        Timestamp = DateTime.Now;
        RequestHost = requestHost;
        RequestProtocol = requestProtocol;
        RequestMethod = requestMethod;
        RequestPath = requestPath;
        RequestPathAndQuery = requestPathAndQuery;
        ResponseStatusCode = responseStatusCode;
    }

    private DateTime Timestamp { get; }
    public string MachineName => Environment.MachineName;
    public string Message { get; set; }
    public string RequestHost { get; set; }
    public string RequestProtocol { get; set; }
    public string RequestMethod { get; set; }
    public string RequestPath { get; set; }
    public string RequestPathAndQuery { get; set; }
    public int ResponseStatusCode { get; set; }
    public Dictionary<string, object> RequestHeaders { get; set; }
    public long? ElapsedMilliseconds { get; set; }
    public string RequestBody { get; set; }
    public Exception Exception { get; set; }
    public string Data { get; set; }
}
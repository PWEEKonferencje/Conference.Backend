using System.Diagnostics;
using System.Text.Json;
using Serilog;

namespace WebApi.Middlewares;

public class LoggingMiddleware(RequestDelegate next) : IMiddleware
{
	private record LogModel(string? Route, string? Query, int? StatusCode, long ResponseTime);

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		var stopwatch = Stopwatch.StartNew();
		context.Response.OnCompleted(() =>
		{
			var responseTime = stopwatch.ElapsedMilliseconds;
			stopwatch.Stop();
			var logModel = new LogModel(
				Route: context.Request.Path,
				Query: context.Request.QueryString.Value,
				StatusCode: context.Response.StatusCode,
				ResponseTime: responseTime
			);
			Log.Information(JsonSerializer.Serialize(logModel, new JsonSerializerOptions
			{
				WriteIndented = true
			}));
			return Task.CompletedTask;
		});
		await next.Invoke(context);
	}
}
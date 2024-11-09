using System.Diagnostics;
using System.Text.Json;
using Serilog;

namespace WebApi.Middlewares;

public class LoggingMiddleware(RequestDelegate next)
{
	private record LogModel(string? Route, string? Query, int? StatusCode, long ResponseTime);
	public async Task Invoke(HttpContext context)
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
		await next(context);
	}
}
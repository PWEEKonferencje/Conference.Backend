using System.Text;
using Serilog;

namespace WebApi.Middlewares;

public class LoggingMiddleware(RequestDelegate next)
{
	public async Task Invoke(HttpContext context)
	{
		var request = await FormatRequest(context.Request);
		Log.Information("HTTP Request: {Request}", request);
		
		var originalBodyStream = context.Response.Body;
		using var responseBody = new MemoryStream();
		context.Response.Body = responseBody;
		
		await next(context);
		
		var response = await FormatResponse(context.Response);
		Log.Information("HTTP Response: {Response}", response);

		await responseBody.CopyToAsync(originalBodyStream);
	}

	private static async Task<string> FormatRequest(HttpRequest request)
	{
		request.EnableBuffering();

		var body = request.Body;
		var buffer = new byte[Convert.ToInt32(request.ContentLength)];
		await request.Body.ReadAsync(buffer);
		var bodyAsText = Encoding.UTF8.GetString(buffer);

		request.Body.Position = 0;

		return $"{request.Method} {request.Path} {request.QueryString} {bodyAsText}";
	}

	private static async Task<string> FormatResponse(HttpResponse response)
	{
		response.Body.Seek(0, SeekOrigin.Begin);
		var text = await new StreamReader(response.Body).ReadToEndAsync();
		response.Body.Seek(0, SeekOrigin.Begin);

		return $"Status Code: {response.StatusCode}, Body: {text}";
	}
}
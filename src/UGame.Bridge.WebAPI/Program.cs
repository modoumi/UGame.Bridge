
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.IO.Compression;
using AiUo;

var builder = AspNetHost.CreateBuilder();

builder.AddAspNetEx();
//builder.Services.AddRequestDecompression();

var app = builder.Build();
app.UseAspNetEx();

app.UseAuthentication();
app.UseAuthorization();

// 使用自定义的请求解压缩中间件
app.UseRequestDecompression();

app.MapControllers();
app.Run();


public class RequestDecompressionMiddleware
{
    private readonly RequestDelegate _next;

    public RequestDecompressionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Content-Encoding", out var encoding))
        {
            if (encoding == "gzip")
            {
                context.Request.Body = new GZipStream(context.Request.Body, CompressionMode.Decompress);
            }
            else if (encoding == "br")
            {
                context.Request.Body = new BrotliStream(context.Request.Body, CompressionMode.Decompress);
            }
        }

        await _next(context);
    }
}

public static class RequestDecompressionMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestDecompression(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestDecompressionMiddleware>();
    }
}
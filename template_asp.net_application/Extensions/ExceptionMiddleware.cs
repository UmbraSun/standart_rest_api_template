using DTOs;
using Microsoft.IO;
using System.Net.Mime;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace template_asp.net_application.Extensions
{
    public sealed class ExceptionMiddleware
    {
        private static int BodyLengthLimitInBytes => 42000;
        private static string DefaultBody => "{\"Message\": \"Large content\",\"Length\": 0}";
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _memoryStreamManager;
        private static HashSet<string> _logMethods = new()
        {
            "POST",
            "PUT",
            "PATCH",
            "DELETE", 
            "GET", 
        };

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _memoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (_logMethods.Contains(context.Request.Method))
                {
                    var random = new Random();
                    var stringBuilder = new StringBuilder();
                    var maxVal = 5;
                    for (var i = 0; i < maxVal; i++)
                        stringBuilder.Append(random.Next(maxVal));
                    var id = stringBuilder.ToString();
                    await LogRequest(context, id);
                    await LogResponse(context, id);
                }
                else
                    await _next(context);
            }
            catch (Exception inner)
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(inner.Message);
                _logger.LogError(inner.Message);
            }
        }

        private async Task LogRequest(HttpContext context, string id)
        {
            var request = new LogRequestDto
            {
                Method = context.Request.Method,
                Path = context.Request.Path,
                Query = context.Request.QueryString.ToString(),
                UserAgent = context.Request.Headers["User-Agent"],
                Authorization = context.Request.Headers[nameof(LogRequestDto.Authorization)],
                Culture = context.Request.Headers[nameof(LogRequestDto.Culture)],
                BodyJson = await RequestBodyJson(context),
            };

            _logger.LogInformation($"_{nameof(request)}({id}): {JsonContent.Create(request)}");
        }

        private async Task<string> RequestBodyJson(HttpContext context)
        {
            context.Request.EnableBuffering();
            if (context.Request.ContentLength > BodyLengthLimitInBytes)
                return DefaultBody.Replace(0.ToString(), context.Request.ContentLength.ToString());

            await using var requestStream = _memoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            context.Request.Body.Position = 0;

            return ReadStreamInChunks(requestStream);
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }

        private async Task LogResponse(HttpContext context, string id)
        {
            var origin = context.Response.Body;
            await using var responseBody = _memoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                var response = new LogResponseDto
                {
                    StatusCode = context.Response.StatusCode,
                    UserName = context.User.Identity?.Name,
                    BodyJson = await ResponseBodyJson(context),
                };
                _logger.LogInformation($"_{nameof(response)}({id}): {JsonContent.Create(response)}");
                await responseBody.CopyToAsync(origin);
            }
            finally
            {
                context.Response.Body = origin;
            }
        }

        private async Task<string> ResponseBodyJson(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            if (context.Response.Body.Length > BodyLengthLimitInBytes)
                return DefaultBody.Replace(0.ToString(), context.Response.Body.Length.ToString());

            var bodyJson = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            return bodyJson;
        }
    }
}
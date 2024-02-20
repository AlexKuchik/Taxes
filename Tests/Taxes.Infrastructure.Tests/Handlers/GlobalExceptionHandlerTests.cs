using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Shouldly;
using Taxes.Infrastructure.Handlers;

namespace Taxes.Infrastructure.Tests.Handlers
{
    public class GlobalExceptionHandlerTests
    {
        [Test]
        public async Task TryHandleAsync_ShouldLogsErrorAndReturnsTrue()
        {
            var loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
            var httpContextMock = new DefaultHttpContext
            {
                Response =
                {
                    Body = new MemoryStream()
                }
            };
            var cancellationToken = CancellationToken.None;
            var exception = new Exception("Test exception");
            var exceptionHandler = new GlobalExceptionHandler(loggerMock.Object);

            var result = await exceptionHandler.TryHandleAsync(httpContextMock, exception, cancellationToken);

            result.ShouldBeTrue();
            httpContextMock.Response.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
            httpContextMock.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(httpContextMock.Response.Body);
            var streamText = await reader.ReadToEndAsync(cancellationToken);
            streamText.ShouldBeEquivalentTo("{\"title\":\"Internal server error\",\"status\":500}");
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!),
                Times.Once);
        }
    }
}
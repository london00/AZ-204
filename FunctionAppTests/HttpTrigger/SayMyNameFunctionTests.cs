using NUnit.Framework;
using FunctionApp.HttpTrigger;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using NSubstitute;

namespace FunctionApp.HttpTrigger.Tests
{
    [TestFixture()]
    public class SayMyNameFunctionTests
    {
        private DefaultHttpRequest request;
        private ILogger logger;

        [SetUp]
        public void SetUp()
        {
            this.request = new DefaultHttpRequest(new DefaultHttpContext());
            this.logger = NullLoggerFactory.Instance.CreateLogger("Dummy Logger");
        }

        [TestCase("abc")]
        public async Task RunTestAsync(string queryStringValue)
        {
            // Arrange
            this.request.Query = new QueryCollection(
                new Dictionary<string, StringValues>()
                {
                    { "name", queryStringValue }
                }
            );

            // Act
            var response = await SayMyNameFunction.Run(request, logger);

            // Assert
            response.Should().BeAssignableTo<OkObjectResult>();

            OkObjectResult okResponse = (OkObjectResult)response;

            okResponse.Value.ToString().Should().Be($"Hello, { queryStringValue }. This HTTP triggered function executed successfully. Counter: { SayMyNameFunction.counter }");
        }
    }
}
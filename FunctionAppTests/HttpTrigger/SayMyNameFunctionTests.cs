using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            this.logger = Substitute.For<ILogger>();
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
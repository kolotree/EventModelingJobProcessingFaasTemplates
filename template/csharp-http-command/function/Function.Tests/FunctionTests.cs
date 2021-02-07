using System;
using System.Threading.Tasks;
using FluentAssertions;
using JobProcessing.InMemoryStore;
using Xunit;

namespace Function.Tests
{
    public sealed class FunctionTests
    {
        private readonly FunctionHandler _functionHandler = new(new InMemoryStore());
        
        [Fact]
        public async Task Test1()
        {
            var (statusCode, responseString) = await  _functionHandler.Handle(
                new
                {
                    Id = Guid.NewGuid()
                }.ToHttpRequest());

            statusCode.Should().Be(200);
            responseString.Should().BeEmpty();
        }
    }
}
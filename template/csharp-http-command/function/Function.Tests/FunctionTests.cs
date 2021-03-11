using System.Threading.Tasks;
using FluentAssertions;
using Function.Domain;
using Grpc.Core;
using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.Serialization;
using JobProcessing.InMemoryStore;
using Xunit;

namespace Function.Tests
{
    public sealed class FunctionTests
    {
        private readonly InMemoryStore _store = new();
        private readonly FunctionHandler _functionHandler;

        public FunctionTests()
        {
            _functionHandler = new(_store);
        }
        
        [Fact]
        public async Task failure_returned_if_stream_with_same_id_exists()
        {
            _store.Given("ReplaceThisStream-Id1", new ReplaceThisEvent("Id1").ToEventEnvelopeUsing(CommandMetadata.GenerateNew()));
            
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    Metadata = CommandMetadata.GenerateNew(),
                    Id = "Id1"
                }.ToHttpRequest());

            functionResult.IsSuccess.Should().BeFalse();
            functionResult.Error.Should().Be("Item with the same ID already in store");
            _store.ProducedEventEnvelopes.Should().BeEmpty();
        }
        
        [Fact]
        public async Task success_returned_if_stream_with_same_id_doesnt_exist()
        {
            _store.Given("ReplaceThisStream-Id1", new ReplaceThisEvent("Id1").ToEventEnvelopeUsing(CommandMetadata.GenerateNew()));

            var commandMetadata = CommandMetadata.GenerateNew();
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    Metadata = commandMetadata,
                    Id = "Id2"
                }.ToHttpRequest());

            functionResult.IsSuccess.Should().BeTrue();
            _store.ProducedEventEnvelopes.Should().Contain(new ReplaceThisEvent("Id2").ToEventEnvelopeUsing(commandMetadata));
        }
    }
}
using EventFlow;
using EventFlow.Extensions;
using EventFlow.Queries;
using FluentAssertions;
using NUnit.Framework;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ColinCook.Documentation.GettingStarted
{
    public class ExampleTests
    {
        [Test]
        public async Task GettingStartedExample()
        {
            // We wire up EventFlow with all of our classes. Instead of adding events,
            // commands, etc. explicitly, we could have used the the simpler
            // AddDefaults(Assembly) instead.
            using (EventFlow.Configuration.IRootResolver resolver = EventFlowOptions.New
                .AddEvents(typeof(ExampleEvent))
                .AddCommands(typeof(ExampleCommand))
                .AddCommandHandlers(typeof(ExampleCommandHandler))
                .UseInMemoryReadStoreFor<ExampleReadModel>()
                .CreateResolver())
            {
                // Create a new identity for our aggregate root
                ExampleId exampleId = ExampleId.New;

                // Define some important value
                const int magicNumber = 42;

                // Resolve the command bus and use it to publish a command
                ICommandBus commandBus = resolver.Resolve<ICommandBus>();
                EventFlow.Aggregates.ExecutionResults.IExecutionResult executionResult = await commandBus.PublishAsync(
                        new ExampleCommand(exampleId, magicNumber),
                        CancellationToken.None)
                    .ConfigureAwait(false);

                // Verify that we didn't trigger our domain validation
                executionResult.IsSuccess.Should().BeTrue();

                // Resolve the query handler and use the built-in query for fetching
                // read models by identity to get our read model representing the
                // state of our aggregate root
                IQueryProcessor queryProcessor = resolver.Resolve<IQueryProcessor>();
                ExampleReadModel exampleReadModel = await queryProcessor.ProcessAsync(
                        new ReadModelByIdQuery<ExampleReadModel>(exampleId),
                        CancellationToken.None)
                    .ConfigureAwait(false);

                // Verify that the read model has the expected magic number
                exampleReadModel.MagicNumber.Should().Be(42);
            }
        }

        [Test]
        public async Task UsingAddDefaultsWorks()
        {
            // We wire up EventFlow with all of our classes. Instead of adding events,
            // commands, etc. explicitly, we could have used the the simpler
            // AddDefaults(Assembly) instead.
            using (EventFlow.Configuration.IRootResolver resolver = EventFlowOptions.New
                .AddDefaults(Assembly.GetExecutingAssembly())
                .UseInMemoryReadStoreFor<ExampleReadModel>()
                .CreateResolver())
            {
                // Create a new identity for our aggregate root
                ExampleId exampleId = ExampleId.New;

                // Define some important value
                const int magicNumber = 42;

                // Resolve the command bus and use it to publish a command
                ICommandBus commandBus = resolver.Resolve<ICommandBus>();
                EventFlow.Aggregates.ExecutionResults.IExecutionResult executionResult = await commandBus.PublishAsync(
                        new ExampleCommand(exampleId, magicNumber),
                        CancellationToken.None)
                    .ConfigureAwait(false);

                // Verify that we didn't trigger our domain validation
                executionResult.IsSuccess.Should().BeTrue();

                // Resolve the query handler and use the built-in query for fetching
                // read models by identity to get our read model representing the
                // state of our aggregate root
                IQueryProcessor queryProcessor = resolver.Resolve<IQueryProcessor>();
                ExampleReadModel exampleReadModel = await queryProcessor.ProcessAsync(
                        new ReadModelByIdQuery<ExampleReadModel>(exampleId),
                        CancellationToken.None)
                    .ConfigureAwait(false);

                // Verify that the read model has the expected magic number
                exampleReadModel.MagicNumber.Should().Be(42);
            }
        }

        [Test]
        public async Task SettingNumberTwiceCausesException()
        {
            // We wire up EventFlow with all of our classes. Instead of adding events,
            // commands, etc. explicitly, we could have used the the simpler
            // AddDefaults(Assembly) instead.
            using (EventFlow.Configuration.IRootResolver resolver = EventFlowOptions.New
                .AddDefaults(Assembly.GetExecutingAssembly())
                .UseInMemoryReadStoreFor<ExampleReadModel>()
                .CreateResolver())
            {
                // Create a new identity for our aggregate root
                ExampleId exampleId = ExampleId.New;

                // Define some important value
                const int magicNumber = 42;

                // Resolve the command bus and use it to publish a command
                ICommandBus commandBus = resolver.Resolve<ICommandBus>();
                EventFlow.Aggregates.ExecutionResults.IExecutionResult executionResult = await commandBus.PublishAsync(
                        new ExampleCommand(exampleId, magicNumber),
                        CancellationToken.None)
                    .ConfigureAwait(false);

                // Verify that we didn't trigger our domain validation
                executionResult.IsSuccess.Should().BeTrue();

                EventFlow.Aggregates.ExecutionResults.IExecutionResult executionResult2 = await commandBus.PublishAsync(
                        new ExampleCommand(exampleId, magicNumber),
                        CancellationToken.None)
                    .ConfigureAwait(false);

                // Verify that we didn't trigger our domain validation
                executionResult2.IsSuccess.Should().BeFalse();
            }
        }
    }
}
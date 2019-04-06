﻿using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Visits.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Visits.ReadModels;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Extensions;
using EventFlow.Queries;
using FluentAssertions;
using NUnit.Framework;

namespace ColinCook.VisitWorkflow.AggregateRoots.Visits
{
    public class VisitAggregateTests
    {
        [Test]
        public async Task SiteNotAdded_When_AddingTheSameSiteTwice()
        {
            using (var resolver = EventFlowOptions.New
                .AddDefaults(Assembly.GetExecutingAssembly())
                .CreateResolver())
            {
                // Create a new identity for our aggregate root
                var visitId = VisitId.New;

                // Define some important value
                var siteId = SiteId.New;

                // Resolve the command bus and use it to publish a command
                var commandBus = resolver.Resolve<ICommandBus>();
                await SuccessfullyPublishCommand(commandBus, new AddSiteCommand(visitId, siteId));
                await UnsuccessfullyPublishCommand(commandBus, new AddSiteCommand(visitId, siteId));
            }
        }

        [Test]
        public async Task EndToEnd()
        {
            using (var resolver = EventFlowOptions.New
                .AddDefaults(Assembly.GetExecutingAssembly())
                .UseInMemoryReadStoreFor<VisitReadModel>()
                .CreateResolver())
            {
                // Create a new identity for our aggregate root
                var visitId = VisitId.New;

                // Define some important value
                var siteId = SiteId.New;
                var operativeId = OperativeId.New;

                // Resolve the command bus and use it to publish a command
                var commandBus = resolver.Resolve<ICommandBus>();
                await SuccessfullyPublishCommand(commandBus, new AddSiteCommand(visitId, siteId));
                await SuccessfullyPublishCommand(commandBus, new AssignOperativeCommand(visitId, operativeId));
                await SuccessfullyPublishCommand(commandBus,
                    new DispatchOperativeCommand(visitId, operativeId, DateTime.Now.AddHours(1)));

                // Resolve the query handler and use the built-in query for fetching
                // read models by identity to get our read model representing the
                // state of our aggregate root
                var queryProcessor = resolver.Resolve<IQueryProcessor>();
                var visitReadModel = await queryProcessor.ProcessAsync(
                        new ReadModelByIdQuery<VisitReadModel>(visitId),
                        CancellationToken.None)
                    .ConfigureAwait(false);

                // Verify that the read model has the expected magic number
                visitReadModel.Sites.Should().Contain(siteId);
                visitReadModel.AssignedOperatives.Should().Contain(operativeId);
                visitReadModel.DispatchedOperatives.Should().Contain(operativeId);
            }
        }

        [Test]
        public async Task OperativeNotAdded_When_AddingTheSameOperativeTwice()
        {
            using (var resolver = EventFlowOptions.New
                .AddDefaults(Assembly.GetExecutingAssembly())
                .CreateResolver())
            {
                // Create a new identity for our aggregate root
                var visitId = VisitId.New;

                // Define some important value
                var operativeId = OperativeId.New;

                // Resolve the command bus and use it to publish a command
                var commandBus = resolver.Resolve<ICommandBus>();
                await SuccessfullyPublishCommand(commandBus, new AssignOperativeCommand(visitId, operativeId));
                await UnsuccessfullyPublishCommand(commandBus, new AssignOperativeCommand(visitId, operativeId));
            }
        }

        [Test]
        public async Task OperativeNotDispatched_When_EstimatedArrivalNotInFuture()
        {
            using (var resolver = EventFlowOptions.New
                .AddDefaults(Assembly.GetExecutingAssembly())
                .CreateResolver())
            {
                // Create a new identity for our aggregate root
                var visitId = VisitId.New;

                // Define some important value
                var siteId = SiteId.New;
                var operativeId = OperativeId.New;

                // Resolve the command bus and use it to publish a command
                var commandBus = resolver.Resolve<ICommandBus>();
                await SuccessfullyPublishCommand(commandBus, new AddSiteCommand(visitId, siteId));
                await SuccessfullyPublishCommand(commandBus, new AssignOperativeCommand(visitId, operativeId));
                await UnsuccessfullyPublishCommand(commandBus,
                    new DispatchOperativeCommand(visitId, operativeId, DateTime.Now));
            }
        }

        [Test]
        public async Task OperativeNotDispatched_When_NoSiteAdded()
        {
            using (var resolver = EventFlowOptions.New
                .AddDefaults(Assembly.GetExecutingAssembly())
                .CreateResolver())
            {
                // Create a new identity for our aggregate root
                var visitId = VisitId.New;

                // Define some important value
                var operativeId = OperativeId.New;

                // Resolve the command bus and use it to publish a command
                var commandBus = resolver.Resolve<ICommandBus>();
                await SuccessfullyPublishCommand(commandBus, new AssignOperativeCommand(visitId, operativeId));
                await UnsuccessfullyPublishCommand(commandBus,
                    new DispatchOperativeCommand(visitId, operativeId, DateTime.Now.AddHours(1)));
            }
        }

        private static async Task SuccessfullyPublishCommand(ICommandBus commandBus,
            Command<VisitAggregate, VisitId, IExecutionResult> command)
        {
            var executionResult = await commandBus.PublishAsync(
                    command,
                    CancellationToken.None)
                .ConfigureAwait(false);

            executionResult.IsSuccess.Should().BeTrue();
        }

        private static async Task UnsuccessfullyPublishCommand(ICommandBus commandBus,
            Command<VisitAggregate, VisitId, IExecutionResult> command)
        {
            var executionResult = await commandBus.PublishAsync(
                    command,
                    CancellationToken.None)
                .ConfigureAwait(false);

            executionResult.IsSuccess.Should().BeFalse();
        }
    }
}
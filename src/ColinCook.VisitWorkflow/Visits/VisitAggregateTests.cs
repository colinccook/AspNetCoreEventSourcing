// The MIT License (MIT)
// 
// Copyright (c) 2015-2017 Rasmus Mikkelsen
// Copyright (c) 2015-2017 eBay Software Foundation
// https://github.com/eventflow/EventFlow
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.Visits.Aggregates;
using ColinCook.VisitWorkflow.Visits.Commands;
using ColinCook.VisitWorkflow.Visits.Identities;
using ColinCook.VisitWorkflow.Visits.ReadModels;
using EventFlow;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Extensions;
using EventFlow.Queries;
using FluentAssertions;
using NUnit.Framework;

namespace ColinCook.VisitWorkflow.Visits
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
                await SuccessfullyPublishCommand(commandBus, new DispatchOperativeCommand(visitId, operativeId, DateTime.Now.AddHours(1)));

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
                await UnsuccessfullyPublishCommand(commandBus, new DispatchOperativeCommand(visitId, operativeId, DateTime.Now));
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
                await UnsuccessfullyPublishCommand(commandBus, new DispatchOperativeCommand(visitId, operativeId, DateTime.Now.AddHours(1)));
            }
        }

        private static async Task SuccessfullyPublishCommand(ICommandBus commandBus, Command<VisitAggregate, VisitId, IExecutionResult> command)
        {
            var executionResult = await commandBus.PublishAsync(
                    command,
                    CancellationToken.None)
                .ConfigureAwait(false);

            executionResult.IsSuccess.Should().BeTrue();
        }

        private static async Task UnsuccessfullyPublishCommand(ICommandBus commandBus, Command<VisitAggregate, VisitId, IExecutionResult> command)
        {
            var executionResult = await commandBus.PublishAsync(
                    command,
                    CancellationToken.None)
                .ConfigureAwait(false);

            executionResult.IsSuccess.Should().BeFalse();
        }
    }
}
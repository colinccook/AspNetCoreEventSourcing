using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Works.Commands
{
    public class WorkRaisedCommand :
        Command<WorkAggregate, WorkId, IExecutionResult>
    {
        public WorkRaisedCommand(WorkId workId, IEnumerable<SiteId> sites, string title, string description ) : base(workId)
        {
            WorkId = workId;
            Sites = sites;
            Title = title;
            Description = description;
        }

        public WorkId WorkId { get; }
        public IEnumerable<SiteId> Sites { get; }
        public string Title { get; }
        public string Description { get; }
    }

    public class WorkRaisedCommandHandler :
        CommandHandler<WorkAggregate, WorkId, IExecutionResult, WorkRaisedCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(WorkAggregate aggregate, WorkRaisedCommand command, CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Raise(command.Title, command.Description, command.Sites);
            return Task.FromResult(executionResult);
        }
    }
}

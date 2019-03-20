using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ColinCook.VisitWorkflow.AggregateRoots.Works.Commands
{
    public class WorkRaisedCommand :
        Command<WorkAggregate, WorkId, IExecutionResult>
    {
        public WorkRaisedCommand(WorkId workId, IEnumerable<SiteId> sites, string title, string description) :
            base(workId)
        {
            Sites = sites;
            Title = title;
            Description = description;
        }

        public IEnumerable<SiteId> Sites { get; }
        public string Title { get; }
        public string Description { get; }
    }

    public class WorkRaisedCommandHandler :
        CommandHandler<WorkAggregate, WorkId, IExecutionResult, WorkRaisedCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(WorkAggregate aggregate, WorkRaisedCommand command,
            CancellationToken cancellationToken)
        {
            IExecutionResult executionResult = aggregate.Raise(command.Title, command.Description, command.Sites);
            return Task.FromResult(executionResult);
        }
    }
}
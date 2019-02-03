using System;
using System.Collections.Generic;
using System.Text;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.Queries;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries
{
    public class AllOperativesQuery : IQuery<IReadOnlyCollection<string>>
    {
    }
}

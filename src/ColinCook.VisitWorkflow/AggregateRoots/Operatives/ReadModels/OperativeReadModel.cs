using System;
using System.Collections.Generic;
using System.Text;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Events;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Models;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels
{
    public class OperativeReadModel : IReadModel,
        IAmReadModelFor<OperativeAggregate, OperativeId, OperativeHiredEvent>
    {
        public OperativeModel OperativeModel { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<OperativeAggregate, OperativeId, OperativeHiredEvent> domainEvent)
        {
            OperativeModel = new OperativeModel
            {
                OperativeId = domainEvent.AggregateIdentity,
                Forename = domainEvent.AggregateEvent.Forename,
                Surname = domainEvent.AggregateEvent.Surname
            };
        }
    }
}

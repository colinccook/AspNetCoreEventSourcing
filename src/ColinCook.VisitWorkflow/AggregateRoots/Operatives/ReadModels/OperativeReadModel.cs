﻿using System;
using System.Collections.Generic;
using System.Text;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Events;
using ColinCook.VisitWorkflow.AggregateRoots.Works;
using ColinCook.VisitWorkflow.AggregateRoots.Works.Events;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels
{
    public class OperativeReadModel : IReadModel,
        IAmReadModelFor<OperativeAggregate, OperativeId, OperativeHiredEvent>
    {
        public OperativeId OperativeId { get; private set; }
        public string Forename { get; private set; }
        public string Surname { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<OperativeAggregate, OperativeId, OperativeHiredEvent> domainEvent)
        {
            OperativeId = domainEvent.AggregateIdentity;
            Forename = domainEvent.AggregateEvent.Forename;
            Surname = domainEvent.AggregateEvent.Surname;
        }
    }
}

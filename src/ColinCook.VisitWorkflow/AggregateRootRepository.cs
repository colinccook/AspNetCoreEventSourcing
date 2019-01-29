using EventFlow.Aggregates;
using EventFlow.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColinCook.VisitWorkflow
{
    public class AggregateRootRepository<TAggregate, TIdentity>
        where TAggregate : AggregateRoot<TAggregate, TIdentity>
        where TIdentity : IIdentity
    {
       List<AggregateRoot<TAggregate, TIdentity>> _aggregateRoots;

        public AggregateRootRepository()
        {
            _aggregateRoots = new List<AggregateRoot<TAggregate, TIdentity>>();
        }

        public void Insert(AggregateRoot<TAggregate, TIdentity> aggregateRoot)
        {
            if (_aggregateRoots.Any(ar => ar.Id.Value == aggregateRoot.Id.Value))
                throw new Exception("Already inserted");

            _aggregateRoots.Add(aggregateRoot);
        }

        public IEnumerable<AggregateRoot<TAggregate, TIdentity>> GetAll()
        {
            return _aggregateRoots;
        }
    }
}

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
using System.Collections.Generic;
using System.Linq;
using ColinCook.VisitWorkflow.AggregateRoots.Visits.Events;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;

namespace ColinCook.VisitWorkflow.AggregateRoots.Visits
{
    class OperativeWorkflow
    {
        internal object EstimatedArrival;

        public OperativeId Operative { get; set; }
        public DateTime Dispatched { get; set; }
    }

    /// The aggregate root
    public class VisitAggregate :
        AggregateRoot<VisitAggregate, VisitId>,
        IEmit<AddSiteEvent>,
        IEmit<AssignOperativeEvent>,
        IEmit<DispatchOperativeEvent>
    {
        private readonly List<SiteId> _sites = new List<SiteId>();
        private readonly List<OperativeWorkflow> _operatives = new List<OperativeWorkflow>();

        public VisitAggregate(VisitId id) : base(id) { }

        // Method invoked by commands
        public IExecutionResult AddSite(SiteId siteId)
        {
            if (_sites.Contains(siteId))
                return ExecutionResult.Failed("Site already added to this visit");

            Emit(new AddSiteEvent(siteId));

            return ExecutionResult.Success();
        }

        public IExecutionResult AssignOperative(OperativeId operativeId)
        {
            if (_operatives.Any(o => o.Operative == operativeId))
                return ExecutionResult.Failed("Operative already assigned to this visit");

            Emit(new AssignOperativeEvent(operativeId));

            return ExecutionResult.Success();
        }

        public IExecutionResult DispatchOperative(OperativeId operativeId, DateTime estimatedArrival)
        {
            if (_operatives.All(o => o.Operative != operativeId))
                return ExecutionResult.Failed("Operative has not been assigned to the visit to dispatch");

            if (!_sites.Any())
                return ExecutionResult.Failed("At least one site must be added to dispatch an operative");

            if (estimatedArrival <= DateTime.Now)
                return ExecutionResult.Failed("Estimated arrival date must be in the future");

            Emit(new DispatchOperativeEvent(operativeId, estimatedArrival));

            return ExecutionResult.Success();
        }

        // We apply the event as part of the event sourcing system. EventFlow
        // provides several different methods for doing this, e.g. state objects,
        // the Apply method is merely the simplest

        public void Apply(AddSiteEvent aggregateEvent)
        {
            _sites.Add(aggregateEvent.SiteId);
        }

        public void Apply(AssignOperativeEvent aggregateEvent)
        {
            _operatives.Add(new OperativeWorkflow { Operative = aggregateEvent.OperativeId});
        }

        public void Apply(DispatchOperativeEvent aggregateEvent)
        {
            var operativeWorkflow = _operatives.Single(o => o.Operative == aggregateEvent.OperativeId);
            operativeWorkflow.Dispatched = DateTime.Now;
            operativeWorkflow.EstimatedArrival = aggregateEvent.EstimatedArrival;
        }
    }
}
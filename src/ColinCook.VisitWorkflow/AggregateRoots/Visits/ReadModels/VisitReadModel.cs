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
// 

using System.Collections.Generic;
using ColinCook.VisitWorkflow.AggregateRoots.Visits.Events;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace ColinCook.VisitWorkflow.AggregateRoots.Visits.ReadModels
{
    /// Read model for our aggregate
    public class VisitReadModel :
        IReadModel,
        IAmReadModelFor<VisitAggregate, VisitId, AddSiteEvent>,
        IAmReadModelFor<VisitAggregate, VisitId, AssignOperativeEvent>,
        IAmReadModelFor<VisitAggregate, VisitId, DispatchOperativeEvent>
    {
        public List<SiteId> Sites { get; }
        public List<OperativeId> AssignedOperatives { get; }
        public List<OperativeId> DispatchedOperatives { get; }

        public VisitReadModel()
        {
            Sites = new List<SiteId>();
            AssignedOperatives = new List<OperativeId>();
            DispatchedOperatives = new List<OperativeId>();
        }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<VisitAggregate, VisitId, AddSiteEvent> domainEvent)
        {
            Sites.Add(domainEvent.AggregateEvent.SiteId);
        }

        public void Apply(
            IReadModelContext context, 
            IDomainEvent<VisitAggregate, VisitId, AssignOperativeEvent> domainEvent)
        {
            AssignedOperatives.Add(domainEvent.AggregateEvent.OperativeId);
        }

        public void Apply(IReadModelContext context, IDomainEvent<VisitAggregate, VisitId, DispatchOperativeEvent> domainEvent)
        {
            DispatchedOperatives.Add(domainEvent.AggregateEvent.OperativeId);
        }
    }
}
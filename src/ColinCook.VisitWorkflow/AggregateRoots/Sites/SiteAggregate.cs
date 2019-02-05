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
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Events;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Events;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;

namespace ColinCook.VisitWorkflow.AggregateRoots.Sites
{
    /// The aggregate root
    public class SiteAggregate :
        AggregateRoot<SiteAggregate, SiteId>,
        IEmit<SiteAcquiredEvent>
    {
        private string _addressLine1;
        private string _town;
        private string _postCode;
        private string _telephoneNumber;

        public SiteAggregate(SiteId id) : base(id)
        {
        }

        internal IExecutionResult Acquire(string addressLine1, string town, string postCode, string telephoneNumber)
        {
            if (string.IsNullOrWhiteSpace(addressLine1))
                return ExecutionResult.Failed("Site must have an address line 1");

            if (string.IsNullOrWhiteSpace(town))
                return ExecutionResult.Failed("Site must have a town");

            if (string.IsNullOrWhiteSpace(postCode))
                return ExecutionResult.Failed("Site must have a post code");

            if (string.IsNullOrWhiteSpace(telephoneNumber))
                return ExecutionResult.Failed("Site must have a telephone number");

            Emit(new SiteAcquiredEvent(addressLine1, town, postCode, telephoneNumber));

            return ExecutionResult.Success();
        }

        public void Apply(SiteAcquiredEvent aggregateEvent)
        {
            _addressLine1 = aggregateEvent.AddressLine1;
            _town = aggregateEvent.Town;
            _postCode = aggregateEvent.PostCode;
            _telephoneNumber = aggregateEvent.TelephoneNumber;
        }
    }
}
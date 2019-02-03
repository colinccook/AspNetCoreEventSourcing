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
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives
{
    /// The aggregate root
    public class OperativeAggregate :
        AggregateRoot<OperativeAggregate, OperativeId>,
        IEmit<OperativeHiredEvent>
    {
        private string _forename;
        private string _surname;

        public OperativeAggregate(OperativeId id) : base(id)
        {
        }

        public void Apply(OperativeHiredEvent aggregateEvent)
        {
            _forename = aggregateEvent.Forename;
            _surname = aggregateEvent.Surname;
        }

        public IExecutionResult Hire(string forename, string surname)
        {
            if (string.IsNullOrWhiteSpace(forename))
                return ExecutionResult.Failed("Operative must have a forename");

            if (string.IsNullOrWhiteSpace(surname))
                return ExecutionResult.Failed("Operative must have a surname");

            Emit(new OperativeHiredEvent(forename, surname));

            return ExecutionResult.Success();
        }
    }
}
using System;
using ColinCook.VisitWorkflow.Operatives.Identities;
using EventFlow.Aggregates;
using EventFlow.Entities;

namespace ColinCook.VisitWorkflow.Operatives.Entities
{
    public class OperativeEntity
    {
        public int Id { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
    }
}

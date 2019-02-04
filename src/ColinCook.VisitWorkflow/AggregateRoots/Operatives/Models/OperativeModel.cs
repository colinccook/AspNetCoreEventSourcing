using System;
using System.Collections.Generic;
using System.Text;
using ColinCook.VisitWorkflow.Identities;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives.Models
{
    public class OperativeModel
    {
        public OperativeId OperativeId { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
    }
}

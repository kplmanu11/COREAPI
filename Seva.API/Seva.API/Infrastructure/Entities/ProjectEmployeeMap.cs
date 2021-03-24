using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seva.API.Infrastructure.Entities
{
    public class ProjectEmployeeMap: EntityBase
    {
        public Employee Employee { get; set; }

        public Project Project { get; set; }

        public bool IsManager { get; set; }

        public bool IsTeamLead { get; set; }


    }
}

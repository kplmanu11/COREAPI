using Seva.API.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seva.API.Infrastructure
{
    public class Employee : EntityBase
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OfficialMobile { get; set; }

        public string PersonalMobile { get; set; }

        public string LandLine { get; set; }

        public string Address { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

    }
}

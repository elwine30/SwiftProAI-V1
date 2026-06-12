using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupRoadCondition
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupRoadCondition(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "RC001", Description = "Well Maintained", Active = true, Sequence = 1, Group = "RoadCondition" },
                new Lookup { Code = "RC002", Description = "Fair", Active = true, Sequence = 2, Group = "RoadCondition" },
                new Lookup { Code = "RC003", Description = "Poor Condition", Active = true, Sequence = 3, Group = "RoadCondition" },
            };

            foreach (var lookup in lookupValues)
            {
                if (!_context.Lookups.Any(l => l.Code == lookup.Code))
                {
                    _context.Lookups.Add(lookup);
                }
            }


        }

    }
}

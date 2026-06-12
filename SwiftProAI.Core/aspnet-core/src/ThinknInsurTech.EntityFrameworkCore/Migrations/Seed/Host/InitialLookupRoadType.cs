using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupRoadType
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupRoadType(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "RT001", Description = "Macadamized", Active = true, Sequence = 1, Group = "RoadType" },
                new Lookup { Code = "RT002", Description = "Gravel Road", Active = true, Sequence = 2, Group = "RoadType" },
                new Lookup { Code = "RT003", Description = "Right Curve", Active = true, Sequence = 3, Group = "RoadType" },
                new Lookup { Code = "RT004", Description = "Left Curve", Active = true, Sequence = 4, Group = "RoadType" },
                new Lookup { Code = "RT005", Description = "Single Carriageway", Active = true, Sequence = 5, Group = "RoadType" },
                new Lookup { Code = "RT006", Description = "Dual Carriageway", Active = true, Sequence = 6, Group = "RoadType" },
                new Lookup { Code = "RT007", Description = "Others", Active = true, Sequence = 7, Group = "RoadType" },

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

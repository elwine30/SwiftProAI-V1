using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupCenterDemarcation
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupCenterDemarcation(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "CD001", Description = "White Intermittent Single/Double line", Active = true, Sequence = 1, Group = "CenterDemarcation" },
                new Lookup { Code = "CD002", Description = "Raised Concrete Divider", Active = true, Sequence = 2, Group = "CenterDemarcation" },
                new Lookup { Code = "CD003", Description = "Galvanized Metal Railing", Active = true, Sequence = 3, Group = "CenterDemarcation" },

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

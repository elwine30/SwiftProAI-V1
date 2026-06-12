using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupSurroundingArea
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupSurroundingArea(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "SA001", Description = "Commercial", Active = true, Sequence = 1, Group = "SurroundingArea" },
                new Lookup { Code = "SA002", Description = "Residential", Active = true, Sequence = 2, Group = "SurroundingArea" },
                new Lookup { Code = "SA003", Description = "Industrial Area", Active = true, Sequence = 3, Group = "SurroundingArea" },
                new Lookup { Code = "SA004", Description = "School Area", Active = true, Sequence = 4, Group = "SurroundingArea" },
                new Lookup { Code = "SA005", Description = "Plantation", Active = true, Sequence = 5, Group = "SurroundingArea" },
                new Lookup { Code = "SA006", Description = "Kampong", Active = true, Sequence = 6, Group = "SurroundingArea" },
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

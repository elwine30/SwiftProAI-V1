using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialLookupIDType
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupIDType(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "IDTYPE001", Description = "IC", Active = true, Sequence = 1, Group = "IDType" },
                new Lookup { Code = "IDTYPE002", Description = "Passport", Active = true, Sequence = 2, Group = "IDType" },
                new Lookup { Code = "IDTYPE003", Description = "Company ID", Active = true, Sequence = 3, Group = "IDType" },
                new Lookup { Code = "IDTYPE004", Description = "Armed Force ID", Active = true, Sequence = 4, Group = "IDType" },
                new Lookup { Code = "IDTYPE005", Description = "Old IC", Active = true, Sequence = 5, Group = "IDType" },
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
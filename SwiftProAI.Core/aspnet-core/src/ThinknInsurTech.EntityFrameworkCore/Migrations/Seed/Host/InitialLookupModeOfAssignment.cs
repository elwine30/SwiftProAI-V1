using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupModeOfAssignment
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupModeOfAssignment(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "MA001", Description = "Merimen", Active = true, Sequence = 1, Group = "ModeOfAssignment" },
                new Lookup { Code = "MA002", Description = "Email", Active = true, Sequence = 2, Group = "ModeOfAssignment" },
                new Lookup { Code = "MA003", Description = "Fax", Active = true, Sequence = 3, Group = "ModeOfAssignment" },
                new Lookup { Code = "MA004", Description = "By Hand", Active = true, Sequence = 4, Group = "ModeOfAssignment" },
                new Lookup { Code = "MA005", Description = "Portal", Active = true, Sequence = 5, Group = "ModeOfAssignment" },
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

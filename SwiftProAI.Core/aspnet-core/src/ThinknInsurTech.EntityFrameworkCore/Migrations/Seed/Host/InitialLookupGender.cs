using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialLookupGender
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupGender(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "GEN001", Description = "Male", Active = true, Sequence = 1, Group = "Gender" },
                new Lookup { Code = "GEN002", Description = "Female", Active = true, Sequence = 2, Group = "Gender" },
                new Lookup { Code = "GEN003", Description = "Others", Active = true, Sequence = 3, Group = "Gender" },
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
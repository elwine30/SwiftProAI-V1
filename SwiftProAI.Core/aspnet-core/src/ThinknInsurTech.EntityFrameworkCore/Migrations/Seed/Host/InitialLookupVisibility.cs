using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupVisibility
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupVisibility(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "V001", Description = "Unhampered", Active = true, Sequence = 1, Group = "Visibility" },
                new Lookup { Code = "V002", Description = "Hampered", Active = true, Sequence = 2, Group = "Visibility" },
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

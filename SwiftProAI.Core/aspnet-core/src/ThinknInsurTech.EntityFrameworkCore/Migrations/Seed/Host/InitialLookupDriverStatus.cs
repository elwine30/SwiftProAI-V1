using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupDriverStatus
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupDriverStatus(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "DS001", Description = "Alone", Active = true, Sequence = 1, Group = "DriverStatus" },  
                new Lookup { Code = "DS002", Description = "Accompanied", Active = true, Sequence = 2, Group = "DriverStatus" },  
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

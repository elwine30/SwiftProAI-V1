using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupVisbilityReasons
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupVisbilityReasons(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "VR001", Description = "Not Applicable (Unhampered)", Active = true, Sequence = 1, Group = "VisibilityReason" },
                new Lookup { Code = "VR002", Description = "Rain", Active = true, Sequence = 2, Group = "VisibilityReason" },
                new Lookup { Code = "VR003", Description = "Hazy/Fog", Active = true, Sequence = 3, Group = "VisibilityReason" },
                new Lookup { Code = "VR004", Description = "No Street Lights", Active = true, Sequence = 4, Group = "VisibilityReason" },
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupPoliceReportType
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupPoliceReportType(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "PR001", Description = "Claimant", Active = true, Sequence = 1, Group = "PoliceReportType" },
                new Lookup { Code = "PR002", Description = "ThirdParty", Active = true, Sequence = 2, Group = "PoliceReportType" },
                new Lookup { Code = "PR003", Description = "Police", Active = true, Sequence = 3, Group = "PoliceReportType" },
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

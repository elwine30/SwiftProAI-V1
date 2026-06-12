using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupViewThirdPartyRequestStatus
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupViewThirdPartyRequestStatus(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookups = new List<Lookup>
            {
                new Lookup { Code = "VTPRS001", Description = "Pending Approval", Active = true, Sequence = 1, Group = "RequestStatus" },
                new Lookup { Code = "VTPRS002", Description = "Approved", Active = true, Sequence = 2, Group = "RequestStatus" },
                new Lookup { Code = "VTPRS003", Description = "Cancelled", Active = true, Sequence = 3, Group = "RequestStatus" },
            };

            foreach (var lookup in lookups)
            {
                if (!_context.Lookups.Any(l => l.Code == lookup.Code))
                {
                    _context.Lookups.Add(lookup);
                }
            }

        }
    }
}

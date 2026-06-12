using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialExpensesClaimsApproval
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialExpensesClaimsApproval(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "ExClaimAppr001", Description = "Expenses", Active = true, Sequence = 1, Group = "ExpClaimApproval" },
                new Lookup { Code = "ExClaimAppr002", Description = "Claims", Active = true, Sequence = 2, Group = "ExpClaimApproval" }
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

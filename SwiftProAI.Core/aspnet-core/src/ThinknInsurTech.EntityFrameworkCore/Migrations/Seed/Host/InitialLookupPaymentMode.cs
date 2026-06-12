using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialLookupPaymentMode
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupPaymentMode(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "PAYMODE001", Description = "Cash", Active = true, Sequence = 1, Group = "PaymentMode" },
                new Lookup { Code = "PAYMODE002", Description = "Online", Active = true, Sequence = 2, Group = "PaymentMode" },
                new Lookup { Code = "PAYMODE003", Description = "Check", Active = true, Sequence = 3, Group = "PaymentMode" }
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

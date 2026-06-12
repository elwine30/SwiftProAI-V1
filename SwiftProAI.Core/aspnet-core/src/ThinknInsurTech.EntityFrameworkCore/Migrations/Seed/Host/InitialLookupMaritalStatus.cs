using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialLookupMaritalStatus
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupMaritalStatus(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "MST001", Description = "Single", Active = true, Sequence = 1, Group = "MaritalStatus" },
                new Lookup { Code = "MST002", Description = "Married", Active = true, Sequence = 2, Group = "MaritalStatus" },
                new Lookup { Code = "MST003", Description = "Divorced", Active = true, Sequence = 3, Group = "MaritalStatus" },
                new Lookup { Code = "MST004", Description = "Widowed", Active = true, Sequence = 4, Group = "MaritalStatus" },
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
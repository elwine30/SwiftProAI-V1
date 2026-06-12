using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class UpdateLookupExpensesStatus
    {
        private readonly ThinknInsurTechDbContext _context;

        public UpdateLookupExpensesStatus(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Update()
        {
            // disable options 
            var codes = new List<string> { "EXP004", "EXP006" };
            var lookups = _context.Lookups
                .Where(l => codes.Contains(l.Code))
                .ToList();

            foreach (var lookup in lookups)
            {
                lookup.Sequence = 0;
                lookup.Active = false;
            }

        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupExpensesStatus
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupExpensesStatus(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "EXP001", Description = "Pending For Approval", Active = true, Sequence = 1, Group = "ExpensesStatus" },
                new Lookup { Code = "EXP002", Description = "Pending For Payment", Active = true, Sequence = 2, Group = "ExpensesStatus" },
                new Lookup { Code = "EXP003", Description = "Payment Done", Active = true, Sequence = 3, Group = "ExpensesStatus" },
                new Lookup { Code = "EXP004", Description = "Submitted", Active = true, Sequence = 4, Group = "ExpensesStatus" },
                new Lookup { Code = "EXP005", Description = "Rejected", Active = true, Sequence = 5, Group = "ExpensesStatus" },
                new Lookup { Code = "EXP006", Description = "Submit Without Payment", Active = true, Sequence = 6, Group = "ExpensesStatus" },
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialLookupRelationship
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupRelationship(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup> {
                new Lookup { Code = "RLTN001", Description = "Self", Active = true, Sequence = 1, Group = "Relationship" },
                new Lookup { Code = "RLTN002", Description = "Father", Active = true, Sequence = 2, Group = "Relationship" },
                new Lookup { Code = "RLTN003", Description = "Mother", Active = true, Sequence = 3, Group = "Relationship" },
                new Lookup { Code = "RLTN004", Description = "Husband", Active = true, Sequence = 4, Group = "Relationship" },
                new Lookup { Code = "RLTN005", Description = "Wife", Active = true, Sequence = 5, Group = "Relationship" },
                new Lookup { Code = "RLTN006", Description = "Son", Active = true, Sequence = 6, Group = "Relationship" },
                new Lookup { Code = "RLTN007", Description = "Daughter", Active = true, Sequence = 7, Group = "Relationship" },
                new Lookup { Code = "RLTN008", Description = "Friend", Active = true, Sequence = 8, Group = "Relationship" },
                new Lookup { Code = "RLTN009", Description = "Family", Active = true, Sequence = 9, Group = "Relationship" },
                new Lookup { Code = "RLTN010", Description = "Insured Pillion", Active = true, Sequence = 10, Group = "Relationship" },
                new Lookup { Code = "RLTN011", Description = "Insured Passenger", Active = true, Sequence = 11, Group = "Relationship" },
                new Lookup { Code = "RLTN012", Description = "Employee", Active = true, Sequence = 12, Group = "Relationship" },
                new Lookup { Code = "RLTN013", Description = "None", Active = true, Sequence = 13, Group = "Relationship" },
                new Lookup { Code = "RLTN014", Description = "Others", Active = true, Sequence = 14, Group = "Relationship" },
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
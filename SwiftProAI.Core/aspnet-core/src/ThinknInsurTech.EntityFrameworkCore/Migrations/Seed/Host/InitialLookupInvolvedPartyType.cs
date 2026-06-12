using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupInvolvedPartyType
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupInvolvedPartyType(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "IPTYPE001", Description = "Insured", Active = true, Sequence = 1, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE002", Description = "Insured Driver", Active = true, Sequence = 2, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE003", Description = "Insured Passenger", Active = true, Sequence = 3, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE004", Description = "Insured FIR", Active = true, Sequence = 4, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE005", Description = "Insured Rider", Active = true, Sequence = 5, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE006", Description = "Insured Pillion", Active = true, Sequence = 6, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE007", Description = "Third Party", Active = true, Sequence = 7, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE008", Description = "Third Party Driver", Active = true, Sequence = 8, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE009", Description = "Third Party Passenger", Active = true, Sequence = 9, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE010", Description = "Third Party FIR", Active = true, Sequence = 10, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE011", Description = "Third Party Rider", Active = true, Sequence = 11, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE012", Description = "Third Party Pillion", Active = true, Sequence = 12, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE013", Description = "Pedestrian", Active = true, Sequence = 13, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE014", Description = "Cyclist", Active = true, Sequence = 14, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE015", Description = "Cyclist FIR", Active = true, Sequence = 15, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE016", Description = "Child", Active = true, Sequence = 16, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE017", Description = "Child FIR", Active = true, Sequence = 17, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE018", Description = "Employer FIR", Active = true, Sequence = 18, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE019", Description = "Highway Authorities FIR", Active = true, Sequence = 19, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE020", Description = "Police FIR", Active = true, Sequence = 20, Group = "InvolvedPartyType" },
                new Lookup { Code = "IPTYPE021", Description = "Witness", Active = true, Sequence = 21, Group = "InvolvedPartyType" },
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

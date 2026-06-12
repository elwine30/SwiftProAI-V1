using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupViewOfRoad
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupViewOfRoad(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "RV001", Description = "Unobstructed", Active = true, Sequence = 1, Group = "RoadView" },   
                new Lookup { Code = "RV002", Description = "Obstructed", Active = true, Sequence = 2, Group = "RoadView" },   
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

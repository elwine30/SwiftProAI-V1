using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ThinknInsurTech.Branches;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialLocationCreator
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLocationCreator(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var my = _context.Locations.FirstOrDefault(p => p.Name == "MALAYSIA");
            if (my == null)
            {
                _context.Locations.Add(
                    new Location
                    {
                        ParentLocationId = 0,
                        Name = "MALAYSIA",
                        ShortName = "MY"
                    });
            }

            var states = new List<Location>
            {
                new Location { Name = "KUALA LUMPUR", ShortName = "KL" },
                new Location { Name = "PENANG", ShortName = "PG" },
                new Location { Name = "IPOH", ShortName = "IP" },
                new Location { Name = "SELANGOR", ShortName = "SEL" },
                new Location { Name = "JOHOR", ShortName = "JOH" },
                new Location { Name = "NEGERI SEMBILAN", ShortName = "N9" },
                new Location { Name = "KELANTAN", ShortName = "KLT" },
                new Location { Name = "SABAH", ShortName = "SAB" },
                new Location { Name = "SARAWAK", ShortName = "SWK" },
                new Location { Name = "KEDAH", ShortName = "KED" },
                new Location { Name = "TERENGGANU", ShortName = "TRG" },
                new Location { Name = "PAHANG", ShortName = "PAH" },
                new Location { Name = "MELAKA", ShortName = "MEL" },
                new Location { Name = "PERLIS", ShortName = "PLS" },
                new Location { Name = "LABUAN", ShortName = "LBN" },
                new Location { Name = "PUTRAJAYA", ShortName = "PJY" }
            };

            var newLocations = states
                .Where(state => !_context.Locations.Any(x => x.Name == state.Name))
                .Select(state => new Location
                {
                    Name = state.Name,
                    ShortName = state.ShortName,
                    ParentLocationId = 1
                })
                .ToList();

            if (newLocations.Any())
            {
                _context.Locations.AddRange(newLocations); // batch insert
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class InitialLookupWeatherCondition
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialLookupWeatherCondition(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var lookupValues = new List<Lookup>
            {
                new Lookup { Code = "WC001", Description = "Fine", Active = true, Sequence = 1, Group = "WeatherCondition" },
            new Lookup { Code = "WC002", Description = "Drizzling", Active = true, Sequence = 2, Group = "WeatherCondition" },
            new Lookup { Code = "WC003", Description = "Raining", Active = true, Sequence = 3, Group = "WeatherCondition" },
            new Lookup { Code = "WC004", Description = "Heavy Rain", Active = true, Sequence = 4, Group = "WeatherCondition" },
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

using System.Collections.Generic;
using System.Linq;
using ThinknInsurTech.Branches;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialBranchCreator
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialBranchCreator(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var states = new List<Branch>
            {
                new Branch { Name = "KUALA LUMPUR", ShortName = "KL" },
                new Branch { Name = "PENANG", ShortName = "PG" },
                new Branch { Name = "IPOH", ShortName = "IP" },
                new Branch { Name = "SELANGOR", ShortName = "SEL" },
                new Branch { Name = "JOHOR", ShortName = "JOH" },
                new Branch { Name = "NEGERI SEMBILAN", ShortName = "N9" },
                new Branch { Name = "KELANTAN", ShortName = "KLT" },
                new Branch { Name = "SABAH", ShortName = "SAB" },
                new Branch { Name = "SARAWAK", ShortName = "SWK" },
                new Branch { Name = "KEDAH", ShortName = "KED" },
                new Branch { Name = "TERENGGANU", ShortName = "TRG" },
                new Branch { Name = "PAHANG", ShortName = "PAH" },
                new Branch { Name = "MELAKA", ShortName = "MEL" },
                new Branch { Name = "PERLIS", ShortName = "PLS" }
            };

            var newBranches = states
                .Where(state => !_context.Branches.Any(x => x.Name == state.Name))
                .Select(state => new Branch
                {
                    Name = state.Name,
                    ShortName = state.ShortName,
                    TenantId = 1
                })
                .ToList();

            if (newBranches.Any())
            {
                _context.Branches.AddRange(newBranches); // batch insert
            }
        }
    }
}

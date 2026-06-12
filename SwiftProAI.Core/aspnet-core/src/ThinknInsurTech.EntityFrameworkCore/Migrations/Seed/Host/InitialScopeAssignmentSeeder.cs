using Microsoft.EntityFrameworkCore;
using System.Linq;
using ThinknInsurTech.EntityFrameworkCore;
using ThinknInsurTech.Registration;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialScopeAssignmentSeeder
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialScopeAssignmentSeeder(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {

            var scopeAssignmentsQuery = _context.ScopeAssignments
                .IgnoreQueryFilters()
                .Where(v => v.Description.ToLower() == "others");

            if (!scopeAssignmentsQuery.Any())
            {
                var scopeAssignments = new ScopeAssignment
                {
                    Description = "Others",
                    isActive = true,
                    TenantId = 1
                };
                _context.ScopeAssignments.Add(scopeAssignments);
            }
        }
    }
}

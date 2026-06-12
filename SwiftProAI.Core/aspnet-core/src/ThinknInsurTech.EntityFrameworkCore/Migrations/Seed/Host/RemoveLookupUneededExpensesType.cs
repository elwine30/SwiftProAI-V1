using System.Linq;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class RemoveLookupUnneededExpensesType
    {
        private readonly ThinknInsurTechDbContext _context;

        public RemoveLookupUnneededExpensesType(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Remove()
        {
            var codesToDelete = new[] { "EXPTYPE002", "EXPTYPE003" };

            var lookupsToDelete = _context.Lookups
                                          .Where(l => codesToDelete.Contains(l.Code))
                                          .ToList();

            if (lookupsToDelete.Any())
            {
                _context.Lookups.RemoveRange(lookupsToDelete);
                _context.SaveChanges();
            }
        }
    }
}
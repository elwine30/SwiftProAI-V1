using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Case;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialCaseTypeCreator
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialCaseTypeCreator(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var od = _context.CaseTypes.FirstOrDefault(p => p.Description == "OWN DAMAGE");
            if (od == null)
            {
                _context.CaseTypes.Add(
                    new CaseType
                    {
                        Description = "OWN DAMAGE",
                        ShortName = "OD",
                        IsActive = true
                    });
            }

            var odi = _context.CaseTypes.FirstOrDefault(p => p.Description == "OWN DAMAGE INVESTIGATION");
            if (odi == null)
            {
                _context.CaseTypes.Add(
                    new CaseType
                    {
                        Description = "OWN DAMAGE INVESTIGATION",
                        ShortName = "ODI",
                        IsActive = true
                    });
            }

            var tpbi = _context.CaseTypes.FirstOrDefault(p => p.Description == "THIRD PARTY BODILY INVESTIGATION");
            if (tpbi == null)
            {
                _context.CaseTypes.Add(
                    new CaseType
                    {
                        Description = "THIRD PARTY BODILY INVESTIGATION",
                        ShortName = "TPBI",
                        IsActive = true
                    });
            }

            var tp_odi = _context.CaseTypes.FirstOrDefault(p => p.Description == "THIRD PARTY OWN DAMAGE INVESTIGATION");
            if (tpbi == null)
            {
                _context.CaseTypes.Add(
                    new CaseType
                    {
                        Description = "THIRD PARTY OWN DAMAGE INVESTIGATION",
                        ShortName = "TP ODI",
                        IsActive = true
                    });
            }

        }
    }
}

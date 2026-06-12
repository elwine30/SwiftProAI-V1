using System.Linq;
using ThinknInsurTech.Case;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialStatusCreator
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialStatusCreator(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        //public string Code { get; set; }

        //public string Description { get; set; }

        //public string Closeflag { get; set; }

        //public string Type { get; set; }

        public void Create()
        {
            var ui = _context.Statuses.FirstOrDefault(p => p.Description == "Under Investigation");
            if (ui == null)
            {
                _context.Statuses.Add(
                    new Status
                    {
                        Code = "UI",
                        Description = "Under Investigation",
                        Closeflag = "No",
                        Type = "Pending"            
                    });
            }

            var a = _context.Statuses.FirstOrDefault(p => p.Description == "Adjusters");
            if (a == null)
            {
                _context.Statuses.Add(
                    new Status
                    {
                        Code = "A",
                        Description = "Adjusters",
                        Closeflag = "No",
                        Type = "Pending"
                    });
            }

            var pi = _context.Statuses.FirstOrDefault(p => p.Description == "Pending Invoices");
            if (pi == null)
            {
                _context.Statuses.Add(
                    new Status
                    {
                        Code = "PI",
                        Description = "Pending Invoices",
                        Closeflag = "No",
                        Type = "Pending"
                    });
            }


            var ci = _context.Statuses.FirstOrDefault(p => p.Description == "Completed Invoices");
            if (ci == null)
            {
                _context.Statuses.Add(
                    new Status
                    {
                        Code = "CI",
                        Description = "Completed Invoices",
                        Closeflag = "Yes",
                        Type = "Completed"
                    });
            }

            var c = _context.Statuses.FirstOrDefault(p => p.Description == "Cancelled");
            if (c == null)
            {
                _context.Statuses.Add(
                    new Status
                    {
                        Code = "C",
                        Description = "Cancelled",
                        Closeflag = "Yes",
                        Type = "Completed"
                    });
            }

        }
    }
}

using Stripe.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.EntityFrameworkCore;
using ThinknInsurTech.Vehicles;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialVehicleDetails
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialVehicleDetails(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var vehicleDetails = new List<Vehicle>
            {
                new Vehicle{Make= "PERODUA", Model= "Myvi", Specification= "1.3 G (MT)"},
                new Vehicle{Make= "PERODUA", Model= "Myvi", Specification= "1.3 G (AT)"},
                new Vehicle{Make= "PERODUA", Model= "Myvi", Specification= "1.5 H (AT)"},
                new Vehicle{Make= "PERODUA", Model= "Myvi", Specification= "1.5 AV (AT)"},
                new Vehicle{Make= "PERODUA", Model= "Myvi", Specification= "1.5 X (CVT)"},
                new Vehicle{Make= "PROTON", Model= "X70", Specification= "1.5 TGDi"},
                new Vehicle{Make= "PROTON", Model= "X70", Specification= "1.8 TGDi EXECUTIVE 2WD"},
                new Vehicle{Make= "PROTON", Model= "X70", Specification= "1.8 TGDI PREMIUM 2WD"},
                new Vehicle{Make= "PROTON", Model= "X70", Specification= "1.8 TGDI STANDARD 2WD"},
                new Vehicle{Make= "PROTON", Model= "SAGA", Specification= "1.3 PREMIUM CVT"},
                new Vehicle{Make= "TOYOTA", Model= "COROLLA", Specification= "1.3 GL (NEW)"},
                new Vehicle{Make= "TOYOTA", Model= "COROLLA", Specification= "1.3 SEG"},
                new Vehicle{Make= "TOYOTA", Model= "COROLLA", Specification= "1.6 SE (A)"},
                new Vehicle{Make= "TOYOTA", Model= "COROLLA", Specification= "1.6 SE (M)"},
                new Vehicle{Make= "TOYOTA", Model= "COROLLA", Specification= "1.6 SEG (A)"},
                new Vehicle{Make= "HONDA", Model= "CR-V", Specification= "2.0 i-VTEC"},
                new Vehicle{Make= "HONDA", Model= "CR-V", Specification= "2.4L i-VTEC"},
                new Vehicle{Make= "HONDA", Model= "CITY", Specification= "1.3 (A)"},
                new Vehicle{Make= "HONDA", Model= "CITY", Specification= "1.5 EH (A)"},
                new Vehicle{Make= "HONDA", Model= "CIVIC", Specification= "1.6 EH (A)"},
                new Vehicle{Make= "BMW", Model= "X5 (E53)", Specification= "4.8"},
                new Vehicle{Make= "BMW", Model= "X5 (E53)", Specification= "4.4"},
                new Vehicle{Make= "MERCEDES-BENZ", Model= "S400", Specification= "HYBRID"},
                new Vehicle{Make= "AUDI", Model= "A4", Specification= "1.8(A)"},
                new Vehicle{Make= "AUDI", Model= "A4", Specification= "1.8(A) TURBO (TIP- T)"},
                new Vehicle{Make= "AUDI", Model= "A4", Specification= "2.0(A)"},
                new Vehicle{Make= "VOLKSWAGEN", Model= "POLO", Specification= "1.6"},
                new Vehicle{Make= "VOLKSWAGEN", Model= "BEETLE", Specification= "1.4"},
                new Vehicle{Make= "VOLKSWAGEN", Model= "BEETLE", Specification= "1.6"},
                new Vehicle{Make= "FORD", Model= "RANGER WILDTRAK", Specification= "2.2L"},
                new Vehicle{Make= "FORD", Model= "RANGER WILDTRAK", Specification= "2.0 BI - TURBO"},
                new Vehicle{Make= "CHEVROLET", Model= "COLORADO", Specification= "2.5 LT MT TURBO DIESEL 4X4"},
                new Vehicle{Make= "CHEVROLET", Model= "COLORADO", Specification= "2.8 LT MT TURBO DIESEL 4X4"},
                new Vehicle{Make= "NISSAN", Model= "X-TRAIL", Specification= "2.5L 4WD"},
                new Vehicle{Make= "NISSAN", Model= "X-TRAIL", Specification= "2.0L 2WD"},
                new Vehicle{Make= "MAZDA", Model= "CX5", Specification= "2.5G 2WD H SKYACTIV (A)"},
                new Vehicle{Make= "MAZDA", Model= "CX5", Specification= "2.0G 2WD H SKYACTIV (A)"},
                new Vehicle{Make= "MITSUBISHI", Model= "TRITON", Specification= "2.5"},
                new Vehicle{Make= "MITSUBISHI", Model= "TRITON", Specification= "3.2"},
                new Vehicle{Make= "CHERY", Model= "TIGGO", Specification= "1.6 MT"},
                new Vehicle{Make= "CHERY", Model= "TIGGO", Specification= "SUV 2.0AT"},
                new Vehicle{Make= "SUBARU", Model= "FORESTER", Specification= "2.0i"},
                new Vehicle{Make= "SUBARU", Model= "FORESTER", Specification= "2.0iP"},
                new Vehicle{Make= "SUBARU", Model= "FORESTER", Specification= "2.0XT"},


            };

            foreach(var vehicle in vehicleDetails)
            {
                if(!_context.Vehicles.Any(v => v.Make == vehicle.Make))
                {
                    _context.Vehicles.Add(vehicle);
                }
            }
        }

    }
}

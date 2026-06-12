using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetRegistrationExportInput
    {
        public int RegisterId { get; set; }
        public string VehicleNo { get; set; }
    }
}

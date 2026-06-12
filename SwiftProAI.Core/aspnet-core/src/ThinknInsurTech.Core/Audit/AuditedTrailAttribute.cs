using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinknInsurTech.Audit
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AuditedTrailAttribute : Attribute
    {
    }
}

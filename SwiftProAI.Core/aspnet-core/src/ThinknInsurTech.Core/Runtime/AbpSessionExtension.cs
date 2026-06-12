using Abp.Extensions;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinknInsurTech.Runtime
{
    public static class AbpSessionExtension
    {
        public static long? GetCurrentOUId(this IAbpSession session)
        {
            if (session is IOUAbpSession ouSession)
                return ouSession.CurrentOUId;

            return null;
        }
    }
}

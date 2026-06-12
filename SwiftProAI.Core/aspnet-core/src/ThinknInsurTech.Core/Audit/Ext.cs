using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinknInsurTech.Audit
{
    public static class Ext
    {
        public static IEnumerable<EntityEntry> BaseEntityEntries(this ChangeTracker changeTracker, bool ignoreUnChanged = true)
        {
            IEnumerable<EntityEntry> lst = changeTracker.Entries();
            return ignoreUnChanged ? lst.Where(x => x.State != EntityState.Unchanged) : lst;
        }
    }
}

using System;
using System.Collections.Generic;
using Abp.Json;

namespace ThinknInsurTech.ExtraProperties
{
    [Serializable]
    public class ExtraPropertyDictionary : Dictionary<string, object>
    {
        public ExtraPropertyDictionary()
        {
        }

        public ExtraPropertyDictionary(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }

        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
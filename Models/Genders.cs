using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Genders
    {
        male=1,
        female=2,
    }
}

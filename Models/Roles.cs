using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Models
{
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum Roles
    {
        Admin=1,
        Employee=2,
        Customer=3,
    }

//    class Person
//{
//    int Age { get; set; }

//    [ScriptIgnore]
//    Gender Gender { get; set; }

//    string GenderString { get { return Gender.ToString(); } }
//class Person
//{
//    int Age { get; set; }

//    [ScriptIgnore]
//    Gender Gender { get; set; }

//    string GenderString { get { return Gender.ToString(); } }
//}

}

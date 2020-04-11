using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace UniOrm.Common
{
    //    string json = @"{'name': 'Jeremy Dorn','location': {'city': 'San Francisco','state': 'CA'},'pets': [{'type': 'dog','name': 'Walter'}]}";

    //    JObject jobj = JObject.Parse(json);

    //    dynamic obj = new JObjectAccessor(jobj);

    //    Console.WriteLine($"{obj.name}: {obj.location.city} {obj.location.state}");
    //Console.WriteLine($"{obj.pets[0].type}: {obj.pets[0].name}");
    public class JObjectAccessor : DynamicObject
    {
        JToken obj;

        public JObjectAccessor(JToken obj)
        {
            this.obj = obj;
        }
        public JObjectAccessor(string objstring)
        {
            obj = JToken.Parse(objstring); 
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            if (obj == null) return false;

            var val = obj[binder.Name];

            if (val == null) return false;

            result = Populate(val);

            return true;
        }


        private object Populate(JToken token)
        {
            var jval = token as JValue;
            if (jval != null)
            {
                return jval.Value;
            }
            else if (token.Type == JTokenType.Array)
            {
                var objectAccessors = new List<object>();
                foreach (var item in token as JArray)
                {
                    objectAccessors.Add(Populate(item));
                }
                return objectAccessors;
            }
            else
            {
                return new JObjectAccessor(token);
            }
        }
    }
}

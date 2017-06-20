using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PdfFiller
{
    public class JTokenToDictionary
    {
        public static void Create(JObject token,Dictionary<string,string>  dict)
        {
            foreach (var obj in token)
            {
                if (obj.Value.Type != JTokenType.Object)
                {
                    dict.Add(obj.Key,obj.Value.Value<string>());
                }
                else
                {
                    Create((JObject) obj.Value,dict);
                }
            }
            
        } 
    }
}

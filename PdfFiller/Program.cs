using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using Newtonsoft.Json.Linq;

namespace PdfFiller
{
    class Program
    {
        static void Main(string[] args)
        {
            //(@"C:\Users\lavi\Desktop\Hackaton\testpdf\form.pdf"
            PdfReader rdr = new PdfReader(args[0]);
            var dict = JArray.Parse(File.ReadAllText(args[2])).ToObject<KeyValue[]>().ToDictionary(json => json.Key, json => json.Value);

            PdfStamper stamper = new PdfStamper(rdr,new FileStream(args[1],FileMode.Create));
            foreach (string key in stamper.AcroFields.Fields.Keys)
            {
                stamper.AcroFields.SetField(key, dict[key]);
            }
            
            stamper.Close();
            rdr.Close();

        }
    }
}

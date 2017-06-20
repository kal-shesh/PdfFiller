using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
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
            var format = JObject.Parse(File.ReadAllText(args[2])).ToObject<Format>();
            var dict =    format.FormItems.ToDictionary(json => json.Key, json => json.Value);

            PdfStamper stamper = new PdfStamper(rdr,new FileStream(args[1],FileMode.Create));


            foreach (var imageFormat in format.Images)
            {
                var pdfContentByte = stamper.GetOverContent(1);
                
                var itemsPositions = stamper.AcroFields.GetFieldPositions(imageFormat.FormName);
                foreach (var pos in itemsPositions)
                {
                    Image image = Image.GetInstance(imageFormat.ImagePath);
                    
                    image.ScaleToFit(pos.position.Width*3,pos.position.Height);
                    image.SetAbsolutePosition(pos.position.Left,pos.position.Bottom);
                    pdfContentByte.AddImage(image);
                }
                
            }            


            foreach (string key in stamper.AcroFields.Fields.Keys)
            {
                if (key.ToLower() == "today")
                {
                    stamper.AcroFields.SetField(key, DateTime.Now.Date.ToString("d"));
                }
                else
                {
                    if (dict.ContainsKey(key))
                    {
                        stamper.AcroFields.SetField(key, dict[key]);
                    }
                }
                
            }
            
            stamper.Close();
            rdr.Close();

        }
    }
}

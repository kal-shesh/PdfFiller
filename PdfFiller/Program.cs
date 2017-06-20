using System;
using System.Collections.Generic;
using System.Configuration;
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
            PdfReader rdr = new PdfReader(args[0]);
            var dict = new Dictionary<string,string>();
            JTokenToDictionary.Create(JObject.Parse(File.ReadAllText(args[2])),dict);
            

            PdfStamper stamper = new PdfStamper(rdr,new FileStream(args[1],FileMode.Create));
            var format = new Format
            {
                Images = new List<PdfImage>
                {
                    new PdfImage {ImagePath = ConfigurationManager.AppSettings["Soldier"],FormName = "Soldier Signature" },
                    new PdfImage {ImagePath = ConfigurationManager.AppSettings["Commander"],FormName = "Commander Signature" }
                }
            };

            foreach (var imageFormat in format.Images)
            {
                var pdfContentByte = stamper.GetOverContent(1);
                
                var itemsPositions = stamper.AcroFields.GetFieldPositions(imageFormat.FormName);
                foreach (var pos in itemsPositions)
                {
                    Image image = Image.GetInstance(imageFormat.ImagePath);
                    
                    image.ScaleToFit(pos.position.Width,pos.position.Height);
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

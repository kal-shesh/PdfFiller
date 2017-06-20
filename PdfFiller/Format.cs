using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFiller
{
    public class Format
    {
        public List<KeyValue> FormItems{ get; set; }
        public List<PdfImage> Images{ get; set; }
    }
}

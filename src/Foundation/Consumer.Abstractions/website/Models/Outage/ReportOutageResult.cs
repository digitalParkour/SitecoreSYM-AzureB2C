using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.Outage
{
    public class ReportOutageResult
    {

        public decimal SYMB2CCode { get; set; }

        public decimal OraCode { get; set; }

        public decimal ReportNumber { get; set; }
        
        public bool AlreadyReported { get; set; }

        public bool Error { get; set; }
    }
}

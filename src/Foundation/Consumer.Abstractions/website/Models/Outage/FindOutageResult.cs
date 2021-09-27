using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.Outage
{
    public class FindOutageResult
    {
        public decimal OraCode { get; set; }
      
        public decimal SYMB2CCode { get; set; }
       
        public string AccountNumber { get; set; }
        
        public string Name { get; set; }
       
        public decimal AreaCode { get; set; }
        
        public decimal PhoneNumber { get; set; }
       
        public string MeterNumber { get; set; }
        
        public string Address { get; set; }
       
        public string Apartment { get; set; }
        
        public string CallerType { get; set; }
       
        public string CustomerType { get; set; }
      
        public string OpenReport { get; set; }
        
    }
}

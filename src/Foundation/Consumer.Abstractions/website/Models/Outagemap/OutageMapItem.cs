using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.Outagemap
{
    public class OutageMapItem
    {
    
     //   public string MapBlock { get; set; }
      

        
        public int OutageNumber { get; set; }
        

        
     //   public decimal Affected { get; set; }
        
        
        public int CustAffected { get; set; }
        

        
        public decimal X1 { get; set; }
        
        public decimal Y1 { get; set; }
       
      //  public decimal Z1 { get; set; }
       

        
      //  public decimal X2 { get; set; }

        
       // public decimal Y2 { get; set; }
       

        
     //   public decimal Z2  { get; set; }
       

        
    //    public decimal X3 { get; set; }
        

        
      //  public decimal Y3 { get; set; }
        

        
      //  public decimal Z3 { get; set; }
        

        
        //public decimal X4 { get; set; }
        

        
        //public decimal Y4 { get; set; }
        

        
        //public decimal Z4 { get; set; }
       

        
        //public decimal X5 { get; set; }
        

        
        //public decimal Y5 { get; set; }
       

        
        //public decimal Z5 { get; set; }
       

        
        public bool CrewDeployed { get; set; }
       

        
      //  public System.DateTime OutageTime { get; set; }
        

        
       // public System.DateTime LocalOutageTime { get; set; }
        

        
        public string StringOutageTime { get; set; }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket
{
    internal class Customer
    {
       public string Name { get; set; }
       public static int NumberOfCustomer = 0;
       public int ID { get; set; }

        public string Phone { get; set; }
       public List<Customer_Bill> bills;
       public double Points { get; set;}  
       public Customer()
        {
            NumberOfCustomer++;
            bills = new List< Customer_Bill>();
            ID = NumberOfCustomer;
        }    
        
 //Auto increment 
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}

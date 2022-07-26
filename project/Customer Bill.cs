using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket
{
    internal class Customer_Bill
    { 
        static int NumberOfBills = 1;
        public int CustomerId { get; set; }  
        public String CustomerName { get; set; }
        public int CashierId { get; set; }
        public string CashierName { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedTime;
        public int BillId ; //Auto increment
        public List<ProductNeed> Customer_Product;

        public Customer_Bill()
        {
            NumberOfBills++;
            CreatedTime = DateTime.Now;
            Customer_Product = new List<ProductNeed>();
            BillId = NumberOfBills;
        }

    }
}

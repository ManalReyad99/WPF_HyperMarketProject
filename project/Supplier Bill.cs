using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket
{
    internal class Supplier_Bill
    {
        static int NumberOfBills = 0;
        public int SupplierId { get; set; }
        public int BillId { get; set; }
        public String SupplierName { get; set; }
        public List<ProductNeed> Supplier_Product { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedTime;
        public Supplier_Bill()
        {
            NumberOfBills++;
            CreatedTime = DateTime.Now;
            Supplier_Product = new List<ProductNeed>();
            BillId = NumberOfBills;
    }
     //Auto increment 

    }
}

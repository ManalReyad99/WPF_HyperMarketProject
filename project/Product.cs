using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket
{
    //public struct Group
    //{
    //    public DateTime ExpireDate { get; set; }
    //    public long Amount { get; set; }
    //}
    internal class Product
    {
        public string Name { get; set; }

        static int NumberOfProduct = 0;
        public int ID { get; set; }
        public double PriceForBuy { get; set; }
        public double PriceForSell { get; set; }
        public long Amount { get; set; }
        
        public string category { get; set; } //category id
        public DateTime ExpireDate { get; set; }

        public bool IsExist = false; // this product exist in my market or not 
      
                                 
        
        //for manager
        public Product(int ID, double PriceForBuy, double PriceForSell, long Amount, string Name, DateTime ExpireDate, string category)
        {
            this.ID = ID;
            this.PriceForBuy = PriceForBuy;
            this.Amount = Amount;
            this.Name = Name;
            this.PriceForSell = PriceForSell;
            this.ExpireDate = ExpireDate;
            this.category = category;
        }
        //increment ID with supplier x adding new product doesn't exist in our market reference list
        public Product(string name, double priceForSell, string category)
        {

            NumberOfProduct++;
            ID = NumberOfProduct;
            Name = name;
            this.category = category;
            PriceForSell = priceForSell;
        }
        // without increment the id it will assign by existing one for supplier Y
        public Product(string name, double priceForSell, string category, int Id)
        {
            this.Name = name;
            this.category = category;
            this.PriceForSell = priceForSell;
            this.ID = Id;
        }
        public Product()
        {
            NumberOfProduct++;
            ID = NumberOfProduct++;
            PriceForSell = 0;
            PriceForBuy = 0;
            ExpireDate = DateTime.Now;
        }
        public override string ToString()
        {
            return $"{Name}";
        }

    }
}

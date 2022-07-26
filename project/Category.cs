using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket
{
    internal class Category
    {
        public string Name { get; set; }
        //C-TwoDigit
        static int numberOfCategory = 0;
        public int ID { get; set; }
        public List<Product> Products { get; set; }
        public Category()
        {
            numberOfCategory++;
            ID = numberOfCategory;
            Products = new List<Product>();
        }
        //set Id
        public Category(string Name, int ID)
        {
            this.Name = Name;
            this.ID = ID;
            Products = new List<Product>();
        }
        public Category(string Name)
        {
            numberOfCategory++;
            this.Name = Name;
            ID = numberOfCategory;
            Products = new List<Product>();
        }


        public Product GetProduct(string ProductName)
        {
            foreach (Product Product in Products)
            {
                if (ProductName == Product.Name)
                    return Product;
            }
            return null;
        }
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}

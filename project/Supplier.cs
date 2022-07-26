using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket
{
    internal class Supplier
    {

        public string Name { get; set; }
        // S-TwoDigits 
        static int NumberOfSupplier = 0;
        public int ID { get; set; }
        public string PhoneNumber { get; set; }
        public List<Supplier_Bill> bills;
        public Category category;
        public string Cname { get; set; }
        public Supplier(string Name, string PhoneNumber, string Cname)
        {
            this.Name = Name;
            this.PhoneNumber = PhoneNumber;
            NumberOfSupplier++;
            bills = new List<Supplier_Bill>();
            this.Cname = Cname;
            this.ID = NumberOfSupplier;
        }
        // category --> category name ** name--> product Name
        public void Addingproduct(string category, string name, double priceForSell)
        {
            Category category2 = Market.market.Categories.Find(x => x.Name == category);
            Product produ = category2.Products.Find(x => x.Name == name);
            // if this product exist in our category products refrences 
            // in another supplier not in this category
            Product product1 = this.category.Products.Find(x => x.Name == name);
            if (produ != null )
            {
                if (product1 == null)
                {
                    Product product = new Product(produ.Name, produ.PriceForSell, produ.category, produ.ID);
                    this.category.Products.Add(product);
                }
            }
            else
            {
                // create a new product and assign new Id to it 
                Product product = new Product(name, priceForSell, category);
                this.category.Products.Add(product);
                category2.Products.Add(product);

            }

        }
        //remove product from this supplier list and checking if there is another supplier 
        //provide this product or not to take action of removing it from market
        //product or not
        public bool RemoveProduct(string category, string name, double priceForSell)
        {
            //Product produ = Market.market.Products.Find(x => x.Name == name);
            Category category1 = Market.market.Categories.Find(x => x.Name == category);
            Product supProduct = category1.Products.Find(x => x.Name == name);
            if (supProduct != null)
            {
                bool existInAnotherSupp = false;
                // loop to search if anyone don't have this product in my market
                // supplier
                foreach (Supplier supplier in Market.market.Suppliers)
                {
                    if (supplier.ID != ID)
                    {
                        if (supplier.category.Products.Contains(supProduct))
                        {
                            existInAnotherSupp = true;
                        }
                    }
                }
                this.category.Products.Remove(supProduct);
                if (!existInAnotherSupp)
                    category1.Products.Remove(supProduct);
                return true;
            }
            else
                return false;
        }
    }
}
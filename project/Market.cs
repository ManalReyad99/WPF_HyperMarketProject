using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket
{
    internal sealed class Market
    {
        //singlton ISA 
        public double Budget { get; set; }
        public string Name { get; set; }
        public List<Category> Categories;
        public List<Customer> Customers;
        public List<Supplier> Suppliers;
        public List<Cashier> Cashiers;
        private Market()
        {
            Budget = 1500000;
            Name = "United Group";
            Categories = new List<Category>();
            Customers = new List<Customer>();
            Suppliers = new List<Supplier>();
            Cashiers = new List<Cashier>();

        }
        private static Market mark = null;
        public static Market market
        {
            get
            {
                if (mark == null)
                {
                    mark = new Market();
                }
                return mark;
            }
        }
        public List<Product> ReportOExistingProducts()
        {
            List<Product> products = new List<Product>();
            foreach (Category cate in Categories)
            {
                foreach (Product item in cate.Products)
                {
                    products.Add(item);
                }
            }
            return products;
        }
        public List<Product> ReportOfExpireProducts()
        {
            List<Product> products = new List<Product>();
            foreach (Category cate in Categories)
            {
                foreach (Product item in cate.Products)
                {
                    if (item.ExpireDate < DateTime.Now)
                        products.Add(item);
                }
            }
            return products;
        }
        //Get Casher From user name
        public Cashier GetCasher(string username)
        {
            foreach (Cashier casher in Market.market.Cashiers)
            {
                if (username == casher.Username)
                {
                    return casher;
                }

            }
            //testing data
            return new Cashier("Ahmed", "ah", "12345", 8, "0145687" , 4000);
        }
        //Get Customer From Name
        public Customer GetCustomer(string CustomerNameID)
        {
            foreach (Customer customer in Market.market.Customers)
            {
                if (CustomerNameID == customer.ToString())
                {
                    return customer;
                }

            }
            return null;
        }

    }
}
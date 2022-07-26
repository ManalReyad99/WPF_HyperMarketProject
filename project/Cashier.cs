using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket
{
    internal class Cashier
    {
        public string Name { get; set; }

        static int NumberOfCashers = 0;
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int WorkingHours { get; set; }
        public string Phone { get; set; }
        public float Salary { get; set; }

        public event Action<Customer_Bill> BillCreated;
        public event Action CustomerAdded;


        //public List<ProductNeed> Products=new List<ProductNeed>();
        //Check For Product's Existance
        public Cashier() //:this($"hamdy{this.ID}" ,$"cashier{this.ID}" ,"root" , 12 , new List<string>{ "0100005558"}  , 2500.0)
        {
            NumberOfCashers++;
            ID = NumberOfCashers;
        }

        public Cashier(string Name, String UserName, String Password, int WorkingHours, string phone, float Salary)
        {
            NumberOfCashers++;
            this.WorkingHours = WorkingHours;
            this.Salary = Salary;
            this.Password = Password;
            this.Username = UserName;
            this.Name = Name;
            Phone = phone;
            ID = NumberOfCashers;

        }


        //check existance of product search 
        public bool ProductIsExist(Product product, Category category)
        {
            foreach (Product p in category.Products)
            {
                if (p.ID == product.ID)
                {
                    return true;
                }

            }
            return false;
        }

        // check existance of product and it's amount covered the customer Need
        public Category retCat(string catName)
        {
            foreach (Category item in Market.market.Categories)
            {
                if (item.ToString() == catName)
                {
                    return item;
                }
            }
            return null;
        }
        public bool ProductCoverd(ProductNeed product, Category category)
        {
            foreach (Product p in category.Products)
            {
                if (p.ID == product.Product.ID)
                {
                    if (p.Amount >= product.AmountNeeded && p.IsExist == true)
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        //check if customer is not exist in customer List or not before adding
        public void AddCustomer(Customer customer)
        {
            if (!Market.market.Customers.Contains(customer))
            {
                Market.market.Customers.Add(customer);
            }

            //int index = Market.market.Customers.IndexOf(customer);
            //customer = Market.market.Customers[index];
            if (CustomerAdded != null)
            {
                CustomerAdded();
            }
        }
        //create Customer Bill
        public void CreateBill(Customer customer)
        {
            Customer_Bill customer_Bill = new Customer_Bill();
            customer_Bill.CashierId = this.ID;
            customer_Bill.CustomerId = customer.ID;
            customer_Bill.CashierName = this.Name;
            customer_Bill.CustomerName = customer.Name;
            customer.bills.Add(customer_Bill);
        }


        //edit amount of existance product
        
        public bool EditProduct(ProductNeed pnew, Customer customer)
        {
            bool Exist = false;
            bool edited = false;
            int counter = -1;
            int x = 0;
            foreach (ProductNeed productNeed in customer.bills[customer.bills.Count - 1].Customer_Product)
            {
                ++counter;
                if (productNeed.Product.Name == pnew.Product.Name)
                {
                    Exist = true;
                    x = counter;

                }
            }
            if (Exist)
            {
                if (this.ProductCoverd(pnew, retCat(pnew.Product.category)))
                {
                    DeleteProduct(customer.bills[customer.bills.Count - 1].Customer_Product[x], customer);
                    customer.bills[customer.bills.Count - 1].Customer_Product.Add(pnew);
                    edited = true;
                }
                else
                {
                    edited = false;
                }
            }
            else
                edited = false;
            return edited;

        }

        //adding product to the last added  cutomer's bill and throw exception if 
        //the product couldn't cover amount or not exist
        public bool AddProduct(ProductNeed p, Customer customer)
        {
            bool Exist = false;
            bool added = false;
            int counter = -1;
            int x=0;
            foreach (ProductNeed productNeed in customer.bills[customer.bills.Count - 1].Customer_Product)
            {
                ++counter;
                if(productNeed.Product.Name==p.Product.Name)
                {
                    Exist = true;
                    x = counter;

                }
            }
            if(Exist)
            {
            ProductNeed check = p;

            check.AmountNeeded += customer.bills[customer.bills.Count - 1].Customer_Product[x].AmountNeeded;
                if (this.ProductCoverd(check, retCat(check.Product.category)))
                {
                    DeleteProduct(customer.bills[customer.bills.Count - 1].Customer_Product[x], customer);
                    customer.bills[customer.bills.Count - 1].Customer_Product.Add(check);
                    added = true;
                }
                else
                {
                    added= false;
                }
            }
            // for first time adding product to our basket
            else if (this.ProductCoverd(p, retCat(p.Product.category)))
            {
                customer.bills[customer.bills.Count - 1].Customer_Product.Add(p);
                added = true;
            }
            else
                added= false;
            return added;

        }
        //Remove product to the last added  cutomer's bill

        public bool DeleteProduct(ProductNeed p, Customer customer)
        {
            bool Exist = false;
            int counter = -1;
            int x = 0;
            foreach (ProductNeed productNeed in customer.bills[customer.bills.Count - 1].Customer_Product)
            {
                ++counter;
                if (productNeed.Product.Name == p.Product.Name)
                {
                    Exist = true;
                    x = counter;
                }
            }
            if (Exist)
            {
                customer.bills[customer.bills.Count - 1].Customer_Product.RemoveAt(x);
       
                return true;
            }
            else
                return false;
        }
        //remove that bill "لو الكستمر رجع ف كلامه"

        public void FinalDeletForBill(Customer customer)
        {
            customer.bills.Remove(customer.bills[customer.bills.Count - 1]);
        }

        //Complete the payment basket list of product every prodct have amount and price 
        // and decrease amount of stock products in Market.market
        public void pay(Customer customer)
        {
            //increase budget 
            double total = 0;
            foreach (ProductNeed p in customer.bills[customer.bills.Count - 1].Customer_Product)
            {
                total += (p.AmountNeeded * p.Product.PriceForBuy);
            }
            customer.bills[customer.bills.Count - 1].TotalPrice = total;

            Market.market.Budget += total;

            // decrease amount of products
            foreach (ProductNeed item in customer.bills[customer.bills.Count - 1].Customer_Product)
            {
                int catIndex = Market.market.Categories.IndexOf(retCat(item.Product.category));
                int ProdIndex = Market.market.Categories[catIndex].Products.IndexOf(item.Product);
                Market.market.Categories[catIndex].Products[ProdIndex].Amount -= item.AmountNeeded;

            }
            //calculate customer Points
            double revenu = 0;
            foreach (ProductNeed item in customer.bills[customer.bills.Count - 1].Customer_Product)
            {
                revenu += (item.Product.PriceForBuy - item.Product.PriceForSell);
            }
            customer.Points += revenu * (double)0.01; //points is 10% of revenue
            Customer_Bill customer_Bill = customer.bills[customer.bills.Count - 1];
            if (BillCreated != null)
            {
                BillCreated(customer_Bill);
            }
        }
        //Pay with points
        public void PayWithPoints(Customer customer)
        {
            //decrease budjet
            double total = 0;
            foreach (ProductNeed p in customer.bills[customer.bills.Count - 1].Customer_Product)
            {
                total += (p.AmountNeeded * p.Product.PriceForBuy);
            }
            if (total <= customer.Points)
            {
                Market.market.Budget -= total;
                //decrease points
                customer.Points -= total;
                total = 0;
            }
            else if (total > customer.Points)
            {
                total -= customer.Points;
                //decrease points
                customer.Points = 0;
            }
            customer.bills[customer.bills.Count - 1].TotalPrice = total;
            // decrease amount of products
            foreach (ProductNeed item in customer.bills[customer.bills.Count - 1].Customer_Product)
            {
                int catIndex = Market.market.Categories.IndexOf(retCat(item.Product.category));
                int ProdIndex = Market.market.Categories[catIndex].Products.IndexOf(item.Product);
                Market.market.Categories[catIndex].Products[ProdIndex].Amount -= item.AmountNeeded;

            }
            Customer_Bill customer_Bill = customer.bills[customer.bills.Count - 1];
            if (BillCreated != null)
            {
                BillCreated(customer_Bill);
            }

        }

    }
}

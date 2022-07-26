using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket
{
    internal sealed class Manager
    {
        //Sigleton
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int ID { get; set; }



        public Manager()
        {
            Name = "Admin";
            UserName = "Admin";
            Password = "Admin";
            ID = 255;
        }
        private static Manager mange = null;
        public static Manager manager
        {
            get
            {
                if (mange == null)
                {
                    mange = new Manager();
                }
                return mange;
            }
        }

        private Category retCat(string catName)
        {
            foreach (Category item in Market.market.Categories)
            {
                if (item.Name == catName)
                {
                    return item;
                }
            }
            return null;
        }
        //add new Cashier
        public void AddCashier(Cashier Cashier)
        {
            if (!Market.market.Cashiers.Contains(Cashier))
            {
                Market.market.Cashiers.Add(Cashier);
            }
        }
        // add new Supplier
        public void AddSupplier(Supplier supplier)
        {
            if (!SupplierIsExist(supplier))
            {
                Market.market.Suppliers.Add(supplier);
            }
        }

        //Add category to categories if is not exist
        public void AddCategory(Category category)
        {
            if (!CategoryIsExist(category))
            {
                Market.market.Categories.Add(category);
            }
        }
        //create Supplier Bill
        public void CreateBill(Supplier supplier)
        {
            Supplier_Bill Supplier_Bill = new Supplier_Bill();
            Supplier_Bill.SupplierId = supplier.ID;
            Supplier_Bill.SupplierName = supplier.Name;
            supplier.bills.Add(Supplier_Bill);
        }

        //Remove Supplier
        public void DeleteSupplier(Supplier supplier)
        {
            if (SupplierIsExist(supplier))
                Market.market.Suppliers.Remove(supplier);
        }
        //Remove Cashier
        public void DeleteCashier(Cashier cashier)
        {
            if (CashierIsExist(cashier))
                Market.market.Cashiers.Remove(cashier);
        }
      

        //Remove category
        public void DeleteCategory(Category category)
        {
            if (CategoryIsExist(category))
                Market.market.Categories.Remove(category);
        }
        // Search For Cashier
        public bool CashierIsExist(Cashier cashier)
        {
            foreach (Cashier item in Market.market.Cashiers)
            {
                if (item.ID == cashier.ID)
                    return true;
            }
            return false;
        }
        //Search For Supplier
        public bool SupplierIsExist(Supplier supplier)
        {
            foreach (Supplier item in Market.market.Suppliers)
            {
                if (item.ID == supplier.ID)
                    return true;
            }
            return false;
        }

        //search for category
        public bool CategoryIsExist(Category category)
        {
            foreach (Category item in Market.market.Categories)
            {
                if (item.ID == category.ID)
                {
                    return true;
                }
            }
            return false;
        }

        //Add product to supplier-list
        public void AddSupplierProduct(Supplier supplier, ProductNeed product)
        {
            int lastBillIndex = supplier.bills.Count - 1;
            // check of this product exist in supplier bill then add new amount
            bool foundOrNot = false;
            int idx = 0;
            foreach (ProductNeed item in supplier.bills[lastBillIndex].Supplier_Product)
            {
                if (item.Product.ID == product.Product.ID)
                {
                    foundOrNot = true;
                    break;
                }
                idx++;
            }
            if (foundOrNot)
            {

                ProductNeed che = product;
                che.AmountNeeded += supplier.bills[lastBillIndex].Supplier_Product[idx].AmountNeeded;

                supplier.bills[lastBillIndex].Supplier_Product[idx] = che;

                double oldTotalPrice = supplier.bills[lastBillIndex].TotalPrice;

                double newTotalPrice = 0;
                newTotalPrice += oldTotalPrice;
                newTotalPrice += product.AmountNeeded * product.Product.PriceForSell;
                if (CheckBudgedCovered(newTotalPrice))
                {

                    supplier.bills[lastBillIndex].TotalPrice = newTotalPrice;
                }
                else
                {
                    //warning if the budget
                    throw new Exception("budget not cover this amount of products");
                }

            }
            else
            {
                double oldTotalPrice = supplier.bills[lastBillIndex].TotalPrice;

                double newTotalPrice = 0;
                newTotalPrice += oldTotalPrice;

                newTotalPrice += product.AmountNeeded * product.Product.PriceForSell;
                if (CheckBudgedCovered(newTotalPrice))
                {
                    supplier.bills[lastBillIndex].Supplier_Product.Add(product);
                    supplier.bills[lastBillIndex].TotalPrice = newTotalPrice;
                }
                else
                {
                    //warning if the budget
                    throw new Exception("budget not cover this amount of products");
                }

            }
        }

        //Remove product from supplier bill
        public void RemoveSupplierProduct(Supplier supplier, ProductNeed product)
        {
            int lastBillIndex = supplier.bills.Count - 1;
           

            //int productIndex = supplier.bills[lastBillIndex].Supplier_Product.IndexOf(product);
            ProductNeed PN = supplier.bills[lastBillIndex].Supplier_Product.Find(x => x.Product.ID == product.Product.ID);
           
            int amountNeeded = PN.AmountNeeded;

            double priceOfsell = PN.Product.PriceForSell;

            double priceOfRemovedProduct = priceOfsell * amountNeeded;

            supplier.bills[lastBillIndex].TotalPrice -= priceOfRemovedProduct;

            supplier.bills[lastBillIndex].Supplier_Product.Remove(product);

        }
        //check budget covered 
        public bool CheckBudgedCovered(double billprice)
        {

            return ((Market.market.Budget - billprice) >= 0) ? true : false;
        }
        //paymnet to supplier
        public void pay(Supplier supplier)
        {
            //decrease Budget amount
            int lastBillIndex = supplier.bills.Count - 1;
            Market.market.Budget -= supplier.bills[lastBillIndex].TotalPrice;

            //increase amount of product in Category
            foreach (ProductNeed item in supplier.bills[lastBillIndex].Supplier_Product)
            {
                Category cat = retCat(item.Product.category);
                int indexofcat = Market.market.Categories.IndexOf(cat);
                Product temp = null;
               foreach(Product it in cat.Products)
                {
                    if(it.Name == item.Product.Name)
                    {
                        temp=it;    
                    }
                }
                ////int indexOfProduct = Market.market.Categories[indexofcat].Products.IndexOf(item.Product);
                if (temp != null)
                {
                    temp.Amount += item.AmountNeeded;
                    if(temp.PriceForBuy < item.Product.PriceForBuy)
                    {
                        temp.PriceForBuy=item.Product.PriceForBuy;
                    }
                    temp.IsExist = true;
                }

            }


        }
    }
}


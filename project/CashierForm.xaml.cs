using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Drawing;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace HyperMarket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CashierForm : Window
    {

        Customer customer;
        Cashier casher;
        List<Customer_Bill> BillsList = new List<Customer_Bill>();
        Validation Valid = new Validation();
        public CashierForm(string CashierUsername)
        {
            InitializeComponent();
            casher = Market.market.GetCasher(CashierUsername);
            casher.BillCreated += this.AddDailyBills;
            casher.CustomerAdded += this.AddCustomerList;
            if (Market.market.Customers != null)
            {
                foreach (Customer customer in Market.market.Customers)
                {
                    CBForSearchCustmoer.Items.Add(customer.Name);
                }
            }
            if (Market.market.Categories != null)
            {
                foreach (Category category in Market.market.Categories)
                {
                    CBForSearchCategory.Items.Add(category.Name);
                }
            }


        }
      
private void AddCustomerList()
        {
            CBForSearchCustmoer.Items.Add(Market.market.Customers[Market.market.Customers.Count - 1].ToString());
        }
        private void AddDailyBills(Customer_Bill customerBill)
        {
            bool ISReapeated = false;
            foreach (Customer_Bill bill in BillsList)
            {
                if (customerBill.BillId == bill.BillId)
                {
                    ISReapeated = true;
                }
            }
            if (!ISReapeated)
            {
                BillsList.Add(customerBill);
                Bills.ItemsSource = null;
                Bills.ItemsSource= BillsList; double total = 0;
                foreach (Customer_Bill bill in BillsList)
                {
                    total += bill.TotalPrice;
                }
                TotalGainedmoney.Content = total.ToString();
                NoOfBiils.Content = BillsList.Count.ToString();
            }
            else
            {
                MessageBox.Show("bb");
                MessageBox.Show("Invalid,The Bill Already Created");
            }
        }



        private void CreateBillBtn_Click(object sender, RoutedEventArgs e)
        {
            DailyBillsPanal.Visibility = Visibility.Hidden;
            CreateBillPanal.Visibility = Visibility.Visible;
            CustomerPanel.Visibility = Visibility.Hidden;
        }

        private void CustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            DailyBillsPanal.Visibility = Visibility.Hidden;
            CreateBillPanal.Visibility = Visibility.Hidden;
            CustomerPanel.Visibility = Visibility.Visible;

        }

        private void DailyBillsBtn_Click(object sender, RoutedEventArgs e)
        {
            DailyBillsPanal.Visibility = Visibility.Visible;
            CreateBillPanal.Visibility = Visibility.Hidden;
            CustomerPanel.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DailyBillsPanal.Visibility = Visibility.Hidden;
            CreateBillPanal.Visibility = Visibility.Visible;
            CustomerPanel.Visibility = Visibility.Hidden;

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void AddCustome_Click(object sender, RoutedEventArgs e)
        {
            Regex RePhone = new Regex("^(011|012|010)[0-9]{8}");

            bool IsValidPhone = RePhone.IsMatch(CustomerPhone.Text);
           bool IsValidName = Valid.IsName(CustomerName.Text);
            if ((!String.IsNullOrEmpty(CustomerPhone.Text)) && IsValidName && IsValidPhone)
            {
                customer = new Customer();
                customer.Name = CustomerName.Text;
                customer.Phone = CustomerPhone.Text;
                customer.Points = 0;
                casher.AddCustomer(customer);
                AddedCustomerData.ItemsSource = null;
               AddedCustomerData.ItemsSource=Market.market.Customers;
                CustomerName.Clear();

                CustomerPhone.Clear();
                MessageBox.Show("Customer Added Successfully");
            }
            else
            {
                MessageBox.Show("Enter Valid Data");
            }
        }

        private void SaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddedCustomerData.ItemsSource = null;
        }

        private void CBForSearchCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBForSearchProduct.Items.Clear();



            Category categoryType = casher.retCat(CBForSearchCategory.SelectedItem.ToString());
            if (categoryType != null)
            {
                foreach (Product product in categoryType.Products)
                {
                    CBForSearchProduct.Items.Add(product.Name);
                }
            }
            else
            {
                MessageBox.Show("Not Valid Data");
            }
        }

        private void ButtonForAdd_Click(object sender, RoutedEventArgs e)
        {
            
if (!(string.IsNullOrEmpty(QuantityTxtBox.Text))
&& (CBForSearchCategory.SelectedItem != null)
&& (CBForSearchProduct.SelectedItem != null) && (CBForSearchCustmoer.SelectedItem != null))
            {
                customer = Market.market.GetCustomer(CBForSearchCustmoer.SelectedItem.ToString());
                Category category = casher.retCat(CBForSearchCategory.SelectedItem.ToString());
                Product product = category.GetProduct(CBForSearchProduct.SelectedItem.ToString());
                ProductNeed productNeed = new ProductNeed();
                productNeed.Product = product;
                productNeed.AmountNeeded = int.Parse(QuantityTxtBox.Text); if (!casher.AddProduct(productNeed, customer))
                {
                    MessageBox.Show("Product Not Cover");
                }
                CustomerBillGrid.ItemsSource = null;
                CustomerBillGrid.ItemsSource = customer.bills[customer.bills.Count - 1].Customer_Product;
            }
            else
            {
                MessageBox.Show("Enter Correct Data");
            }


        }

        private void CBForSearchCustmoer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            customer = Market.market.GetCustomer(CBForSearchCustmoer.SelectedItem.ToString());
            casher.CreateBill(customer);
            ValueOfPointlbl.Content = customer.Points.ToString();
        }

        private void ButtonForEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(QuantityTxtBox.Text))
&& (CBForSearchCategory.SelectedItem != null)
&& (CBForSearchProduct.SelectedItem != null) && (CBForSearchCustmoer.SelectedItem != null))
            {
                customer = Market.market.GetCustomer(CBForSearchCustmoer.SelectedItem.ToString());
                Category category = casher.retCat(CBForSearchCategory.SelectedItem.ToString());
                Product product = category.GetProduct(CBForSearchProduct.SelectedItem.ToString());
                ProductNeed productNeed = new ProductNeed();
                productNeed.Product = product;
                productNeed.AmountNeeded = int.Parse(QuantityTxtBox.Text);
                if (!casher.EditProduct(productNeed, customer))
                {
                    MessageBox.Show("Product Not Exist");
                }

                CustomerBillGrid.ItemsSource = null;
                CustomerBillGrid.ItemsSource = customer.bills[customer.bills.Count - 1].Customer_Product;
            }
            else
            {
                MessageBox.Show("Enter Correct Data");
            }
        }

        private void ButtonForDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(QuantityTxtBox.Text))
&& (CBForSearchCategory.SelectedItem != null)
&& (CBForSearchProduct.SelectedItem != null) && (CBForSearchCustmoer.SelectedItem != null))
            {
                customer = Market.market.GetCustomer(CBForSearchCustmoer.SelectedItem.ToString());
                Category category = casher.retCat(CBForSearchCategory.SelectedItem.ToString());
                Product product = category.GetProduct(CBForSearchProduct.SelectedItem.ToString());
                ProductNeed productNeed = new ProductNeed();
                productNeed.Product = product;
                productNeed.AmountNeeded = int.Parse(QuantityTxtBox.Text);

                if (!casher.DeleteProduct(productNeed, customer))
                {
                    MessageBox.Show("Product Not Exist");
                }
                CustomerBillGrid.ItemsSource = null;
                CustomerBillGrid.ItemsSource= customer.bills[customer.bills.Count - 1].Customer_Product;
            }
            else
            {
                MessageBox.Show("Enter Correct Data");
            }
        }

        private void ButtonforPay_Click(object sender, RoutedEventArgs e)
        {
           

if (!(string.IsNullOrEmpty(QuantityTxtBox.Text))
&& (CBForSearchCategory.SelectedItem != null)
&& (CBForSearchProduct.SelectedItem != null) && (CBForSearchCustmoer.SelectedItem != null))
            {
                customer = Market.market.GetCustomer(CBForSearchCustmoer.SelectedItem.ToString());
                casher.pay(customer);
                TotalPriceLabel.Content= customer.bills[customer.bills.Count - 1].TotalPrice.ToString();
                ValueOfPointlbl.Content = customer.Points.ToString();
            }
            else
            {
                MessageBox.Show("Please ,Enter valid Data!");
            }


        }

        private void BtnforPayWithPoints_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(QuantityTxtBox.Text))
&& (CBForSearchCategory.SelectedItem != null)
&& (CBForSearchProduct.SelectedItem != null) && (CBForSearchCustmoer.SelectedItem != null))
            {
                customer = Market.market.GetCustomer(CBForSearchCustmoer.SelectedItem.ToString());
                casher.PayWithPoints(customer);
                TotalPriceLabel.Content = customer.bills[customer.bills.Count - 1].TotalPrice.ToString();
                ValueOfPointlbl.Content = customer.Points.ToString();
            }
            else
            {
                MessageBox.Show("Please Enter Valid Data!");
            }
        }

        private void BTNForPrint_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(QuantityTxtBox.Text))
&& (CBForSearchCategory.SelectedItem != null)
&& (CBForSearchProduct.SelectedItem != null) && (CBForSearchCustmoer.SelectedItem != null))
            {
                Customer_Bill customer_Bill = customer.bills[customer.bills.Count - 1];



                SaveFileDialog save = new SaveFileDialog();



                save.Filter = "PDF (*.pdf)|*.pdf";



                save.FileName = "Customer Bill.pdf";



                bool ErrorMessage = false;
                if (save.ShowDialog() == DialogResult)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = true;
                            MessageBox.Show("Unable to wride data in disk" + ex.Message);
                        }



                    }



                    if (!ErrorMessage)



                    {
                        try
                        {
                            Paragraph paragraph = new Paragraph();
                            paragraph.SpacingBefore = 10;
                            paragraph.SpacingAfter = 10;
                            paragraph.Alignment = Element.ALIGN_LEFT;
                            paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.BLACK);
                            paragraph.Add("***********************************Customer Bill***********************************\n");
                            paragraph.Add("Bill ID: " + customer_Bill.BillId + '\n');
                            paragraph.Add("Customer Name: " + customer_Bill.CustomerName + '\n');
                            paragraph.Add("Customer ID: " + customer_Bill.CustomerId + '\n');
                            paragraph.Add("Casher Name: " + customer_Bill.CashierName + '\n');
                            paragraph.Add("Casher ID: " + customer_Bill.CashierId + '\n');
                            DateTime dateTime = DateTime.Now;
                            paragraph.Add("Created Time: " + dateTime + '\n');
                            foreach (ProductNeed productNeed in customer_Bill.Customer_Product)
                            {
                                paragraph.Add("Product Name: " + productNeed.Product.Name + '\n');
                                paragraph.Add("Product Amount: " + productNeed.AmountNeeded + '\n');



                            }
                            paragraph.Add("***********************************************\n");
                            paragraph.Add("Total Price: " + customer_Bill.TotalPrice + '\n');
                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(paragraph);
                                document.Close();
                                fileStream.Close();
                            }



                            MessageBox.Show("Data Export Successfully", "info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while exporting Data" + ex.Message);
                        }
                    }
                }

                else
                {
                    MessageBox.Show("No Record Found", "Info");
                }



                CustomerBillGrid.ItemsSource = null;
            }
            else
            {
                MessageBox.Show("Please ,Enter Valid Data!");
            }



            TotalPriceLabel.Content= "0";
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
           
            Login login = new Login();
            login.Show();
            this.Close();
        }

     

        private void QuantityTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsNumber(e.Text)==false)
            {
                e.Handled = true;
            }
   
        
        }
        private bool IsNumber(string text)
        {
            int result;
            return int.TryParse(text,out result);
        }

    }


}

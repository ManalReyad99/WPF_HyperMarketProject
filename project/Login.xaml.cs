using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HyperMarket
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
          
Cashier casher = Market.market.GetCasher(TextForUserName.Text);
            if (TextForUserName.Text == "" || PwdTxt.Password.ToString() == "")
            {
                MessageBox.Show("Enter The UserName And Password");
            }
            else
            {
                if (CBSelectRole.SelectedIndex > -1)
                {
                    if (CBSelectRole.Text.ToString() == "Admin")
                    {
                        if (TextForUserName.Text == "Admin" && PwdTxt.Password.ToString() == "Admin")
                        {
                            this.Close();
                            //adminForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("If you are the Admin,Enter The Correct Data");
                        }
                    }
                    else if (CBSelectRole.Text.ToString() == "Casher")
                    {
                        if (TextForUserName.Text == casher.Username.ToString() && PwdTxt.Password.ToString() == casher.Password.ToString())
                        {
                            CashierForm cashierForm = new CashierForm(TextForUserName.Text);
                            cashierForm.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("If you are the Casher,Enter The Correct Data");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select A Role");
                }
            }


        }

        private void ResetLable_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextForUserName.Clear();
            PwdTxt.Clear();
        }
    }
}

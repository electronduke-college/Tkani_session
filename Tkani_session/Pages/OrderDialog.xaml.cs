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

namespace Tkani_session.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderDialog.xaml
    /// </summary>
    public partial class OrderDialog : Window
    {
        private List<TradeDataSet.ProductRow> products;
        public OrderDialog(List<TradeDataSet.ProductRow> products)
        {
            InitializeComponent();
            this.products = products;
            UpdateList();
        }

        private void UpdateList()
        {
            if (products.Count > 0)
            {
                btnMakeOrder.Visibility = Visibility.Visible;
            }
            else
            {
                btnMakeOrder.Visibility = Visibility.Collapsed;
            }
            lvProducts.ItemsSource = products;
            lvProducts.Items.Refresh();
            decimal fullPrice = 0;
            decimal price = 0;

            products.ForEach(e => fullPrice += e.ProductCost);
            products.ForEach(e => price += e.ProductCost - (e.ProductCost / 100 * e.ProductDiscountAmount));
            tbPriceAmount.Text = fullPrice.ToString();
            tbFinalAmount.Text = price.ToString();
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine(products.Count);
            products.Remove(products.Where(ele => ele == ((sender as Button).Parent as StackPanel).DataContext as TradeDataSet.ProductRow).First());
            Console.WriteLine(products.Count);
            UpdateList();
        }

        private void btnMakeOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

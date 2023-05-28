using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tkani_session.TradeDataSetTableAdapters;

namespace Tkani_session.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddProductPage.xaml
    /// </summary>
    public partial class AddProductPage : Page
    {
        private readonly MainWindow mainWindow;
        private readonly ProductTableAdapter productTableAdapter;
        private readonly TradeDataSet.UserRow user;
        public AddProductPage(MainWindow mainWindow, TradeDataSet.UserRow user)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.user = user;
            productTableAdapter = new ProductTableAdapter();
            cbCategory.ItemsSource = productTableAdapter.GetData().Select(e => e.ProductCategory).Distinct().ToList();
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            string name = tbProductName.Text;
            string description = tbProductDescription.Text;
            string manufacture = tbProductManufacturer.Text;
            int count = 0;
            byte? discount = 0;
            decimal price = 0;          
            string error = "";

            if (Int32.TryParse(tbProductCount.Text, out int val))
            {
                count = val;
                if (count <= 0)
                {
                    error += "\nКоличество товара не может быть отрицательным числом!";
                }
            }
            else
            {
                error += "\nКоличество товара должно быть целым числом!";

            }

            if (Byte.TryParse(tbProductCount.Text, out byte valDiscount))
            {
                discount = valDiscount;
                if (count <= 0)
                {
                    error += "\nСкидка не может быть отрицательным числом!";
                }
            }
            else
            {
                error += "\nКоличество товара должно быть целым числом!";

            }

            if (Decimal.TryParse(tbProductPrice.Text, out decimal dec))
            {
                price = dec;
                if (price <= 0)
                {
                    error += "\nЦена не может быть отрицательным числом!";
                }
            }
            else
            {
                error += "\nЦена должна быть целым числом!";
            }

            if (name == "")
            {
                error += "\nНазвание товара - обязательное поле!";
            }

            if (manufacture== "")
            {
                error += "\nПоставщик товара - обязательное поле!";
            }
            if (description == "")
            {
                error += "\nОписание товара - обязательное поле!";
            }
            if (cbCategory.SelectedItem == null)
            {
                error += "\nКатегория товара - обязательное поле!";
            }

            if (error != "")
            {
                MessageBox.Show(error);
            }
            else
            {
                productTableAdapter.InsertQuery(
                    ProductName: name,
                    ProductDescription: description,
                    ProductManufacturer: manufacture,
                    ProductCategory: cbCategory.SelectedItem.ToString(),
                    ProductCost: price,
                    ProductQuantityInStock: count,
                    ProductDiscountAmount: discount 
                    );
                MessageBox.Show("Товар добавлен");
                this.NavigationService.Navigate(new ClientPage(mainWindow, user));
            }
        }

    }
}

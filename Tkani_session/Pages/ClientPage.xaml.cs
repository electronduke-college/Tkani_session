using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>   
    public partial class ClientPage : Page
    {
        private readonly MainWindow mainWindow;
        private readonly ProductTableAdapter productTableAdapter;
        public ClientPage(MainWindow mainWindow, TradeDataSet.UserRow user)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            productTableAdapter = new ProductTableAdapter();
            tbUsername.Text = $"{user.UserSurname} {user.UserName}";
            var data = productTableAdapter.GetData().ToList();

            var listManufactur = data.Select(x => x.ProductManufacturer).Distinct();
            cbManufacturer.ItemsSource = listManufactur;
            ProductPhotoUpdating(data);
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs eArgs)
        {
            FilterAndSearch();
        }

        private void ProductPhotoUpdating(List<TradeDataSet.ProductRow> data)
        {
            tbCount.Text = $"{data.Count} из {productTableAdapter.GetData().Count}";
            data.ForEach(e =>
            {               
                if (e.ProductPhoto == "")
                {
                    e.ProductPhoto = "picture.png";
                }
            });
            data.ForEach(e => e.ProductPhoto = $"../Images/{e.ProductPhoto}");
            dgProducts.ItemsSource = data;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void cbManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSearch();
        }
        private void FilterAndSearch()
        {
            var data = productTableAdapter.GetData().ToList();
            if (tbSearch.Text != "")
            {
                data = data.Where(el => el.ProductName.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
            }

            if (cbManufacturer.SelectedValue != null)
            {
                data = data.Where((el) => el.ProductManufacturer == cbManufacturer.SelectedValue.ToString()).ToList();
            }

            if (cbsort.SelectedValue != null)
            {
                Console.WriteLine(cbsort.SelectedValue);
                Console.WriteLine(cbsort.SelectedIndex);
                if (cbsort.SelectedIndex == 0)
                {
                    data = data.OrderBy(e => e.ProductCost).ToList();
                }
                if (cbsort.SelectedIndex == 1)
                {
                    data = data.OrderByDescending(e => e.ProductCost).ToList();
                }

                
            }


            ProductPhotoUpdating(data);
        }

        private void cbsort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSearch();
        }
    }
}

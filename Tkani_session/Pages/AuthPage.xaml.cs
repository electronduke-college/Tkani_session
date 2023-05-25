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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tkani_session.TradeDataSetTableAdapters;

namespace Tkani_session.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private readonly MainWindow mainWindow;
        UserTableAdapter userTable = new UserTableAdapter();

        public AuthPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text;
            string password = tbPassword.Text;

            var user = userTable.GetData().Where(u=> u.UserLogin == login).Where(u => u.UserPassword == password).FirstOrDefault();

            if(user == null)
            {
                MessageBox.Show("Неправильный логин или пароль");
            }
            else
            {
                this.NavigationService.Navigate(new CLientPage(mainWindow));
            }
        }
    }
}

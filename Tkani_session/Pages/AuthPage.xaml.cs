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
using System.Windows.Threading;
using Tkani_session.TradeDataSetTableAdapters;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;

namespace Tkani_session.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private readonly MainWindow mainWindow;
        UserTableAdapter userTable;

        private DispatcherTimer failedAuthTimer;
        private TimeSpan retryAuthOffset;

        public AuthPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            userTable = new UserTableAdapter();
            tbLogin.Text = "1@gmail.com";
            tbPassword.Text = "LdNyos";
            //tbLogin.Text = "8lf0g@yandex.ru";
            //tbPassword.Text = "2L6KZG";
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text;
            string password = tbPassword.Text;

            var user = userTable.GetData().Where(u => u.UserLogin == login).Where(u => u.UserPassword == password).FirstOrDefault();

            if (user == null)
            {
                MessageBox.Show("Неправильный логин или пароль");
                OnAuthFailed();
            }
            else
            {
                tbLogin.Text = "";
                tbPassword.Text = "";
                this.NavigationService.Navigate(new ClientPage(mainWindow, user));       
            }
        }

        private void OnAuthFailed()
        {
            ChangeUiState(false);
            retryAuthOffset = TimeSpan.FromSeconds(10);
            tbTimer.Text = "Пожалуйста, подождите 10 секунд";
            tbTimer.Visibility = Visibility.Visible;
            failedAuthTimer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, delegate
            {
                OnFailedAuthTimerTick();
            }, Application.Current.Dispatcher);
        }

        private void OnFailedAuthTimerTick()
        {
            var retrySecond = retryAuthOffset.Add(TimeSpan.FromSeconds(-1)).Seconds;
            retryAuthOffset = TimeSpan.FromSeconds(retrySecond);
            if (retrySecond <= 0)
            {
                ChangeUiState(true);
                tbTimer.Visibility = Visibility.Collapsed;
                failedAuthTimer.Stop();
            }
            tbTimer.Text = $"Пожалуйста, подождите {retryAuthOffset.Seconds} сек.";
        }

        private void ChangeUiState(bool state)
        {
            tbLogin.IsEnabled = state;
            tbPassword.IsEnabled = state;
            btnLogin.IsEnabled = state;
        }
    }
}

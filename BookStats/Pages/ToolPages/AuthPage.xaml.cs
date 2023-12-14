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
using BookStats.Windows;
using BookStats.Models;

namespace BookStats.Pages.ToolPages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = new Users();
            try
            {
            currentUser = App.Context.Users.Where(p => (p.Login == tbLogin.Text || p.Email == tbLogin.Text) && p.Password == tbPassword.Password)
                .AsEnumerable()
                .FirstOrDefault(p => (p.Login == tbLogin.Text || p.Email == tbLogin.Text) && p.Password == tbPassword.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (currentUser != null)
            {
                App.CurrentUser = currentUser;
                var currentWindow = Application.Current.Windows
                    .Cast<Window>()
                    .FirstOrDefault(window => window is MainWindow) as MainWindow;

                if (App.CurrentUser != null)
                {
                    currentWindow.stackNormal.Visibility = Visibility.Visible;
                    currentWindow.stackLogOut.Visibility = Visibility.Visible;
                    if (App.CurrentUser.Role == 1)
                    {
                        currentWindow.stackGenres.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        currentWindow.stackGenres.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    currentWindow.stackNormal.Visibility = Visibility.Collapsed;
                    currentWindow.stackLogOut.Visibility = Visibility.Collapsed;
                }

                MessageBox.Show("Прошло удачно!");

                var notificationsList = App.Context.NotificationTable.Where(p => p.Requisitions.UserID == currentUser.ID).ToList();
                if (notificationsList.Count > 0)
                {
                    foreach (var notification in notificationsList)
                    {
                        var dialog = new NotificationWindow(notification);
                        dialog.ShowDialog();
                    }
                }

                NavigationService.Navigate(new Pages.RequisitionsPages.RequisitionsViewPage());

            }
            else
            {
                MessageBox.Show("Пользователь не найден.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            tbLogin.Text = String.Empty;
            var currentWindow = Application.Current.Windows
                .Cast<Window>()
                .FirstOrDefault(window => window is MainWindow) as MainWindow;
            currentWindow.stackGenres.Visibility = Visibility.Collapsed;
            currentWindow.stackGoBack.Visibility = Visibility.Collapsed;
            currentWindow.stackLogOut.Visibility = Visibility.Collapsed;
        }

        private void BtnToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage(null));
        }
    }
}
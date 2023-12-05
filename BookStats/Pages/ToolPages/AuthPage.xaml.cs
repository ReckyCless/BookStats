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
            var currentUser = App.Context.Users.Where(p => (p.Login == tbLogin.Text || p.Email == tbLogin.Text) && p.Password == tbPassword.Password)
                .AsEnumerable()
                .FirstOrDefault(p => (p.Login == tbLogin.Text || p.Email == tbLogin.Text) && p.Password == tbPassword.Password);
            if (currentUser != null)
            {
                App.CurrentUser = currentUser;
                var currentWindow = Application.Current.Windows
                    .Cast<Window>()
                    .FirstOrDefault(window => window is MainWindow) as MainWindow;

                MessageBox.Show("Прошло удачно!");
                NavigationService.Navigate(new Pages.BooksPages.BooksViewPage());

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
            /*            currentWindow.AdminSidePanel.Visibility = Visibility.Collapsed;
                        currentWindow.UserSidePanel.Visibility = Visibility.Collapsed;
                        currentWindow.CabinetMenu.Visibility = Visibility.Collapsed;*/
        }

        private void BtnToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage(null));
        }
    }
}
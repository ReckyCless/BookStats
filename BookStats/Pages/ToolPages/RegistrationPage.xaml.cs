using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using BookStats.Models;

namespace BookStats.Pages.ToolPages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private Users currentElem = new Users();
        public RegistrationPage(Users elemData)
        {
            InitializeComponent();
            if (elemData != null)
            {
                Title = "Профиль. Редактирование";
                currentElem = elemData;
            }

            if (App.CurrentUser != null)
            {
                btnRegister.Content = "Сохранить";
                btnToLogin.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnRegister.Content = "Зарегистрировать";
                btnToLogin.Visibility = Visibility.Visible;
            }

            DataContext = currentElem;
            tbPassword.Password = currentElem.Password;
        }
        // Regexes
        private void TextValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[a-zA-Zа-яА-Я]");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void LoginValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[a-zA-Z@0-9!@#\$%\^\&*\)\(+=._-]");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        private void PasswordValidationTextBox(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");
            if (!regex.IsMatch(tbPassword.Password))
            {
                PasswordValidationErr.Visibility = Visibility.Visible;
                PasswordValidationErr.Text = "Пароль: только латиница, как минимум 8 символов, включая один символ верхнего и нижнего регистра и одну цифру";
                btnRegister.IsEnabled = false;
            }
            else if (regex.IsMatch(tbPassword.Password))
            {
                PasswordValidationErr.Visibility = Visibility.Collapsed;
                PasswordValidationErr.Text = "";
                btnRegister.IsEnabled = true;
            }
        }

        private void PasswordRepeatValidationTextBox(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Password != tbPasswordRepeat.Password)
            {
                PasswordRepeatValidationErr.Visibility = Visibility.Visible;
                PasswordRepeatValidationErr.Text = "Пароли не совпадают";
                btnRegister.IsEnabled = false;
            }
            else
            {
                PasswordRepeatValidationErr.Visibility = Visibility.Collapsed;
                PasswordRepeatValidationErr.Text = "";
                btnRegister.IsEnabled = true;
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Check if textboxes is filled
            StringBuilder err = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentElem.Login))
                err.AppendLine("Укажите логин");
            if (string.IsNullOrWhiteSpace(tbPassword.Password))
                err.AppendLine("Укажите пароль");
            if (string.IsNullOrWhiteSpace(tbPasswordRepeat.Password))
                err.AppendLine("Повторите пароль");
            if (string.IsNullOrWhiteSpace(currentElem.FirstName))
                err.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(currentElem.SecondName))
                err.AppendLine("Укажите фамилию");
            if (string.IsNullOrWhiteSpace(currentElem.Phone))
                err.AppendLine("Укажите номер телефона");

            // Check if login and email are unique
            var emialIsFound = App.Context.Users.FirstOrDefault(p => p.Email == currentElem.Email && p.ID != currentElem.ID);
            if (emialIsFound != null)
                err.AppendLine("Эта почта уже используется");

            var loginIsFound = App.Context.Users.FirstOrDefault(p => p.Login == currentElem.Login && p.ID != currentElem.ID);
            if (loginIsFound != null)
                err.AppendLine("Эта логин уже используется");

            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentElem.Password = tbPassword.Password;

            if (App.CurrentUser == null)
            {
                currentElem.Role = 2; // Grant User 'Client' status (Role = 2)
            }

            if (currentElem.ID == 0)
            {
                currentElem.DateOfAdding = DateTime.Now;
                App.Context.Users.Add(currentElem);
            }

            try
            {
                App.Context.SaveChanges();
                MessageBox.Show("Данные сохранены");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BtnToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }
    }
}
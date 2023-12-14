using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Microsoft.Win32;

namespace BookStats.Pages.GenresPages
{
    /// <summary>
    /// Логика взаимодействия для GenresAddEditPage.xaml
    /// </summary>
    public partial class GenresAddEditPage : Page
    {
        private Genres currentElem = new Genres();

        bool isEditing = false;

        public GenresAddEditPage(Genres elemData)
        {
            InitializeComponent();

            if (elemData != null)
            {
                isEditing = true;
                btnSaveAndNew.Visibility = Visibility.Collapsed;
                Title = "Жанры. Редактирование";
                currentElem = elemData;
            }

            DataContext = currentElem;
        }

        #region Regexes

        private void TextValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[а-яА-ЯёЁa-zA-Z""()0-9''<>.,/\№#«»-]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void TextPastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            Regex regex = new Regex(@"^[а-яА-ЯёЁa-zA-Z""()0-9''<>.,/\№#«»-]+$");
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!regex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        private void ArticleValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9a-zA-Z]");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void ArticlePastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9a-zA-Z]+$");
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!regex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        private void ArticleValidationTextBox(object sender, DataObjectPastingEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9a-zA-Z]");
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!regex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void NumberPastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]");
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!regex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        #endregion Regexes

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Check if fields are filled
            StringBuilder err = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentElem.GenreName))
                err.AppendLine("Выберите название жанра");

            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentElem.GenreName = currentElem.GenreName.Trim();
            currentElem.GenreName = Regex.Replace(currentElem.GenreName, @"\s+", " ");

            if (!isEditing)
            {
                App.Context.Genres.Add(currentElem);
            }

            try
            {
                App.Context.SaveChanges();
                App.Context.SaveChanges();
                MessageBox.Show("Данные сохранены");
                var wnd = Window.GetWindow(this);
                wnd.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                if (ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.ToString());
            }
        }
        private void BtnSaveAndNew_Click(object sender, RoutedEventArgs e)
        {
            // Check if fields are filled
            StringBuilder err = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentElem.GenreName))
                err.AppendLine("Выберите название жанра");


            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentElem.GenreName = currentElem.GenreName.Trim();
            currentElem.GenreName = Regex.Replace(currentElem.GenreName, @"\s+", " ");

            App.Context.Genres.Add(currentElem);

            try
            {
                App.Context.SaveChanges();
                MessageBox.Show("Данные сохранены");

                currentElem = new Genres();
                DataContext = currentElem;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TextValidationTextBox(object sender, DataObjectPastingEventArgs e)
        {

        }
    }
}

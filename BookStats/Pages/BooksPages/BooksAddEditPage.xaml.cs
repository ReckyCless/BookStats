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

namespace BookStats.Pages.BooksPages
{
    /// <summary>
    /// Логика взаимодействия для BooksAddEditPage.xaml
    /// </summary>
    public partial class BooksAddEditPage : Page
    {
        private Books currentElem = new Books();

        bool isEditing = false;
        string agreementNumberBefore = string.Empty;
        public List<BookGenres> bookGenresList = new List<BookGenres>();
        public List<Genres> genresContext = App.Context.Genres.ToList();
        public List<Genres> datagridList = new List<Genres>();
        public List<Genres> genresAvailableList = genresContext;
        public BooksAddEditPage(Books elemData)
        {
            InitializeComponent();

            if (elemData != null)
            {
                isEditing = true;
                btnSaveAndNew.Visibility = Visibility.Collapsed;
                Title = "Книги. Редактирование";
                currentElem = elemData;
                agreementNumberBefore = elemData.AgreementNumber;
                txtAgreementNumber.IsEnabled = false;

                bookGenresList = App.Context.BookGenres.Where(p => p.Book.Article == currentElem.Article).ToList();
                foreach (var item in bookGenresList)
                    datagridList.Add(item.Genres);
            }
            else
            {
                currentElem.PublicationDate = DateTime.Today;
            }
            DataContext = currentElem;


            dpPublicationDate.DisplayDateStart = new DateTime(1950, 1, 1);
            dpPublicationDate.DisplayDateEnd = DateTime.Today;

            cmbGenres.ItemsSource = App.Context.Genres.ToList();
            CheckSpecialities();

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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (datagridGenres.SelectedItems.Count > 0)
            {
                var elemsToDelete = datagridGenres.SelectedItems.Cast<Genres>().ToList();
                if (MessageBox.Show($"Вы точно хотите удалить следующие {elemsToDelete.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    foreach (var item in elemsToDelete)
                    {
                        datagridList.Remove(item);
                        foreach (var elem in bookGenresList)
                        {
                            if (elem.Specialties == item)
                                App.Context.BookGenres.Remove(elem);
                        }
                    }
                    try
                    {
                        MessageBox.Show("Данные удалены!");
                        CheckSpecialities();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
        private void CheckSpecialities()
        {
            genresAvailableList = genresContext;
            if (datagridList.Count > 0)
            {
                foreach (var item in datagridList)
                {
                    if (genresAvailableList.Contains(item))
                    {
                        genresAvailableList.Remove(item);
                    }
                }
            }
            if (cmbGenres.SelectedItem != null)
            {
                var genreSelected = (Genres)cmbGenres.SelectedItem;
                if (!datagridList.Any(p => p.BookGenres == genreSelected.BookGenres))
                {
                    datagridList.Add(genreSelected);
                }
            }

            cmbGenres.ItemsSource = genresAvailableList;
            cmbGenres.SelectedIndex = -1;

            datagridGenres.ItemsSource = datagridList;
            datagridGenres.Items.Refresh();
        }
        private void cmbSpecialities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckSpecialities();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Check if fields are filled
            StringBuilder err = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentElem.Article))
                err.AppendLine("Укажите артикул");
            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentElem.Title = currentElem.Title.Trim();
            currentElem.Title = Regex.Replace(currentElem.Title, @"\s+", " ");


            if (currentElem.Remark != null)
            {
                currentElem.Remark = currentElem.Remark.Trim();
                currentElem.Remark = Regex.Replace(currentElem.Remark, @"\s+", " ");
            }

            if (!isEditing)
            {
                App.Context.Books.Add(currentElem);
            }

            try
            {
                App.Context.SaveChanges();
                foreach (var item in datagridList)
                {
                    if (!agreementSpecialityList.Any(p => p.Genres == item))
                    {
                        AgreementSpeciality agreementspeciality = new AgreementSpeciality();
                        agreementspeciality.SpecialityID = item.SpecialityNumber;
                        agreementspeciality.Specialties = item;
                        agreementspeciality.AgreementID = currentElem.AgreementNumber;
                        agreementspeciality.DateOfAdding = null;
                        App.Context.AgreementSpeciality.Add(agreementspeciality);
                    }
                }
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
            if (string.IsNullOrWhiteSpace(currentElem.Article))
                err.AppendLine("Укажите артикул");


            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentElem.Title = currentElem.Title.Trim();
            currentElem.Title = Regex.Replace(currentElem.Title, @"\s+", " ");


            if (currentElem.Remark != null)
            {
                currentElem.Remark = currentElem.Remark.Trim();
                currentElem.Remark = Regex.Replace(currentElem.Remark, @"\s+", " ");
            }

            if (!string.IsNullOrWhiteSpace(currentElem.Remark))
            {
                currentElem.Remark = currentElem.Remark.Trim();
                currentElem.Remark = Regex.Replace(currentElem.Remark, @"\s+", " ");
            }


            App.Context.Books.Add(currentElem);

            try
            {
                App.Context.SaveChanges();
                MessageBox.Show("Данные сохранены");

                foreach (var item in datagridList)
                {
                    BookGenres bookgenres = new BookGenres();
                    bookgenres.GenreID = item.ID;
                    bookgenres.BookID = currentElem.Article;
                    App.Context.BookGenres.Add(bookgenres);
                }
                datagridList = new List<Genres>();
                genresAvailableList = genresContext;
                datagridGenres.ItemsSource = datagridList;
                datagridGenres.Items.Refresh();
                cmbGenres.ItemsSource = genresAvailableList;
                cmbGenres.Items.Refresh();
                currentElem = new Books();
                currentElem.PublicationDate = DateTime.Today;
                DataContext = currentElem;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}


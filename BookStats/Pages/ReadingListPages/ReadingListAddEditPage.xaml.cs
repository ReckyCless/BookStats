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


namespace BookStats.Pages.ReadingListPages
{
    /// <summary>
    /// Логика взаимодействия для ReadingListAddEditPage.xaml
    /// </summary>
    public partial class ReadingListAddEditPage : Page
    {
        private ReadingLists currentElem = new ReadingLists();

        public List<Books> booksContext = App.Context.Books.ToList();

        public List<ReadingListBooks> readingListbooksList = new List<ReadingListBooks>();
        public List<ReadingListBooks> datagridList = new List<ReadingListBooks>();

        public List<Books> booksAvailableList = App.Context.Books.ToList();
        public ReadingListAddEditPage()
        {
            InitializeComponent();

            if (App.CurrentUser != null && App.CurrentUser.ReadingLists != null)
            {
                Debug.WriteLine("Оно туть");
                Title = "Дневник. Редактирование";

                var currentUser = App.CurrentUser;

                if (App.Context.ReadingLists.Any(p => p.UserID == currentUser.ID))
                    currentElem = App.Context.ReadingLists.FirstOrDefault();

                readingListbooksList = App.Context.ReadingListBooks.Where(p => p.ReadingListID == currentElem.ID).ToList();
                foreach (var item in readingListbooksList)
                {
                    datagridList.Add(item);
                }
            }

            var genre = new Genres();
            genre.GenreName = "Без фильтрации";
            var genres = new List<Genres>();
            genres.Add(genre);
            genres.AddRange(App.Context.Genres.ToList());
            cmbGenres.ItemsSource = genres;
            cmbGenres.SelectedIndex = 0;

            cmbBooks.ItemsSource = booksContext;

            CheckBooks();
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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (datagridManagers.SelectedItems.Count > 0)
            {
                var elemsToDelete = datagridManagers.SelectedItems.Cast<ReadingListBooks>().ToList();
                if (MessageBox.Show($"Вы точно хотите удалить следующие {elemsToDelete.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    foreach (var item in elemsToDelete)
                    {
                        datagridList.Remove(item);
                        if (readingListbooksList.Contains(item))
                        {
                            App.Context.ReadingListBooks.Remove(item);
                            readingListbooksList.Remove(item);
                        }
                    }
                    try
                    {
                        datagridManagers.Items.Refresh();
                        MessageBox.Show("Данные удалены!");
                        CheckBooks();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
        private void CheckBooks()
        {
            booksAvailableList = booksContext;
            if (datagridList.Count > 0)
            {
                foreach (var item in datagridList)
                {
                    if (booksAvailableList.Any(p => p.Article == item.BookID))
                    {
                        booksAvailableList.Remove(item.Books);
                    }
                }
            }

            if (cmbGenres.SelectedIndex > 0)
            {
                booksAvailableList = booksAvailableList.Where(x => x.BookGenres.Any(p => p.Genres == (Genres)cmbGenres.SelectedItem)).ToList();
            }
            if (txtSearchBooks.Text.Length > 0)
            {
                booksAvailableList = booksAvailableList.Where(x => x.Name.ToLower().Contains(txtSearchBooks.Text.ToLower()) ||
                    x.Author.ToLower().Contains(txtSearchBooks.Text.ToLower())
                    ).ToList();
            }


            if (cmbBooks.SelectedItem != null)
            {
                StringBuilder err = new StringBuilder();
                if (string.IsNullOrWhiteSpace(txtNumberOfPage.Text))
                    err.AppendLine("Добавьте номер главы/страницы");

                if (err.Length > 0)
                {
                    MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    string RemarkInText = string.Empty;
                    if (!string.IsNullOrEmpty(txtRemark.Text))
                    {
                        RemarkInText = txtRemark.Text;
                        RemarkInText = RemarkInText.Trim();
                        RemarkInText = Regex.Replace(RemarkInText, @"\s+", " ");
                    }
                    var bookSelected = new ReadingListBooks();
                    bookSelected.Books = (Books)cmbBooks.SelectedItem;
                    bookSelected.ReadingLists = currentElem;
                    bookSelected.ChapterPageNumber = Convert.ToInt32(txtNumberOfPage.Text);
                    bookSelected.Remark = RemarkInText;

                    if (!datagridList.Any(p => p == bookSelected))
                    {
                        txtRemark.Text = "";
                        txtNumberOfPage.Text = "0";
                        booksAvailableList.Remove(bookSelected.Books);
                        datagridList.Add(bookSelected);
                    }
                }
            }

            cmbBooks.ItemsSource = booksAvailableList;
            cmbBooks.Items.Refresh();
            cmbBooks.SelectedIndex = -1;

            datagridManagers.ItemsSource = datagridList;
            datagridManagers.Items.Refresh();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Check if fields are filled
            StringBuilder err = new StringBuilder();
            if (datagridList.Count < 1)
                err.AppendLine("Выберите хотя бы одну книгу!!!");

            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (!App.Context.ReadingLists.Any(p => p.UserID == App.CurrentUser.ID))
            {
                currentElem.Users = App.CurrentUser;
                App.Context.ReadingLists.Add(currentElem);
            }

            try
            {
                App.Context.SaveChanges();
                foreach (var item in datagridList)
                {
                    if (!readingListbooksList.Any(p => p == item))
                    {
                        App.Context.ReadingListBooks.Add(item);
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

        private void cmbBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckBooks();
        }

        private void cmbGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckBooks();
        }
        private void booksSearch(List<Books> listBooks)
        {

            cmbBooks.ItemsSource = listBooks;
        }

        private void txtSearchBooks_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckBooks();
        }

        private void NumberValidationTextBox(object sender, DataObjectPastingEventArgs e)
        {

        }
    }
}

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

namespace BookStats.Pages.BooksPages
{
    /// <summary>
    /// Логика взаимодействия для BooksAddEditPage.xaml
    /// </summary>
    public partial class BooksAddEditPage : Page
    {
        private Books currentElem = new Books();

        bool isEditing = false;

        string pathToImage = string.Empty;
        string pathToImageShort = string.Empty;
        string tempPathImage = string.Empty;
        string directory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources")); // Directory

        BitmapImage imagePreviewBit = new BitmapImage();

        public List<BookGenres> bookGenresList = new List<BookGenres>();
        public List<Genres> genresContext = App.Context.Genres.ToList();
        public List<Genres> datagridList = new List<Genres>();
        public List<Genres> genresAvailableList = App.Context.Genres.ToList();
        public BooksAddEditPage(Books elemData)
        {
            InitializeComponent();

            if (elemData != null)
            {
                isEditing = true;
                btnSaveAndNew.Visibility = Visibility.Collapsed;
                Title = "Книги. Редактирование";
                currentElem = elemData;

                txtArticle.IsEnabled = false;

                bookGenresList = App.Context.BookGenres.Where(p => p.Books.Article == currentElem.Article).ToList();
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
            CheckGenres();


            if (currentElem.Image != null && currentElem.Image != "")
            {
                pathToImage = directory + @"/Images/" + currentElem.Image;

                pathToImageShort = currentElem.Image;
                ImagePreview.Source = new BitmapImage(new Uri(pathToImage, UriKind.Absolute));
            }
            else
            {
                pathToImage = System.IO.Path.Combine(directory, @"default.png");
                ImagePreview.Source = new BitmapImage(new Uri(pathToImage, UriKind.Absolute));
            }
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
                            if (elem.Genres == item)
                                App.Context.BookGenres.Remove(elem);
                        }
                    }
                    try
                    {
                        MessageBox.Show("Данные удалены!");
                        CheckGenres();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
        private void CheckGenres()
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
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tempPathImage))
                    File.Copy(tempPathImage, pathToImage, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            currentElem.Image = pathToImageShort;

            // Check if fields are filled
            StringBuilder err = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentElem.Article))
                err.AppendLine("Укажите артикул");
            else if (!isEditing && App.Context.Books.Any(p => p.Article == currentElem.Article))
                err.AppendLine("Артикль не может повторяться");
            if (string.IsNullOrWhiteSpace(currentElem.Name))
                err.AppendLine("Укажите название");
            if (string.IsNullOrWhiteSpace(currentElem.Author))
                err.AppendLine("Укажите автора");

            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentElem.Name = currentElem.Name.Trim();
            currentElem.Name = Regex.Replace(currentElem.Name, @"\s+", " ");


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
                    if (!bookGenresList.Any(p => p.Genres == item))
                    {
                        BookGenres agreementspeciality = new BookGenres();
                        agreementspeciality.IDGenre = item.ID;
                        agreementspeciality.Genres = item;
                        agreementspeciality.IDBook = currentElem.Article;
                        App.Context.BookGenres.Add(agreementspeciality);
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
            try
            {
                if (!string.IsNullOrEmpty(tempPathImage))
                    File.Copy(tempPathImage, pathToImage, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            currentElem.Image = pathToImageShort;
            // Check if fields are filled
            StringBuilder err = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentElem.Article))
                err.AppendLine("Укажите артикул");
            else if (!isEditing && App.Context.Books.Any(p => p.Article == currentElem.Article))
                err.AppendLine("Артикль не может повторяться");
            if (string.IsNullOrWhiteSpace(currentElem.Name))
                err.AppendLine("Укажите название");
            if (string.IsNullOrWhiteSpace(currentElem.Author))
                err.AppendLine("Укажите автора");


            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentElem.Name = currentElem.Name.Trim();
            currentElem.Name = Regex.Replace(currentElem.Name, @"\s+", " ");


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
                    bookgenres.IDGenre = item.ID;
                    bookgenres.Genres = item;
                    bookgenres.IDBook = currentElem.Article;
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

        private void cmbGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckGenres();
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Image | *.png; *.jpg; *.jpeg" };
            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.SafeFileName;
                tempPathImage = ofd.FileName;
                pathToImage = directory + @"\Images\" + filename;
                if (isEditing)
                {
                    if (pathToImageShort == filename)
                    {
                        filename = "copy_" + filename;
                        pathToImage = directory + @"\Images\" + filename;
                        pathToImageShort = filename;
                    }
                    else
                    {
                        pathToImageShort = filename;
                    }
                }
                else
                {
                    pathToImageShort = filename;
                }

                try
                {
                    ImagePreview.Source = new BitmapImage(new Uri(tempPathImage, UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }
}


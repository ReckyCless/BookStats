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

namespace BookStats.Pages.RequisitionsPages
{
    /// <summary>
    /// Логика взаимодействия для RequisitionsAddEditPage.xaml
    /// </summary>
    public partial class RequisitionsAddEditPage : Page
    {
        private Requisitions currentElem = new Requisitions();

        bool isEditing = false;

        public List<Books> booksContext = App.Context.Books.ToList();

        public List<RequisitionManagers> requisitionmanagersList = new List<RequisitionManagers>();
        public List<Users> managersContext = App.Context.Users.Where(p => p.Role == 3).ToList();
        public List<RequisitionManagers> datagridList = new List<RequisitionManagers>();
        public List<Users> managersAvailableList = App.Context.Users.Where(p => p.Role == 3).ToList();
        public RequisitionsAddEditPage(Requisitions elemData)
        {
            InitializeComponent();

            if (elemData != null)
            {
                isEditing = true;
                btnSaveAndNew.Visibility = Visibility.Collapsed;
                Title = "Заявки. Редактирование";
                currentElem = elemData;

                requisitionmanagersList = App.Context.RequisitionManagers.Where(p => p.RequisitionID == currentElem.ID).ToList();
                foreach (var item in requisitionmanagersList)
                {
                    var manager = new RequisitionManagers();
                    manager.Requisitions = item.Requisitions;
                    manager.Users = item.Users;
                    manager.DateOfAdding = item.DateOfAdding;
                    datagridList.Add(manager);
                }

                if (App.CurrentUser != null)
                    currentElem.Users = App.CurrentUser;
                currentElem.BookStatuses = App.Context.BookStatuses.FirstOrDefault(p => p.ID == 1);            
            }
            else
            {
                if (App.CurrentUser != null && App.CurrentUser.Role == 3)
                {
                    var requisitionmanagers = new RequisitionManagers();
                    requisitionmanagers.Users = App.CurrentUser;
                    requisitionmanagers.Requisitions = currentElem;
                    requisitionmanagers.DateOfAdding = DateTime.Now;
                    App.Context.RequisitionManagers.Add(requisitionmanagers);
                }
            }
            DataContext = currentElem;

            var currentUser = new Users();
            if (App.CurrentUser != null)
            {
                currentUser = App.CurrentUser;
                if (currentUser.Role == 1 || currentUser.Role == 3)
                {
                    stackControl.Visibility = Visibility.Visible;
                    cmbManagers.IsEnabled = false;
                    if (currentUser.Role == 1)
                    {
                        cmbManagers.IsEnabled = true;
                    }
                }
                else
                {
                    cmbManagers.IsEnabled = false;
                    stackControl.Visibility = Visibility.Collapsed;
                }
            }

            var genre = new Genres();
            genre.GenreName = "Без фильтрации";
            var genres = App.Context.Genres.ToList();
            genres.Add(genre);
            genres.AddRange(genres);
            cmbGenres.ItemsSource = genres;
            cmbGenres.SelectedIndex = 0;

            cmbBooks.ItemsSource = booksContext;

            cmbManagers.ItemsSource = managersContext;
            CheckManagers();

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
            if (datagridManagers.SelectedItems.Count > 0 && App.CurrentUser.Role == 1)
            {
                var elemsToDelete = datagridManagers.SelectedItems.Cast<RequisitionManagers>().ToList();
                if (MessageBox.Show($"Вы точно хотите удалить следующие {elemsToDelete.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    foreach (var item in elemsToDelete)
                    {
                        datagridList.Remove(item);
                        if (requisitionmanagersList.Contains(item))
                        {
                            App.Context.RequisitionManagers.Remove(item);
                            requisitionmanagersList.Remove(item);
                        }
                    }
                    try
                    {
                        MessageBox.Show("Данные удалены!");
                        CheckManagers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
        private void CheckManagers()
        {
            managersAvailableList = managersContext;
            if (datagridList.Count > 0)
            {
                foreach (var item in datagridList)
                {
                    if (managersAvailableList.Contains(item.Users))
                    {
                        managersAvailableList.Remove(item.Users);
                    }
                }
            }
            if (cmbGenres.SelectedItem != null)
            {
                var managerSelected = new RequisitionManagers();
                managerSelected.Users = (Users)cmbManagers.SelectedItem;
                managerSelected.Requisitions = currentElem;
                managerSelected.DateOfAdding = DateTime.Now;
                if (!datagridList.Any(p => p == managerSelected))
                {
                    datagridList.Add(managerSelected);
                }
            }

            cmbGenres.ItemsSource = managersAvailableList;
            cmbGenres.SelectedIndex = -1;

            datagridManagers.ItemsSource = datagridList;
            datagridManagers.Items.Refresh();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Check if fields are filled
            StringBuilder err = new StringBuilder();
            if (cmbBooks.SelectedItem == null)
                err.AppendLine("Выберите книгу!!!");

            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (!isEditing)
            {
                App.Context.Requisitions.Add(currentElem);
            }

            try
            {
                App.Context.SaveChanges();
                foreach (var item in datagridList)
                {
                    if (!requisitionmanagersList.Any(p => p == item))
                    {
                        App.Context.RequisitionManagers.Add(item);
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
            if (cmbBooks.SelectedItem == null)
                err.AppendLine("Выберите книгу!!!");


            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            App.Context.Requisitions.Add(currentElem);

            try
            {
                App.Context.SaveChanges();
                MessageBox.Show("Данные сохранены");

                foreach (var item in datagridList)
                {
                    App.Context.RequisitionManagers.Add(item);
                }

                datagridList = new List<RequisitionManagers>();
                managersAvailableList = managersContext;
                datagridManagers.ItemsSource = datagridList;
                datagridManagers.Items.Refresh();
                cmbManagers.ItemsSource = managersAvailableList;
                cmbManagers.Items.Refresh();
                currentElem = new Requisitions();
                if (App.CurrentUser != null)
                {
                    currentElem.Users = App.CurrentUser;
                }
                DataContext = currentElem;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void cmbManagres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckManagers();
        }

        private void cmbGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            booksSearch();
        }
        private void booksSearch()
        {
            var listBooks = booksContext;
            if (cmbGenres.SelectedIndex > 0)
            {
                listBooks = listBooks.Where(x => x.BookGenres.Any(p => p.Genres == (Genres)cmbGenres.SelectedItem)).ToList();
            }
            if (txtSearchBooks.Text.Length > 0)
            {
                listBooks = listBooks.Where(x => x.Name.ToLower().Contains(txtSearchBooks.Text.ToLower()) ||
                    x.Author.ToLower().Contains(txtSearchBooks.Text.ToLower())
                    ).ToList();
            }
            cmbBooks.ItemsSource = listBooks;
        }

        private void txtSearchBooks_TextChanged(object sender, TextChangedEventArgs e)
        {
            booksSearch();
        }
    }
}
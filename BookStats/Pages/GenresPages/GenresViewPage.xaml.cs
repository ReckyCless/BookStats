﻿using BookStats.Models;
using BookStats.Windows;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace BookStats.Pages.GenresPages
{
    /// <summary>
    /// Логика взаимодействия для GenresViewPage.xaml
    /// </summary>
    public partial class GenresViewPage : Page
    {
        List<Genres> datagridSourceList = new List<Genres>();
        private int PagesCount;
        private int NumberOfPage = 0;
        private int maxItemShow = 5;

        public GenresViewPage()
        {
            InitializeComponent();

            cmbSort.SelectedIndex = 0;

            UpdateDataGrid();
        }
        //#region Update database on Events
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataGrid();
        }
        private void CBoxFilterBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow windowAddEdit = new AddEditWindow();
            windowAddEdit.frameAddEdit.Navigate(new GenresAddEditPage(null));
            windowAddEdit.Closed += (s, EventArgs) =>
            {
                UpdateDataGrid();
            };
            windowAddEdit.ShowDialog();

        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LViewMain.SelectedItems.Count > 0)
            {
                var elemsToDelete = LViewMain.SelectedItems.Cast<Genres>().ToList();
                if (MessageBox.Show($"Вы точно хотите удалить следующие {elemsToDelete.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        foreach (var elem in elemsToDelete)
                        {
                            App.Context.Genres.Remove(elem);
                        }
                        App.Context.SaveChanges();
                        MessageBox.Show("Данные удалены!");
                        UpdateDataGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        public void RejectChanges()
        {
            foreach (var entry in App.Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.ToString());
                RejectChanges();
            }
        }

        //DataGrid Fill 
        private void UpdateDataGrid()
        {
            datagridSourceList = App.Context.Genres.ToList();

            //Sorting
            switch (cmbSort.SelectedIndex)
            {
                case 1:
                    datagridSourceList = datagridSourceList.OrderBy(p => p.GenreName).ToList();
                    break;
                case 2:
                    datagridSourceList = datagridSourceList.OrderByDescending(p => p.GenreName).ToList();
                    break;
                default:
                    datagridSourceList = datagridSourceList.OrderBy(p => p.ID).ToList();
                    break;
            }

            //Search
            datagridSourceList = datagridSourceList.Where(p => p.GenreName.ToLower().Contains(txtSearch.Text.ToLower())
            ).ToList();

            //Items Counter
            tbItemCounter.Text = datagridSourceList.Count.ToString() + " из " + App.Context.Genres.Count().ToString();

            //Pages Counter
            if (datagridSourceList.Count % maxItemShow == 0)
            {
                PagesCount = datagridSourceList.Count / maxItemShow;
            }
            else
            {
                PagesCount = (datagridSourceList.Count / maxItemShow) + 1;
            }
            blockPagingControls.Visibility = Visibility.Visible;

            tbPageCounter.Text = $"Страница {NumberOfPage + 1} из {PagesCount}";

            //Empty DataGrid Alert
            if (PagesCount < 1)
            {
                blockPagingControls.Visibility = Visibility.Collapsed;
            }


            LViewMain.ItemsSource = datagridSourceList.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();
            CheckPages();

            //Empty DataGrid Alert
            if (datagridSourceList.Count < 1)
                tbItemCounter.Text = "Ничего не найдено. Измените фильтры.";
        }

        #region Pagination controls
        private void PaginationMove()
        {
            LViewMain.ItemsSource = datagridSourceList.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();
            tbPageCounter.Text = $"Страница {NumberOfPage + 1} из {PagesCount}";
            CheckPages();
        }
        private void CheckPages()
        {
            txtCurrentPage.Text = NumberOfPage + 1 + "";
            // Previous Pages Check
            if (NumberOfPage > 0)
            {
                btnPagingPrevious.IsEnabled = true;
                btnPagingStart.IsEnabled = true;
                btnPagingCurrentMinus1.Content = NumberOfPage;
                btnPagingCurrentMinus1.Visibility = Visibility.Visible;
                btnPagingCurrentMinus2.Visibility = Visibility.Collapsed;
                btnPagingCurrentMinus3.Visibility = Visibility.Collapsed;
                if (NumberOfPage > 1)
                {
                    btnPagingCurrentMinus2.Content = NumberOfPage - 1;
                    btnPagingCurrentMinus2.Visibility = Visibility.Visible;
                    if (NumberOfPage > 2)
                    {
                        btnPagingCurrentMinus3.Content = NumberOfPage - 2;
                        btnPagingCurrentMinus3.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                btnPagingCurrentMinus1.Visibility = Visibility.Collapsed;
                btnPagingCurrentMinus2.Visibility = Visibility.Collapsed;
                btnPagingCurrentMinus3.Visibility = Visibility.Collapsed;
                btnPagingPrevious.IsEnabled = false;
                btnPagingStart.IsEnabled = false;
            }

            // Next Pages Check
            if (NumberOfPage < PagesCount - 1)
            {
                btnPagingNext.IsEnabled = true;
                btnPagingEnd.IsEnabled = true;
                btnPagingCurrentPlus1.Content = NumberOfPage + 2;
                btnPagingCurrentPlus1.Visibility = Visibility.Visible;
                btnPagingCurrentPlus2.Visibility = Visibility.Collapsed;
                btnPagingCurrentPlus3.Visibility = Visibility.Collapsed;
                if (NumberOfPage < PagesCount - 2)
                {
                    btnPagingCurrentPlus2.Content = NumberOfPage + 3;
                    btnPagingCurrentPlus2.Visibility = Visibility.Visible;
                    if (NumberOfPage < PagesCount - 3)
                    {
                        btnPagingCurrentPlus3.Content = NumberOfPage + 4;
                        btnPagingCurrentPlus3.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                btnPagingCurrentPlus1.Visibility = Visibility.Collapsed;
                btnPagingCurrentPlus2.Visibility = Visibility.Collapsed;
                btnPagingCurrentPlus3.Visibility = Visibility.Collapsed;
                btnPagingNext.IsEnabled = false;
                btnPagingEnd.IsEnabled = false;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void PaginationButtonStart_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPage = 0;
            PaginationMove();
        }

        private void PaginationButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPage--;
            PaginationMove();
        }

        private void PaginationButtonCurrentMinus3_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPage -= 3;
            PaginationMove();
        }

        private void PaginationButtonCurrentMinus2_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPage -= 2;
            PaginationMove();
        }

        private void PaginationButtonCurrentMinus1_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPage--;
            PaginationMove();
        }

        private void PaginationButtonCurrentPlus1_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPage++;
            PaginationMove();
        }

        private void PaginationButtonCurrentPlus2_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPage += 2;
            PaginationMove();
        }

        private void PaginationButtonCurrentPlus3_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPage += 3;
            PaginationMove();
        }

        private void PaginationButtonNext_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPage++;
            PaginationMove();
        }

        private void PaginationButtonEnd_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPage = PagesCount - 1;
            PaginationMove();
        }

        private void txtCurrentPage_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!(txtCurrentPage.Text == "" || txtCurrentPage.Text == null))
            {
                int txtValue = Convert.ToInt32(txtCurrentPage.Text);
                txtValue--;
                if (txtValue >= 0 && txtValue < PagesCount)
                {
                    NumberOfPage = txtValue;
                    UpdateDataGrid();
                }
            }
        }
        #endregion Pagination Controls

        // Delete filters
        private void deleteFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            cmbSort.SelectedIndex = 0;
            UpdateDataGrid();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddEditWindow windowAddEdit = new AddEditWindow();
            windowAddEdit.frameAddEdit.Navigate(new GenresAddEditPage(LViewMain.SelectedItem as Genres));
            windowAddEdit.Closed += (s, EventArgs) =>
            {
                UpdateDataGrid();
            };
            windowAddEdit.ShowDialog();
        }

        private void CBoxSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
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
using BookStats.Pages.ToolPages;

namespace BookStats.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frameMain.Navigate(new Pages.ToolPages.AuthPage());
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
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите вернуться?\nНесохраненные данные могут быть утеряны",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                RejectChanges();
                frameMain.GoBack();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void FrameMain_ContentRendered(object sender, EventArgs e)
        {
            if (frameMain.CanGoBack)
            {
                if (frameMain.Content.ToString() != "BookStats.Pages.ToolPages.AuthPage")
                {
                    stackGoBack.Visibility = Visibility.Visible;
                }
            }
            else
            {
                stackGoBack.Visibility = Visibility.Collapsed;
            }
            if (App.CurrentUser != null)
            {
                stackNormal.Visibility = Visibility.Visible;
                stackLogOut.Visibility = Visibility.Visible;
                if (App.CurrentUser.Role == 1)
                {
                    stackGenres.Visibility = Visibility.Visible;
                }
                else
                {
                    stackGenres.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                stackNormal.Visibility = Visibility.Collapsed;
                stackLogOut.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnRequisitions_Click(object sender, RoutedEventArgs e)
        {
            if (frameMain.Content.ToString() != "BookStats.Pages.RequisitionsPages.RequisitionsViewPage")
            {
                frameMain.Navigate(new Pages.RequisitionsPages.RequisitionsViewPage());
            }
        }

        private void btnBooks_Click(object sender, RoutedEventArgs e)
        {
            if (frameMain.Content.ToString() != "BookStats.Pages.BooksPages.BooksViewPage")
            {
                frameMain.Navigate(new Pages.BooksPages.BooksViewPage());
            }
        }

        private void btnGenress_Click(object sender, RoutedEventArgs e)
        {
            if (frameMain.Content.ToString() != "BookStats.Pages.GenresPages.GenresViewPage")
            {
                frameMain.Navigate(new Pages.GenresPages.GenresViewPage());
            }
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            RejectChanges();
            frameMain.Navigate(new Pages.ToolPages.AuthPage());
        }

        private void btnReadingList_Click(object sender, RoutedEventArgs e)
        {
            if (frameMain.Content.ToString() != "BookStats.Pages.ReadingListPages.ReadingListViewPage")
            {
                frameMain.Navigate(new Pages.ReadingListPages.ReadingListViewPage());
            }
        }
    }
}

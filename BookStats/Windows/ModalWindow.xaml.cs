﻿using System;
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
using System.Windows.Shapes;
using BookStats.Models;

namespace BookStats.Windows
{
    /// <summary>
    /// Логика взаимодействия для ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : Window
    {
        List<Requisitions> ItemsList = new List<Requisitions>(); 
        public ModalWindow(List<Requisitions> requisitionsList)
        {
            InitializeComponent();
            cmbWorksheetSelection.ItemsSource = App.Context.BookStatuses.ToList();
            ItemsList = requisitionsList;
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите выбрать этот лист?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                try
                {
                    if (cmbWorksheetSelection.SelectedItem != null)
                    {
                        if (App.CurrentUser != null)
                        {
                            foreach (var elem in ItemsList)
                            {
                                var manager = new RequisitionManagers();
                                manager.Requisitions = elem;
                                manager.RequisitionID = elem.ID;
                                manager.Users = App.CurrentUser;
                                manager.ManagerID = App.CurrentUser.ID;
                                manager.DateOfAdding = DateTime.Now;

                                if (!App.Context.RequisitionManagers.Any(p => p.ManagerID == manager.ManagerID && p.RequisitionID == manager.RequisitionID))
                                {
                                    App.Context.RequisitionManagers.Add(manager);
                                }

                                var notification = new NotificationTable();
                                notification.Requisitions = elem;
                                notification.RequisitionID = elem.ID;
                                notification.Users = App.CurrentUser;
                                notification.UserID = App.CurrentUser.ID;
                                notification.IsChecked = false;

                                if (!App.Context.NotificationTable.Any(p => p.UserID == notification.UserID && p.RequisitionID == notification.RequisitionID))
                                {
                                    App.Context.NotificationTable.Add(notification);
                                }

                                elem.BookStatuses = (BookStatuses)cmbWorksheetSelection.SelectedItem;
                            }
                            App.Context.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    if (ex.InnerException != null)
                        MessageBox.Show(ex.InnerException.ToString());
                }
                DialogResult = true;
                
            }
            else
                DialogResult = false;
        }
        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти? Процесс извлечения данных не начнётся", "Внимание",
    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DialogResult = false;
                Close();
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти? Процесс извлечения данных не начнётся", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DialogResult = false;
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
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
using System.Windows.Shapes;
using BookStats.Models;
using QRCoder;
using QRCoder.Xaml;

namespace BookStats.Windows
{
    /// <summary>
    /// Логика взаимодействия для NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        NotificationTable notificationTable = new NotificationTable();
        public NotificationWindow(NotificationTable notificationTableGet)
        {
            InitializeComponent();
            notificationTable = notificationTableGet;

            txtMessage.Text = $"Менеджер - {notificationTable.Users.FullName}" +
                $"\nизменил статус вашего заказа: {notificationTable.RequisitionID}" +
                $"\nна {notificationTable.Requisitions.BookStatuses.StatusName}" +
                $"\n" +
                $"\nОцените его работу через QR код";
            QRCodeGenerate("https://imgur.com/a/ve321oG");
        }

        private void QRCodeGenerate(string link)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.H);
            XamlQRCode qrCode = new XamlQRCode(qrCodeData);
            DrawingImage qrCodeAsXaml = qrCode.GetGraphic(20);
            qrImage.Source = qrCodeAsXaml;
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Context.NotificationTable.Remove(notificationTable);
            App.Context.SaveChanges();
            MessageBox.Show("Спасибо!");
            Close();
        }
        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show("Уведомление будет показано снова. Что бы больше его не видеть нажмите на кнопку Подтвердить", "Внимание",
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
            if (MessageBox.Show("Уведомление будет показано снова. Что бы больше его не видеть нажмите на кнопку Подтвердить", "Внимание",
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
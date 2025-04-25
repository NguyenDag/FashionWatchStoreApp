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
using ProjectPRN.Models;

namespace ProjectPRN
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Window
    {
        public DashBoard()
        {
            InitializeComponent();
            mainFrame.Navigate(new HomePage());
        }
        private void btnTrangChu_Click(object sender, RoutedEventArgs e)
        {
            // Chuyển đến trang Trang Chủ
            mainFrame.Navigate(new HomePage());
        }

        private void btnKhachHangMuaNhieu_Click(object sender, RoutedEventArgs e)
        {
            // Chuyển đến trang Khách Hàng Mua Nhiều
            mainFrame.Navigate(new TopCustomers());
        }

        private void btnKhachHangHuyDonNhieu_Click(object sender, RoutedEventArgs e)
        {
            // Chuyển đến trang Khách Hàng Hủy Đơn Nhiều
            mainFrame.Navigate(new TopCancelCustomers());
        }

        private void btnNguoiDungTheoHang_Click(object sender, RoutedEventArgs e)
        {
            // Chuyển đến trang Số Người Dùng Theo Hạng
            mainFrame.Navigate(new RankMemberStatistics());
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.Logout();
            MessageBox.Show("Bạn đã đăng xuất thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            Home home = new Home();
            home.Show();
            this.Close();
        }

        private void btnStatistic_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new StatisticWindow());

        }

        private void btnProductStatistic_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new ProductStatistic());

        }

        private void btnRevenue_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new RevenueandProfitStatistic());

        }
    }
}

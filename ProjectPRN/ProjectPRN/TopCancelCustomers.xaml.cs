using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.EntityFrameworkCore;
using ProjectPRN.Models;

namespace ProjectPRN
{
    /// <summary>
    /// Interaction logic for TopCancelCustomers.xaml
    /// </summary>
    public partial class TopCancelCustomers : Page
    {
        private readonly ProjectPrnContext context;
        private ObservableCollection<TopCustomer> topCancelCustomer;
        public TopCancelCustomers()
        {
            topCancelCustomer = new ObservableCollection<TopCustomer>();
            context = new ProjectPrnContext();
            InitializeComponent();
            LoadCbxTopCancel();
            LoadDB();
        }

        private void LoadDB()
        {
            GetTopCancelCustomer();
            if (topCancelCustomer != null && topCancelCustomer.Any())
            {
                dgvTopCancel.ItemsSource = topCancelCustomer;
            }
        }
        private void GetTopCancelCustomer()
        {
            int selectedYear = DateTime.Now.Year;
            try
            {
                var query = context.Orders
                    .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == selectedYear && o.Status == "Canceled")
                    .Include(o => o.Account)  // Load thông tin Account
                    .ThenInclude(acc => acc.AccountRank);  // Load Rank của Accou
                var orders = query.ToList(); // Lấy dữ liệu vào bộ nhớ C#nt

                var topCuss = orders
                    .GroupBy(o => o.Account)  // Nhóm theo tài khoản
                    .Select((g, index) => new TopCustomer
                    {
                        Index = index + 1,
                        CustomerName = g.Key?.Fullname ?? "Không xác định",
                        Rank = g.Key?.AccountRank?.AccountRankName ?? "Không có Rank",
                        OrdersCount = g.Count()
                    })
                    .OrderByDescending(g => g.OrdersCount)
                    .ToList();

                // Cập nhật danh sách hiển thị
                topCancelCustomer.Clear();
                foreach (var cus in topCuss)
                {
                    topCancelCustomer.Add(cus);
                }

                dgvTopCancel.ItemsSource = null; // Đảm bảo cập nhật DataGrid
                dgvTopCancel.ItemsSource = topCancelCustomer;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xảy ra khi lấy dữ liệu khách hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GetTopCancelCustomer(int? selectedMonth)
        {
            int selectedYear = DateTime.Now.Year;
            try
            {
                var query = context.Orders
                    .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == selectedYear && o.OrderDate.Value.Month == selectedMonth && o.Status == "Canceled")
                    .Include(o => o.Account)  // Load thông tin Account
                    .ThenInclude(acc => acc.AccountRank);  // Load Rank của Accou
                var orders = query.ToList(); // Lấy dữ liệu vào bộ nhớ C#nt

                var topCuss = orders
                    .GroupBy(o => o.Account)  // Nhóm theo tài khoản
                    .Select((g, index) => new TopCustomer
                    {
                        Index = index + 1,
                        CustomerName = g.Key?.Fullname ?? "Không xác định",
                        Rank = g.Key?.AccountRank?.AccountRankName ?? "Không có Rank",
                        OrdersCount = g.Count()
                    })
                    .OrderByDescending(g => g.OrdersCount)
                    .ToList();

                // Cập nhật danh sách hiển thị
                topCancelCustomer.Clear();
                foreach (var cus in topCuss)
                {
                    topCancelCustomer.Add(cus);
                }

                dgvTopCancel.ItemsSource = null; // Đảm bảo cập nhật DataGrid
                dgvTopCancel.ItemsSource = topCancelCustomer;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xảy ra khi lấy dữ liệu khách hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadCbxTopCancel()
        {
            cbxTopCancel.Items.Add("Tháng");
            cbxTopCancel.SelectedIndex = 0;
            for (int i = 1; i <= 12; i++)
            {
                cbxTopCancel.Items.Add("Tháng " + i);
            }
        }

        private void cbxTopCancel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxTopCancel.SelectedItem == null) return;

            // Lấy giá trị tháng từ ComboBox, nếu là "Tất cả" thì không lọc
            string selectedValue = cbxTopCancel.SelectedItem.ToString();
            int? selectedMonth = selectedValue == "Tháng" ? (int?)null : int.Parse(selectedValue.Split(' ')[1]);
            if (!selectedMonth.HasValue)
            {
                GetTopCancelCustomer();
            }
            else
            {
                GetTopCancelCustomer(selectedMonth);
            }

            if (topCancelCustomer != null && topCancelCustomer.Any())
            {
                dgvTopCancel.ItemsSource = null; // Đảm bảo DataGrid được làm mới
                dgvTopCancel.ItemsSource = topCancelCustomer;
            }
            else
            {
                dgvTopCancel.ItemsSource = null;
                MessageBox.Show("Không có dữ liệu khách hàng hủy đơn.");
            }
        }
    }
}

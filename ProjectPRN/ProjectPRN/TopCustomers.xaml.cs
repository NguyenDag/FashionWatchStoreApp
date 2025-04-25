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
    /// Interaction logic for TopCustomers.xaml
    /// </summary>
    public partial class TopCustomers : Page
    {
        private readonly ProjectPrnContext context;
        private ObservableCollection<TopCustomer> topCustomers;
        public TopCustomers()
        {
            context = new ProjectPrnContext();
            topCustomers = new ObservableCollection<TopCustomer>();
            InitializeComponent();
            LoadCbxTopCustomer();
            LoadDB();
        }
        private void LoadCbxTopCustomer()
        {
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 3; i <= currentYear; i++)
            {
                cbxYearFilter.Items.Add(i.ToString());
            }
            cbxYearFilter.SelectedIndex = 3; // Mặc định chọn năm hiện tại
            cbxTopCustomer.Items.Add("Tháng");
            cbxTopCustomer.Items.Add("Quý");
        }
        private void LoadDB()
        {
            int currentYear = DateTime.Now.Year; // Lấy năm hiện tại
            GetTopCustomers(currentYear);
            // Kiểm tra danh sách trước khi gán vào DataGrid
            if (topCustomers != null && topCustomers.Any())
            {
                dgvTopProducts.ItemsSource = topCustomers;
            }
            else
            {
                MessageBox.Show("Không có dữ liệu khách hàng mua nhiều.");
            }
        }

        private void GetTopCustomers(int selectedYear)
        {
            try
            {
                var orders = context.Orders
                    .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == selectedYear && o.Status == "Completed")
                    .Include(o => o.Account)  // Load thông tin Account
                    .ThenInclude(a => a.AccountRank)  // Load Rank của Account
                    .ToList();  // Lấy dữ liệu vào bộ nhớ C#

                var topCuss = orders
                    .GroupBy(o => o.Account)  // GroupBy xử lý trên C#
                    .Select((g, index) => new TopCustomer
                    {
                        Index = index + 1,
                        CustomerName = g.Key?.Fullname ?? "Không xác định",
                        Rank = g.Key?.AccountRank?.AccountRankName ?? "Không có Rank",
                        OrdersCount = g.Count(),
                        TotalPrice = g.Sum(o => o.TotalCost)
                    })
                    .OrderByDescending(g => g.OrdersCount)
                    .ToList();
                topCustomers.Clear();
                foreach (var cus in topCuss)
                {
                    topCustomers.Add(cus);
                }

                dgvTopProducts.ItemsSource = topCustomers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xảy ra khi lấy dữ liệu khách hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbxYearFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxYearFilter.SelectedItem == null) return;
            int selectedYear = int.Parse(cbxYearFilter.SelectedItem.ToString());
            GetTopCustomers(selectedYear);

            // Luôn cập nhật ItemsSource, kể cả khi danh sách rỗng
            dgvTopProducts.ItemsSource = null; // Đặt null trước để reset binding
            dgvTopProducts.ItemsSource = topCustomers;

            if (topCustomers == null || !topCustomers.Any())
            {
                MessageBox.Show("Không có dữ liệu khách hàng.");
            }
        }

        private void cbxTopCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxTopCustomer.SelectedItem == null) return;

            string selectedValue = cbxTopCustomer.SelectedItem.ToString();
            cbxTimeFilter.Items.Clear();

            if (selectedValue == "Tháng")
            {
                cbxTimeFilter.Visibility = Visibility.Visible;
                cbxTimeFilter.Items.Add("Tháng");
                for (int i = 1; i <= 12; i++)
                {
                    cbxTimeFilter.Items.Add("Tháng " + i);
                }
                cbxTimeFilter.SelectedIndex = 0;
            }
            else if (selectedValue == "Quý")
            {
                cbxTimeFilter.Visibility = Visibility.Visible;
                cbxTimeFilter.Items.Add("Quý");
                cbxTimeFilter.Items.Add("Quý 1");
                cbxTimeFilter.Items.Add("Quý 2");
                cbxTimeFilter.Items.Add("Quý 3");
                cbxTimeFilter.Items.Add("Quý 4");
                cbxTimeFilter.SelectedIndex = 0;
            }
            else
            {
                cbxTimeFilter.Visibility = Visibility.Collapsed;
            }
        }

        private void cbxTimeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxTimeFilter.SelectedItem == null || cbxYearFilter.SelectedItem == null) return;

            int selectedYear = int.Parse(cbxYearFilter.SelectedItem.ToString());
            string selectedValue = cbxTimeFilter.SelectedItem.ToString();

            if (selectedValue.StartsWith("Tháng "))
            {
                int selectedMonth = int.Parse(selectedValue.Split(' ')[1]); // Lấy số tháng từ chuỗi "Tháng X"
                GetTopCustomersByMonth(selectedYear, selectedMonth);
            }
            else if (selectedValue.StartsWith("Quý "))
            {
                int selectedQuarter = int.Parse(selectedValue.Split(' ')[1]); // Lấy số quý từ chuỗi "Quý X"
                GetTopCustomersByQuarter(selectedYear, selectedQuarter);
            }
            else
            {
                GetTopCustomers(selectedYear); // Mặc định lấy theo năm
            }

            // Luôn cập nhật DataGrid
            dgvTopProducts.ItemsSource = null;
            dgvTopProducts.ItemsSource = topCustomers;
        }
        private void GetTopCustomersByQuarter(int selectedYear, int selectedQuarter)
        {
            int startMonth = (selectedQuarter - 1) * 3 + 1; // Xác định tháng bắt đầu của quý
            int endMonth = startMonth + 2;
            try
            {
                var orders = context.Orders
                    .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == selectedYear &&
                    o.OrderDate.Value.Month >= startMonth && o.OrderDate.Value.Month <= endMonth && o.Status == "Completed")
                    .Include(o => o.Account)  // Load thông tin Account
                    .ThenInclude(a => a.AccountRank)  // Load Rank của Account
                    .ToList();  // Lấy dữ liệu vào bộ nhớ C#

                var topCuss = orders
                    .GroupBy(o => o.Account)  // GroupBy xử lý trên C#
                    .Select((g, index) => new TopCustomer
                    {
                        Index = index + 1,
                        CustomerName = g.Key?.Fullname ?? "Không xác định",
                        Rank = g.Key?.AccountRank?.AccountRankName ?? "Không có Rank",
                        OrdersCount = g.Count(),
                        TotalPrice = g.Sum(o => o.TotalCost)
                    })
                    .OrderByDescending(g => g.OrdersCount)
                    .ToList();
                topCustomers.Clear();
                foreach (var cus in topCuss)
                {
                    topCustomers.Add(cus);
                }

                dgvTopProducts.ItemsSource = topCustomers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xảy ra khi lấy dữ liệu khách hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetTopCustomersByMonth(int selectedYear, int selectedMonth)
        {
            try
            {
                var orders = context.Orders
                    .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == selectedYear && o.OrderDate.Value.Month == selectedMonth && o.Status == "Completed")
                    .Include(o => o.Account)  // Load thông tin Account
                    .ThenInclude(a => a.AccountRank)  // Load Rank của Account
                    .ToList();  // Lấy dữ liệu vào bộ nhớ C#

                var topCuss = orders
                    .GroupBy(o => o.Account)  // GroupBy xử lý trên C#
                    .Select((g, index) => new TopCustomer
                    {
                        Index = index + 1,
                        CustomerName = g.Key?.Fullname ?? "Không xác định",
                        Rank = g.Key?.AccountRank?.AccountRankName ?? "Không có Rank",
                        OrdersCount = g.Count(),
                        TotalPrice = g.Sum(o => o.TotalCost)
                    })
                    .OrderByDescending(g => g.OrdersCount)
                    .ToList();
                topCustomers.Clear();
                foreach (var cus in topCuss)
                {
                    topCustomers.Add(cus);
                }

                dgvTopProducts.ItemsSource = topCustomers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xảy ra khi lấy dữ liệu khách hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

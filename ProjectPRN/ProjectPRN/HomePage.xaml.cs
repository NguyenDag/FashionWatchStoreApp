using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private readonly ProjectPrnContext context;
        private ObservableCollection<TopCustomer> topCustomers;
        private ObservableCollection<TopCustomer> topCancelCustomer;
        private ObservableCollection<RankGroup> rankGroups;
        public HomePage()
        {
            topCustomers = new ObservableCollection<TopCustomer>();
            topCancelCustomer = new ObservableCollection<TopCustomer>();
            rankGroups = new ObservableCollection<RankGroup>();
            context = new ProjectPrnContext();
            InitializeComponent();
            LoadCbxTopCustomer();
            LoadCbxTopCancel();
            LoadDB();
        }

        private void LoadDB()
        {
            //tải top 10 khách hàng mua nhiều nhất
            int currentYear = DateTime.Now.Year; // Lấy năm hiện tại
            GetTopCustomers(currentYear);
            // Kiểm tra danh sách trước khi gán vào DataGrid
            if (topCustomers != null && topCustomers.Any())
            {
                dgvTopCustomers.ItemsSource = topCustomers;
            }
            else
            {
                MessageBox.Show("Không có dữ liệu khách hàng mua nhiều.");
            }
            //top 10 khách hàng boom hàng nhiều nhất
            GetTopCancelCustomer();
            if (topCancelCustomer != null && topCancelCustomer.Any())
            {
                dgvTopCancel.ItemsSource = topCancelCustomer;
            }
            
            //số thành viên theo rank
            GetRankMemberStatistic();
        }

        private void GetRankMemberStatistic()
        {
            try
            {
                var rankStatistics = context.AccountRanks
                    .GroupJoin(
                        context.Accounts.Where(acc => acc.Role == 1),
                        rank => rank.AccountRankId, // Khóa chính từ AccountRanks
                        acc => acc.AccountRankId,   // Khóa ngoại từ Accounts
                        (rank, accounts) => new
                        {
                            Rank = rank,
                            AccsCount = accounts.Count() // Đếm số lượng tài khoản trong mỗi nhóm
                        }
                    )
                    .AsEnumerable() // Chuyển sang LINQ to Objects để sử dụng index
                    .Select((group, index) => new RankGroup
                    {
                        Index = index + 1,
                        RankName = group.Rank.AccountRankName,
                        MinAmount = group.Rank.MinAmount,
                        AccsCount = group.AccsCount
                    })
                    .OrderByDescending(r => r.AccsCount) // Sắp xếp theo số lượng tài khoản giảm dần
                    .ToList();

                if (rankStatistics.Any())
                {
                    dgvRankMemberStatistic.ItemsSource = null; // Đảm bảo cập nhật DataGrid
                    dgvRankMemberStatistic.ItemsSource = rankStatistics;
                }
                else
                {
                    dgvRankMemberStatistic.ItemsSource = null;
                    MessageBox.Show("Không có dữ liệu xếp hạng thành viên.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xảy ra khi lấy dữ liệu xếp hạng thành viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    .Take(10)
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
                    .Take(10)
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
        public void GetTopCustomers(int selectedYear)
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
                        OrdersCount = g.Count()
                    })
                    .OrderByDescending(g => g.OrdersCount)
                    .Take(10)
                    .ToList();
                topCustomers.Clear();
                foreach (var cus in topCuss)
                {
                    topCustomers.Add(cus);
                }

                dgvTopCustomers.ItemsSource = topCustomers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xảy ra khi lấy dữ liệu khách hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void cbxYearFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxYearFilter.SelectedItem == null) return;
            int selectedYear = int.Parse(cbxYearFilter.SelectedItem.ToString());
            GetTopCustomers(selectedYear);

            // Luôn cập nhật ItemsSource, kể cả khi danh sách rỗng
            dgvTopCustomers.ItemsSource = null; // Đặt null trước để reset binding
            dgvTopCustomers.ItemsSource = topCustomers;

            if (topCustomers == null || !topCustomers.Any())
            {
                MessageBox.Show("Không có dữ liệu khách hàng.");
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
            dgvTopCustomers.ItemsSource = null;
            dgvTopCustomers.ItemsSource = topCustomers;
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
                        OrdersCount = g.Count()
                    })
                    .OrderByDescending(g => g.OrdersCount)
                    .Take(10)
                    .ToList();
                topCustomers.Clear();
                foreach (var cus in topCuss)
                {
                    topCustomers.Add(cus);
                }

                dgvTopCustomers.ItemsSource = topCustomers;
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
                        OrdersCount = g.Count()
                    })
                    .OrderByDescending(g => g.OrdersCount)
                    .Take(10)
                    .ToList();
                topCustomers.Clear();
                foreach (var cus in topCuss)
                {
                    topCustomers.Add(cus);
                }

                dgvTopCustomers.ItemsSource = topCustomers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xảy ra khi lấy dữ liệu khách hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
            }
        }
    }
    public class TopCustomer
    {
        public int Index { get; set; }
        public string? CustomerName { get; set; }
        public string? Rank { get; set; }
        public int OrdersCount { get; set; }
        public decimal? TotalPrice { get; set; }
    }
    public class RankGroup
    {
        public int Index { get; set; }
        public int RankId {  get; set; }
        public string? RankName { get; set; }
        public decimal? MinAmount { get; set; }
        public int AccsCount { get; set; }
    }
}

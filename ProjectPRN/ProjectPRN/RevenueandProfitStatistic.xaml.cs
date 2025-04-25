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
using LiveCharts.Wpf;
using LiveCharts;
using ProjectPRN.Models;

namespace ProjectPRN
{
    /// <summary>
    /// Interaction logic for RevenueandProfíttatistic.xaml
    /// </summary>
    public partial class RevenueandProfitStatistic : Page
    {
        public Func<double, string> Formatter { get; set; }
        public List<string> ProductTypeLabels { get; set; } = new List<string>();
        public List<string> BrandLabels { get; set; } = new List<string>();
        public List<string> CategoryLabels { get; set; } = new List<string>();

        private int currentPage = 1;
        private const int pageSize = 13;

        public RevenueandProfitStatistic()
        {
            InitializeComponent();
            Formatter = value => value.ToString("C");
            DataContext = this;
            LoadYears();
        }

        private void LoadYears()
        {
            using (var context = new ProjectPrnContext())
            {
                var years = context.OrderDetails
                    .Where(od => od.Order.OrderDate.HasValue)
                    .Select(od => od.Order.OrderDate.Value.Year)
                    .Distinct()
                    .OrderBy(y => y)
                    .ToList();

                YearComboBox.ItemsSource = years;
            }
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMonthComboBox();
            LoadData();
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void LoadMonthComboBox()
        {
            MonthComboBox.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                MonthComboBox.Items.Add(new ComboBoxItem { Content = $"Month {i}", Tag = i });
            }
            MonthComboBox.Items.Add(new ComboBoxItem { Content = "Year", Tag = (int?)null });
            MonthComboBox.SelectedIndex = 12; // Default to Year
        }

        private void LoadData()
        {
            int selectedYear = (int)(YearComboBox.SelectedItem ?? DateTime.Now.Year);
            int? selectedMonth = (int?)((ComboBoxItem)MonthComboBox.SelectedItem)?.Tag;

            var productTypeData = GetTopProductsByRevenue(selectedYear, selectedMonth, currentPage, pageSize);
            var totalRevenue = GetTotalRevenue(selectedYear, selectedMonth);
            var totalProfit = GetTotalProfit(selectedYear, selectedMonth);

            ProductTypeChart.Series = new SeriesCollection
{
    new ColumnSeries
    {
        Title = "Revenue",
        Values = new ChartValues<double>(productTypeData.Values.Select(v => v.Revenue))
    }
};

            ProductTypeLabels = productTypeData.Keys.ToList();
            ProductTypeChart.AxisX.Clear();
            ProductTypeChart.AxisX.Add(new Axis
            {
                Labels = ProductTypeLabels
            });

            TotalRevenueTextBlock.Text = $"Total Revenue: {totalRevenue:C}";
            TotalProfitTextBlock.Text = $"Total Profit: {totalProfit:C}";
        }

        private Dictionary<string, (double Revenue, double Profit)> GetTopProductsByRevenue(int year, int? month, int pageNumber, int pageSize)
        {
            using (var context = new ProjectPrnContext())
            {
                var query = context.OrderDetails
                    .Join(context.Products, od => od.ProductId, p => p.ProductId, (od, p) => new { od, p })
                    .Where(x => x.od.Order.OrderDate.HasValue && x.od.Order.OrderDate.Value.Year == year);

                if (month.HasValue)
                {
                    query = query.Where(x => x.od.Order.OrderDate.Value.Month == month);
                }

                return query
                    .GroupBy(x => x.p.ProductName)
                    .Select(g => new
                    {
                        ProductName = g.Key,
                        Revenue = g.Sum(x => (double?)x.od.Quantity * (double?)x.od.Price) ?? 0,
                        Profit = g.Sum(x => (double?)x.od.Quantity * (double?)(x.od.Price - x.p.PriceBuy)) ?? 0
                    })
                    .OrderByDescending(x => x.Revenue)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToDictionary(x => x.ProductName, x => (x.Revenue, x.Profit));
            }
        }

        private double GetTotalRevenue(int year, int? month)
        {
            using (var context = new ProjectPrnContext())
            {
                var query = context.OrderDetails
                    .Where(od => od.Order.OrderDate.HasValue && od.Order.OrderDate.Value.Year == year
                                 && (od.Order.Status == "Shipping" || od.Order.Status == "Completed"));

                if (month.HasValue)
                {
                    query = query.Where(od => od.Order.OrderDate.Value.Month == month);
                }

                return query.Sum(od => (double?)od.Order.ActualCost) ?? 0;
            }
        }

        private double GetTotalProfit(int year, int? month)
        {
            using (var context = new ProjectPrnContext())
            {
                var query = context.OrderDetails
                    .Join(context.Products, od => od.ProductId, p => p.ProductId, (od, p) => new { od, p })
                    .Where(x => x.od.Order.OrderDate.HasValue && x.od.Order.OrderDate.Value.Year == year);

                if (month.HasValue)
                {
                    query = query.Where(x => x.od.Order.OrderDate.Value.Month == month);
                }

                return query.Sum(x => (double?)x.od.Quantity * (double?)(x.od.Price - x.p.PriceBuy)) ?? 0;
            }
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartPoint)
        {
            var series = (ColumnSeries)chartPoint.SeriesView;
            var chart = (CartesianChart)series.Model.Chart.View;
            string label = string.Empty;
            double revenue = 0;
            double profit = 0;

            if (chart == ProductTypeChart)
            {
                label = ProductTypeLabels[chartPoint.Key];
                var data = GetTopProductsByRevenue((int)YearComboBox.SelectedItem, (int?)((ComboBoxItem)MonthComboBox.SelectedItem)?.Tag, currentPage, pageSize);
                if (data.ContainsKey(label))
                {
                    revenue = data[label].Revenue;
                    profit = data[label].Profit;
                }
            }

            MessageBox.Show($"Product: {label}\nRevenue: {revenue:C}\nProfit: {profit:C}", "Product Details", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

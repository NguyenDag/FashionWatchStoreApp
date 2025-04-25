using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using ProjectPRN.Models;
using MessageBox = System.Windows.MessageBox;

namespace ProjectPRN
{
    public partial class ProductStatistic : Page
    {
        private ProjectPrnContext _context;

        public ProductStatistic()
        {
            InitializeComponent();
            _context = new ProjectPrnContext();
            LoadProductMostSell(DateTime.Today, DateTime.Today.AddDays(1));
            LoadProductNotSold(DateTime.Today, DateTime.Today.AddDays(1));

        }

        private void LoadProductMostSell(DateTime startDate, DateTime endDate)
        {
            var topProducts = (from od in _context.OrderDetails
                               join o in _context.Orders on od.OrderId equals o.OrderId
                               join p in _context.Products on od.ProductId equals p.ProductId
                               where o.OrderDate >= startDate && o.OrderDate <= endDate
                               group od by new { od.ProductId, p.ProductName } into g
                               select new
                               {
                                   ProductName = g.Key.ProductName,
                                   TotalQuantity = g.Sum(od => od.Quantity)
                               })
                               .OrderByDescending(p => p.TotalQuantity)
                               .Take(5)
                               .ToList();

            dgMostProductStatistics.ItemsSource = topProducts;
        }


        private void LoadProductNotSold(DateTime startDate, DateTime endDate)
        {
            var notSoldProducts = (from p in _context.Products
                                   where !(from od in _context.OrderDetails
                                           join o in _context.Orders on od.OrderId equals o.OrderId
                                           where o.OrderDate >= startDate && o.OrderDate <= endDate
                                           select od.ProductId)
                                           .Contains(p.ProductId)
                                   select new
                                   {
                                       p.ProductId,
                                       p.ProductName
                                   })
                                   .ToList();

            dgLeastProductStatistic.ItemsSource = notSoldProducts;
        }



        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;
           
            // Kiểm tra nếu đang ở chế độ "Khoảng thời gian tùy chọn"
            if (cbTimeFilter.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content.ToString() == "Khoảng thời gian tùy chọn")
            {
                if (startDate == null || endDate == null)
                {
                    MessageBox.Show("Vui lòng chọn cả ngày bắt đầu và ngày kết thúc!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (startDate > endDate)
                {
                    MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                return;
            }

            // Gọi hàm tải dữ liệu
            LoadProductMostSell(startDate.Value, endDate.Value);
            LoadProductNotSold(startDate.Value, endDate.Value);
        }


        private void CbTimeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTimeFilter.SelectedItem == null) return;

            string selectedTime = (cbTimeFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            // Kiểm tra nếu chọn "Khoảng thời gian tùy chọn"
            bool isCustom = selectedTime == "Khoảng thời gian tùy chọn";
            dpStartDate.IsEnabled = isCustom;
            dpEndDate.IsEnabled = isCustom;
            if (isCustom)
            {
                dpStartDate.SelectedDate = null;
                dpEndDate.SelectedDate = null;
                return;
            }

            switch (selectedTime)
            {
                case "Hôm nay":
                    startDate = DateTime.Today;
                    endDate = DateTime.Today.AddDays(1);
                    break;
                case "7 ngày qua":
                    startDate = DateTime.Today.AddDays(-7);
                    endDate = DateTime.Today.AddDays(1);
                    break;
                case "30 ngày qua":
                    startDate = DateTime.Today.AddDays(-30);
                    endDate = DateTime.Today.AddDays(1);
                    break;
                case "Năm nay":
                    startDate = new DateTime(DateTime.Today.Year, 1, 1);
                    endDate = DateTime.Today.AddDays(1);
                    break;
            }

            // Gọi hàm lọc dữ liệu ngay khi chọn thời gian
            LoadProductMostSell(startDate, endDate);
            LoadProductNotSold(startDate, endDate);
            }
        }


    }


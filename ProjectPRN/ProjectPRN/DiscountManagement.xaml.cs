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
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectPRN.Models;

namespace ProjectPRN
{
    /// <summary>
    /// Interaction logic for DiscountManagement.xaml
    /// </summary>
    public partial class DiscountManagement : Window
    {
        public DiscountManagement()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDiscount();
            LoadAccountRank();
        }

        public void LoadDiscount()
        {
            var discounts = ProjectPrnContext.Ins.Discounts.Select(d => new
            {
                d.DiscountId,
                d.DiscountName,
                d.Percent,
                d.MinCost,
                d.Amount,
                d.EndDate,
                d.MyStatus,
                d.AccountRankId,
                AccountRankName = ProjectPrnContext.Ins.AccountRanks
                    .Where(a => a.AccountRankId == d.AccountRankId)
                    .Select(a => a.AccountRankName)
                    .FirstOrDefault()
            }).ToList();
            dgvDisplay.ItemsSource = discounts;
        }


        public void LoadAccountRank()
        {
            var accountRanks = ProjectPrnContext.Ins.AccountRanks.ToList();
            cbxAccountRank.ItemsSource = accountRanks;
            cbxAccountRank.DisplayMemberPath = "AccountRankName";
            cbxAccountRank.SelectedValuePath = "AccountRankId";
            cbxAccountRank.SelectedIndex = 0;
        }


        private void dgvDisplay_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var stds = dgvDisplay.SelectedItem as dynamic;
            if (stds != null)
            {
                txtDiscountId.Text = stds.DiscountId.ToString();
                txtDiscountName.Text = stds.DiscountName.ToString();
                cbxAccountRank.SelectedValue = stds.AccountRankId;
                txtPercent.Text = stds.Percent.ToString();
                txtMinCost.Text = stds.MinCost.ToString();
                txtAmount.Text = stds.Amount.ToString(); 
                dpkEndDate.SelectedDate = DateTime.Parse(stds.EndDate.ToString());
                cbxStatus.Text = stds.MyStatus;
            }
        }


        private void clearForm()
        {
            txtDiscountId.Text = string.Empty;
            txtDiscountName.Text = "";
            cbxAccountRank.SelectedIndex = 0;
            txtPercent.Text = string.Empty;
            dpkEndDate.SelectedDate = null;
            txtMinCost.Text = string.Empty;
            txtAmount.Text = string.Empty; 
            cbxStatus.Text = string.Empty;
        }



        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDiscountName.Text))
            {
                MessageBox.Show("Tên mã giảm giá không được để trống.");
                return;
            }

            if (!double.TryParse(txtPercent.Text, out double percent) || percent <= 0)
            {
                MessageBox.Show("Giá trị phần trăm không hợp lệ. Vui lòng nhập một số dương.");
                return;
            }

            if (!decimal.TryParse(txtMinCost.Text, out decimal minCost) || minCost < 0)
            {
                MessageBox.Show("Giá trị chi phí tối thiểu không hợp lệ. Vui lòng nhập một số không âm.");
                return;
            }
            if (!int.TryParse(txtAmount.Text, out int amount) || amount < 0)
            {
                MessageBox.Show("Giá trị Amount không hợp lệ. Vui lòng nhập một số nguyên không âm.");
                return;
            }




            if (!dpkEndDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Ngày kết thúc không được để trống.");
                return;
            }

            var newDiscount = new Discount
            {
                DiscountName = txtDiscountName.Text,
                Percent = percent,
                MinCost = minCost,
                Amount = amount, 
                EndDate = DateOnly.FromDateTime(dpkEndDate.SelectedDate.Value),
                Status = cbxStatus.Text == "Active",
                AccountRankId = (int)cbxAccountRank.SelectedValue
            };

            ProjectPrnContext.Ins.Discounts.Add(newDiscount);
            ProjectPrnContext.Ins.SaveChanges();

            MessageBox.Show("Bạn đã thêm mã giảm giá mới thành công!");
            LoadDiscount();
            clearForm();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(txtDiscountId.Text);
                var x = ProjectPrnContext.Ins.Discounts.Find(id);
                if (x != null)
                {
                    ProjectPrnContext.Ins.Discounts.Remove(x);
                    ProjectPrnContext.Ins.SaveChanges();
                    MessageBox.Show("Bạn đã xóa mã giảm giá mới thành công!");
                    LoadDiscount();
                    clearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDiscountName.Text))
            {
                MessageBox.Show("Tên mã giảm giá không được để trống.");
                return;
            }

            if (!double.TryParse(txtPercent.Text, out double percent) || percent <= 0)
            {
                MessageBox.Show("Giá trị phần trăm không hợp lệ. Vui lòng nhập một số dương.");
                return;
            }

            if (!decimal.TryParse(txtMinCost.Text, out decimal minCost) || minCost < 0)
            {
                MessageBox.Show("Giá trị chi phí tối thiểu không hợp lệ. Vui lòng nhập một số không âm.");
                return;
            }

            if (!int.TryParse(txtAmount.Text, out int amount) || amount < 0)
            {
                MessageBox.Show("Giá trị Amount không hợp lệ. Vui lòng nhập một số nguyên không âm.");
                return;
            }


            if (!dpkEndDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Ngày kết thúc không được để trống.");
                return;
            }

            int discountId = int.Parse(txtDiscountId.Text);
            var discountToUpdate = ProjectPrnContext.Ins.Discounts.Find(discountId);

            if (discountToUpdate != null)
            {
                discountToUpdate.DiscountName = txtDiscountName.Text;
                discountToUpdate.Percent = percent;
                discountToUpdate.MinCost = minCost;
                discountToUpdate.Amount = amount; 
                discountToUpdate.EndDate = DateOnly.FromDateTime(dpkEndDate.SelectedDate.Value);
                discountToUpdate.Status = cbxStatus.Text == "Active";
                discountToUpdate.AccountRankId = (int)cbxAccountRank.SelectedValue;

                ProjectPrnContext.Ins.SaveChanges();

                MessageBox.Show("Bạn đã cập nhật mã giảm giá thành công!");
                LoadDiscount();
                clearForm();
            }
            else
            {
                MessageBox.Show("Mã giảm giá không tồn tại.");
            }
        }

        private void btnCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoryManagementWindow categoryManagementWindow = new CategoryManagementWindow();
            categoryManagementWindow.Show();
            this.Close();
        }


        private void btnDiscount_Click(object sender, RoutedEventArgs e)
        {
            DiscountManagement dis = new DiscountManagement();
            dis.Show();
            this.Close();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Close();
        }


    }
}

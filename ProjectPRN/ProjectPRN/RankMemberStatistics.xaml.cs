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
using ProjectPRN.Models;

namespace ProjectPRN
{
    /// <summary>
    /// Interaction logic for RankMemberStatistics.xaml
    /// </summary>
    public partial class RankMemberStatistics : Page
    {
        private readonly ProjectPrnContext context;
        private ObservableCollection<Account> accounts;
        public RankMemberStatistics()
        {
            context = new ProjectPrnContext();
            accounts = new ObservableCollection<Account>();
            InitializeComponent();
            LoadDB();
        }

        private void LoadDB()
        {
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
                        RankId = group.Rank.AccountRankId,
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

        private void dgvRankMemberStatistic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RankGroup rankGroup = dgvRankMemberStatistic.SelectedItem as RankGroup;
            if (rankGroup != null)
            {
                GetAccountsByRankID(rankGroup.RankId);
                dgvListCustomerRank.ItemsSource = accounts;
            }
        }

        private void GetAccountsByRankID(int rankId)
        {
            try
            {
                var accountList = context.Accounts
                    .Where(a => a.AccountRankId == rankId && a.Role == 1)
                    .ToList();

                accounts.Clear();
                foreach (var account in accountList)
                {
                    accounts.Add(account);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách tài khoản: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

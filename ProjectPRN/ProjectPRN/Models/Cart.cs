using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPRN.Models
{
    public partial class Cart : INotifyPropertyChanged
    {
        public int CartId { get; set; }
        public int AccountId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;

        public string ProductName => Product?.ProductName ?? "N/A";
        public decimal PriceSell => Product?.PriceSell ?? 0;
        public decimal TotalPrice => Product != null ? Product.PriceSell * Quantity : 0;

        // ✅ Thuộc tính này không có trong database
        private bool _isSelected;

        [NotMapped] // Đảm bảo Entity Framework bỏ qua thuộc tính này
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

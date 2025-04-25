using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProjectPRN.Models
{
    internal class ProductViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BitmapImage> displayedImages = new ObservableCollection<BitmapImage>();
        private ICollectionView _productView;
        public ICollectionView ProductView
        {
            get { return _productView; }
            set { _productView = value; NotifyPropertyChanged(nameof(ProductView)); }
        }
        public ObservableCollection<BitmapImage> DisplayedImages
        {
            get { return displayedImages; }
            set
            {
                displayedImages = value;
                NotifyPropertyChanged(nameof(DisplayedImages));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

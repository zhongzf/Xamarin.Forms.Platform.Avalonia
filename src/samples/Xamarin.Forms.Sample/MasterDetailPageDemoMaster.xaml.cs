using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Sample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPageDemoMaster : ContentPage
    {
        public ListView ListView;

        public MasterDetailPageDemoMaster()
        {
            InitializeComponent();

            BindingContext = new MasterDetailPageDemoMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MasterDetailPageDemoMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterDetailPageDemoMasterMenuItem> MenuItems { get; set; }

            public MasterDetailPageDemoMasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterDetailPageDemoMasterMenuItem>(new[]
                {
                    new MasterDetailPageDemoMasterMenuItem { Id = 0, Title = "Page 1" },
                    new MasterDetailPageDemoMasterMenuItem { Id = 1, Title = "Page 2", TargetType = typeof(MainPage) },
                    new MasterDetailPageDemoMasterMenuItem { Id = 2, Title = "Page 3", TargetType = typeof(CarouselPageDemo) },
                    new MasterDetailPageDemoMasterMenuItem { Id = 3, Title = "Page 4" },
                    new MasterDetailPageDemoMasterMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}
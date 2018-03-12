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

namespace GameOfChallengers.Views.Monsters
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonsterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public MonsterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new MonsterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MonsterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MonsterDetailPageMenuItem> MenuItems { get; set; }
            
            public MonsterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MonsterDetailPageMenuItem>(new[]
                {
                    new MonsterDetailPageMenuItem { Id = 0, Title = "Page 1" },
                    new MonsterDetailPageMenuItem { Id = 1, Title = "Page 2" },
                    new MonsterDetailPageMenuItem { Id = 2, Title = "Page 3" },
                    new MonsterDetailPageMenuItem { Id = 3, Title = "Page 4" },
                    new MonsterDetailPageMenuItem { Id = 4, Title = "Page 5" },
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
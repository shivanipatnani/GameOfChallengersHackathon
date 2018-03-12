using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GameOfChallengers.ViewModels;
using GameOfChallengers.Models;
using GameOfChallengers.Views.Battle;
using System.Collections.ObjectModel;

namespace GameOfChallengers.Views.Character
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildTeamPage : ContentPage
    {
        public CharactersViewModel viewModel;
        public CreatureDetailViewModel _viewModel;
        public ObservableCollection<Creature> Dataset { get; set; }
        public Creature Data { get; set; }

        public BuildTeamPage()
        {
            InitializeComponent();
            Data = new Creature
            {
                Name = "Character name",
                Id = Guid.NewGuid().ToString(),
                Type = 0,
                Level = 1,
                XP = 0,
                MaxHealth = 10,
                CurrHealth = 10,
                Alive = true,

            };

            _viewModel = new CreatureDetailViewModel(Data);
            BindingContext = viewModel = CharactersViewModel.Instance;
            //CharacterPicker1.SelectedIndex = 0;
        }

        public class LabelGridCode : ContentPage
        {
            //public LabelGridCode()

            //{
            //    var grid = new Xamarin.Forms.Grid();
            //    var topLeft = new Xamarin.Forms.Image { Source = "icon.png" };
            //    var topRight = new Xamarin.Forms.Image { Source = "icon.png" };
            //    var bottomLeft = new Xamarin.Forms.Image { Source = "icon.png" };
            //    var bottomRight = new Xamarin.Forms.Image { Source = "icon.png" };
            //    //var topLeft = new Label { Text = "Top Left" };
            //    //var topRight = new Label { Text = "Top Right" };
            //    //var bottomLeft = new Label { Text = "Bottom Left" };
            //    //var bottomRight = new Label { Text = "Bottom Right" };

            //    grid.RowDefinitions.Add(new Xamarin.Forms.RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //    grid.RowDefinitions.Add(new Xamarin.Forms.RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //    grid.ColumnDefinitions.Add(new Xamarin.Forms.ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //    grid.ColumnDefinitions.Add(new Xamarin.Forms.ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //    Content = grid;


            //}
        }

    

        private async void SaveTeam_Clicked(object sender, EventArgs e)
        {
            // await Navigation.PopAsync();
            await Navigation.PushAsync(new BattleScreen());
        }



        private async void AutoSelect_Clicked(object sender, EventArgs e)
        {
            //var myTest = CharacterPicker1.SelectedItem;
            //var myTest1 = CharacterPicker2.SelectedItem;
            //var myTest2 = CharacterPicker3.SelectedItem;
            //var myTest3 = CharacterPicker4.SelectedItem;
            //var myTest4 = CharacterPicker5.SelectedItem;
            //var myTest5 = CharacterPicker6.SelectedItem;

            await Navigation.PushAsync(new BattleScreen());
        }


    }


}
 


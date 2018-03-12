using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GameOfChallengers.Models;
using GameOfChallengers.ViewModels;

namespace GameOfChallengers.Views.Character
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterDetail : ContentPage
    {
        private CreatureDetailViewModel _viewModel;
        public Creature Data { get; set; }

        public CharacterDetail(CreatureDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        public CharacterDetail()
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
                ImageURI = "batman.jpeg",

            };

            _viewModel = new CreatureDetailViewModel(Data);
            BindingContext = _viewModel;
        }


        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditCharacter(_viewModel));
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new DeleteCharacter(_viewModel));

        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
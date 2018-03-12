using GameOfChallengers.ViewModels;
using GameOfChallengers.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameOfChallengers.Views.Character
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharactersPage : ContentPage
    {
        private CharactersViewModel _viewModel;
        public CharactersPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = CharactersViewModel.Instance;
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Creature;
            if (data == null)
                return;

            await Navigation.PushAsync(new CharacterDetail(new CreatureDetailViewModel(data)));

            // Manually deselect item.
            CharactersListView.SelectedItem = null;
        }

        private async void BuildTeam_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BuildTeamPage());
        }

        private async void AddCharacter_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewCharacter());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = null;

            if (ToolbarItems.Count > 0)
            {
                ToolbarItems.RemoveAt(0);
            }

            InitializeComponent();

            if (_viewModel.Dataset.Count == 0)
            {
                _viewModel.LoadDataCommand.Execute(null);
            }
            else if (_viewModel.NeedsRefresh())
            {
                _viewModel.LoadDataCommand.Execute(null);
            }

            BindingContext = _viewModel;
        }
    }
}
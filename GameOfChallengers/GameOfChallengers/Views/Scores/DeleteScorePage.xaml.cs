using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GameOfChallengers.Models;
using GameOfChallengers.ViewModels;

namespace GameOfChallengers.Views.Scores
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeleteScorePage : ContentPage
	{
        private ScoreDetailViewModel _viewModel;

        public Score Data { get; set; }

        public DeleteScorePage(ScoreDetailViewModel viewModel)
        {
            // Save off the item
            Data = viewModel.Data;

            InitializeComponent();

            // Set the data binding for the page
            BindingContext = _viewModel = viewModel;

        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteData", Data);

            // Remove CharacterDetailPage manualy
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            await Navigation.PopAsync();

        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
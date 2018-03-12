using GameOfChallengers.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using GameOfChallengers.ViewModels;
using GameOfChallengers.Services;
using GameOfChallengers.Models;


namespace GameOfChallengers.Views
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();

            name.Text = GameGlobals.PlayerName;
            BindingContext = name;


        }

        private async void StartGame_Command(object sender, EventArgs e)
        {
            GameGlobals.PlayerName = name.Text;
            await Navigation.PushAsync(new GameHomePage());
            CharactersViewModel.Instance.LoadDataCommand.Execute(null);
            MonstersViewModel.Instance.LoadDataCommand.Execute(null);
            ScoresViewModel.Instance.LoadDataCommand.Execute(null);
            ItemsViewModel.Instance.LoadDataCommand.Execute(null);
            
        }


    }
}

using System;
using System.Collections.Generic;

using Xamarin.Forms;
using GameOfChallengers.Views.Scores;
using GameOfChallengers.ViewModels;
using GameOfChallengers.Controllers;

namespace GameOfChallengers.Views.Battle
{
    public partial class AutoBattleScreen : ContentPage
    {
        //private TeamViewModel team;
        public AutoBattleScreen()
        {
            InitializeComponent();
            GameScoreController game = new GameScoreController();
            game.Start(true);
        }
        private async void Score_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScorePage());
        }
    }
}

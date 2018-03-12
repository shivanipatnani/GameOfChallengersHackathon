using System;
using System.Collections.Generic;

using Xamarin.Forms;
using GameOfChallengers.Views.Items;
using GameOfChallengers.Views.Monsters;
using GameOfChallengers.Views.Character;
using GameOfChallengers.Views.Scores;
using GameOfChallengers.Views.Battle;
using GameOfChallengers.Controllers;
using GameOfChallengers.ViewModels;

namespace GameOfChallengers.Views
{
    public partial class GameHomePage : ContentPage
    {
        private AboutPage aboutPageinstance;
        public GameHomePage()
        {
            InitializeComponent();

        }


        private async void Help_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new HelpPage());
        }

        private async void About_Clicked(object sender, EventArgs e)
        {
            if(aboutPageinstance == null)
            {
                aboutPageinstance = new AboutPage();
            }
            await Navigation.PushAsync(aboutPageinstance);
        }

        private async void Play_Command(object sender, EventArgs e)
        {            

            await Navigation.PushAsync(new BuildTeamPage());
        }

        private async void AutoBattle_Command(object sender, EventArgs e)
        {
            GameScoreController game = new GameScoreController();
            await Navigation.PushAsync(new ScoreDetailPage(new ScoreDetailViewModel(game.Start(true))));
        }

        private async void Characters_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CharactersPage());
        }

        private async void Monsters_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonsterPage());
        }

        private async void Items_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemsPage());
        }

        private async void Scores_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScorePage());
        }

    }
}

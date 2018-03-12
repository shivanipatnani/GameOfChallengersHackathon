using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GameOfChallengers.ViewModels;
using GameOfChallengers.Views.Scores;


namespace GameOfChallengers.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GameOver : ContentPage
	{
		public GameOver ()
		{
			InitializeComponent ();
		}

        private async void ViewScore_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScorePage());
        }

    }
}
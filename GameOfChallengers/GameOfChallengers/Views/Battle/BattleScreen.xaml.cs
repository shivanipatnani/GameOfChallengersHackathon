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
using GameOfChallengers.Controllers;

namespace GameOfChallengers.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BattleScreen : ContentPage
	{
        private TeamViewModel _viewModel;
        public Creature Data { get; set; }
		public BattleScreen ()
		{
            
            //BattleController bc = new BattleController(_viewModel,a);
			InitializeComponent ();
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
           
            _viewModel = new TeamViewModel();
           // BindingContext = a;
		}

        public void BattleMessages(string message)
        {
            L1.Text = message ;
            //add message to some kind of list that can be displayed on the view
        }

        private async void DeleteTeamMember(object sender, EventArgs e)
        {

            MessagingCenter.Send(this, "DeleteData", Data);
            await Navigation.PopAsync();

        }
        private void ClickedZeroZero(object sender, EventArgs e)
        {
            zero.Text = "M1"; 
        }

        private void Start_clicked(object sender, EventArgs e)
        {
            L1.Text = "C1 hit M1\n M1 is dead \n C1 get XP of 100";
        }

        private async void End_clicked(object sender, EventArgs e)
        {
           // L1.Text = "End the Game";
            await Navigation.PushAsync(new BattleOver());
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GameOfChallengers.Models;

namespace GameOfChallengers.Views.Scores
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewScorePage : ContentPage
	{
        public Score Data { get; set; }

        // Constructor for the page, will create a new blank character
        public NewScorePage()
        {
            InitializeComponent();

            Data = new Score
            {
                Name = "Player name",
                Id = Guid.NewGuid().ToString(),
                FinalScore = 0,
                Round = 1,
                Date = DateTime.Now,
                TotalXP = 10,
                Turns = 10,
                Auto = true,

            };

            BindingContext = this;
        }

        // Respond to the Save click
        // Send the add message to so it gets added...
        private async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddData", Data);
            await Navigation.PopAsync();
        }

        // Cancel and go back a page in the navigation stack
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
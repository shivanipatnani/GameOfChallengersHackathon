using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GameOfChallengers.Models;
using GameOfChallengers.Views.Items;

namespace GameOfChallengers.Views.Character
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCharacter : ContentPage
    {
        public Creature Data { get; set; }

        // Constructor for the page, will create a new blank character
        public NewCharacter()
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

        private async void AddItems_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InventoryPage());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameOfChallengers.ViewModels;
using Xamarin.Forms;
using GameOfChallengers.Models;
using GameOfChallengers.Controllers;

namespace GameOfChallengers.Views.Items
{
    public partial class AssignItemPage : ContentPage
    {
        private TeamViewModel _viewModel;
        private DroppedItemViewModel _viewModel1;
        public ObservableCollection<Item> _Dataset { get; set; }
        public Creature Data { get; set; }
        public ObservableCollection<Creature> Dataset { get; set; }

        public AssignItemPage()
        {
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
                       
            BindingContext = _viewModel = TeamViewModel.Instance;
        }
    }
}

using GameOfChallengers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfChallengers.ViewModels
{
    public class CreatureDetailViewModel : BaseViewModel
    {
        public Creature Data { get; set; }
        public CreatureDetailViewModel(Creature data = null)
        {
            Title = data?.Name;
            Data = data;
        }
    }
}

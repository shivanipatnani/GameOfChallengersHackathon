using GameOfChallengers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfChallengers.ViewModels
{
    public class ScoreDetailViewModel : BaseViewModel
    {
        public Score Data { get; set; }
        public ScoreDetailViewModel(Score data = null)
        {
            Title = data?.Name;
            Data = data;
        }
    }
}
using GameOfChallengers.Models;
using GameOfChallengers.ViewModels;
using GameOfChallengers.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GameOfChallengers.Controllers
{
    public class GameScoreController
    {
        int round = 0;
        Score GameScore;//this is the Score for this game
        TeamViewModel Team;//this is the team of six characters for this game

        public GameScoreController()
        {
            GameScore = new Score();
           
            TeamViewModel.Instance.LoadData();
            TeamViewModel Team = TeamViewModel.Instance;
        }

        public Score Start(bool auto)
        {
            TeamViewModel Team = TeamViewModel.Instance;
            BattleController battle = new BattleController();
            while (Team.Dataset.Count > 0)
            {
                round++;
                if (auto)
                {
                    battle.SetBattleController(round);
                    GameScore = battle.AutoBattle(GameScore);
                    GameScore.Auto = true;
                }
                else
                {
                    GameScore.Auto = false;
                    battle.Battle();
                }
            }
            return ReportScore();
        }
        
        public Score ReportScore()
        {
            //the final score will be total XP + # of turns + # of monsters killed
            //this method will report the final score as well as the "Battle History" metadata
            //the metadata is the variables at the top of the page as well as the characters' stats

            //load GameScore
            GameScore.Name = GameGlobals.PlayerName;
            GameScore.Date = DateTime.Now;//to set the time to when the game was finished
            GameScore.Round = round;
            //GameScore.Team.AddRange(Team);
            GameScore.FinalScore = GameScore.TotalXP;
            MessagingCenter.Send(this, "AddData", GameScore);
            return GameScore;
        }
    }
}

using GameOfChallengers.Models;
using GameOfChallengers.ViewModels;
using GameOfChallengers.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

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
            int Miracle = 1;
            while (Team.Dataset.Count > 0)
            {
                round++;
                int Potions = 6;
                int Volcano = 20;
                string message = "";
                Debug.WriteLine("New Round " + round.ToString());
                foreach(var character in Team.Dataset)
                {
                    character.CanFocusAttack = true;
                }
                if(GameGlobals.EnableRandomBadThings == true)
                {
                    int dateSeed = DateTime.Now.Millisecond;
                    Random rand = new Random(dateSeed);
                    Volcano = rand.Next(1, 21);
                    Volcano = 4;
                    if (GameGlobals.DisableRandomNumbers)
                    {
                        Volcano = 20;
                    }
                    if(Volcano == 1)
                    {
                        foreach (var character in Team.Dataset)
                        {
                            character.CurrHealth = character.MaxHealth;
                        }
                        message = "Volcano erupted, characters feel stronger";
                    }
                    if(Volcano == 2)
                    {
                        foreach (var character in Team.Dataset)
                        {
                            double damage = character.CurrHealth * .2;
                            character.CurrHealth -= (int)Math.Ceiling(damage);
                            if(character.CurrHealth < 1)
                            {
                                character.CurrHealth = 1;
                            }
                        }
                        message = "Volcano erupted, characters are sickened by the fumes";
                    }
                    if(Volcano > 4)
                    {
                        message = "Volcano did not explode";
                    }
                }
                if (auto)
                {
                    battle.SetBattleController(round);
                    if (Volcano == 3)
                    {
                        foreach (var monster in battle.CurrMonsters.Dataset)
                        {
                            double damage = monster.CurrHealth * .2;
                            monster.CurrHealth -= (int)Math.Ceiling(damage);
                            if (monster.CurrHealth < 1)
                            {
                                monster.CurrHealth = 1;
                            }
                        }
                        message = "Volcano erupted, falling ash hurts the monsters";
                    }
                    if(Volcano == 4)
                    {
                        MonsterController mc = new MonsterController();
                        Creature strongestMonster = null;
                        int strongest = 0;
                        foreach (var monster in battle.CurrMonsters.Dataset)
                        {
                            int newStrongest = monster.CurrHealth + mc.GetBaseAttack(monster);
                            if (strongest < newStrongest)
                            {
                                strongest = newStrongest;
                                strongestMonster = monster;
                            }
                        }
                        
                        message = "Volcano erupted, " + strongestMonster.Name + " ran to watch";
                        battle.CurrMonsters.Dataset.Remove(strongestMonster);
                        battle.TurnOrder.Remove(strongestMonster);
                        battle.GameBoardRemove(strongestMonster);
                    }
                    if(GameGlobals.EnableRandomBadThings == true)
                    {
                        Debug.WriteLine(message);
                    }
                    GameScore = battle.AutoBattle(GameScore, round, Potions, Miracle);
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

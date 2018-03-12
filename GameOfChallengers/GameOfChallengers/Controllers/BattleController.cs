﻿using GameOfChallengers.Models;
using GameOfChallengers.Services;
using GameOfChallengers.ViewModels;
using GameOfChallengers.Views.Battle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GameOfChallengers.Views.Battle;

namespace GameOfChallengers.Controllers
{
   public class BattleController
    {
        MonstersListViewModel CurrMonsters;
        TeamViewModel team;
        public List<Creature> TurnOrder = new List<Creature>();
        public List<Item> ItemPool = new List<Item>();
        public Creature[,] GameBoard = new Creature[3, 6];
        //there will be one controller per type and the specific creature will be passed in to the controller methods
        CharacterController CC = new CharacterController();
        MonsterController MC = new MonsterController();
        int turns = 0;
        int totalXP = 0;
        //string Text = null;
        public BattleController()
        {
            CurrMonsters = MonstersListViewModel.Instance;
            team = TeamViewModel.Instance;
        }

        public void SetBattleController(int round)
        {
            CurrMonsters.setRound(round);
            CurrMonsters.setMonsters();
            TurnOrder = GetTurnOrder();
            InitializeGameBoard();
        }

        public List<Creature> GetTurnOrder()
        {
            //this will get the list that will be cycled through for this round to choose whose turn it is
            //the speed will be found by adding the Speed data from the Creature with all of the boosts from any items (GetBaseSpeed())
            //ties in speed will be broken it the following way:
            //highest level -> highest xp -> character before monster -> alphabetic by name -> first in list order
            List<Creature> turnOrder = new List<Creature>();
            for (int i = 0; i < TeamViewModel.Instance.Dataset.Count; i++)
            {
                turnOrder.Add(team.Dataset[i]);
            }
            for (int i = 0; i < CurrMonsters.Dataset.Count; i++)
            {
                turnOrder.Add(CurrMonsters.Dataset[i]);
            }


            CompareByAllCriteria listComparer = new CompareByAllCriteria();
            turnOrder.Sort(listComparer);
            return turnOrder;

        }

        public void Battle()
        {
            //this will run the turns (using the turn controller) in a loop until either all the team is dead or all the monsters are
        }

        public Score AutoBattle(Score score)
        {
            //without asking the player for input
            //this will run the turns in a loop until either all the team is dead or all the monsters are
            BattleScreen screen = new BattleScreen();
            string message;
            while (CurrMonsters.Dataset.Count > 0)
            {
                if(team.Dataset.Count <= 0)
                {
                    break;
                }
                for (int i = 0; i < TurnOrder.Count; i++)
                {
                   

                    message = "Battle Start" + " Characters :" + team.Dataset.Count;
                    Debug.WriteLine(message);
                    screen.BattleMessages(message);

                    //.WriteLine(Text);
                    message = "Battle Start" + " Monsters :" + CurrMonsters.Dataset.Count;
                    Debug.WriteLine(message);
                    screen.BattleMessages(message);


                    TurnController turn = new TurnController();
                    turns++;
                    if (TurnOrder[i].Type == 0)
                    {
                        
                        Creature character = TurnOrder[i];
                        //int loc = GetNewLoc(character, GameBoard);
                        //GameBoard = turn.Move(character, loc, GameBoard);
                        message = "New Turn :" + TurnOrder[i];
                        Debug.WriteLine(message);
                        screen.BattleMessages(message);

                        Creature target = AutoTarget(character);//get a monster target for the character
                        if(target == null)
                        {
                            
                            break;
                           
                        }
                        if (!CanHit(character, target))
                        {
                            continue;
                        }
                        bool hit = turn.Attack(character, target);
                        if (hit)
                        {
                            int damageToDo = turn.DamageToDo(character);
                            int xpToGive = MC.GiveXP(target, damageToDo);
                            totalXP += xpToGive;
                            CC.TestForLevelUp(character, xpToGive);
                            bool monsterAlive;
                            MC.TakeDamage(target, damageToDo);
                            monsterAlive = target.Alive;

                            if (monsterAlive == false)
                            {
                                message = "Monster dead ";
                                Debug.WriteLine(message);
                                screen.BattleMessages(message);

                                ItemPool.AddRange(MC.DropItems(target));
                                message = "Items Dropped :" + MC.DropItems(target);
                                Debug.WriteLine(message);
                                screen.BattleMessages(message);

                                TurnOrder.Remove(target);
                                //score.TotalMonstersKilled.Add(target);
                                CurrMonsters.Dataset.Remove(target);
                                GameBoardRemove(target);

                                message = "Monster Removed :" + CurrMonsters.Dataset.Remove(target);
                                Debug.WriteLine(message);
                                screen.BattleMessages(message);

                            }
                        }

                    }
                    else
                    {
                        Creature monster = TurnOrder[i];
                        //int loc = GetNewLoc(monster, GameBoard);
                        //GameBoard = turn.Move(monster, loc, GameBoard);
                        Creature target = AutoTarget(monster);//get a character target for the monster
                        if (target == null)
                        {
                            break;
                        }
                        if (!CanHit(monster, target))
                        {
                            continue;
                        }
                        bool hit = turn.Attack(monster, target);
                        if (hit)
                        {
                            int damageToDo = turn.DamageToDo(monster);
                            bool characterAlive = CC.TakeDamage(target, damageToDo);
                            if (!characterAlive)
                            {
                                message = "Character dead ";
                                Debug.WriteLine(message);
                                screen.BattleMessages(message);

                                ItemPool.AddRange(CC.DropItems(target));
                                message = "Items Dropped :" + CC.DropItems(target);
                                Debug.WriteLine(message);
                                screen.BattleMessages(message);

                                TurnOrder.Remove(target);
                                //add dead character to the score list
                                team.Dataset.Remove(target);
                                GameBoardRemove(target);

                                message = "Character Removed :" + team.Dataset.Remove(target);
                                Debug.WriteLine(message);
                                screen.BattleMessages(message);
                            }
                        }
                    }
                }
            }
            score.Turns += turns;
            score.TotalXP += totalXP;
            message =
                "Battle Ended" +
                " Total Experience :" + totalXP +

                " Turns :" + turns +
                " Monster Kills :" + CurrMonsters;
            Debug.WriteLine(message);
            screen.BattleMessages(message);

            return score;
        }

        public Creature AutoTarget(Creature self)//***needs to get the closest enemy(targetType)***
        {
           
            return GetClosestEnemy(self);
            //return c;//return a creature with c.Type == targetType
        }

        public bool CanHit(Creature creature1, Creature creature2)
        {
            int dist = GetDistance(creature1, creature2);
            List<string> itemIds = creature1.GetHandIDs();
            int range = 0;
            for (int i = 0; i < itemIds.Count; i++)
            {
                var items = ItemsViewModel.Instance.Dataset;
                var item = items.Where(a => a.Id == itemIds[i]).FirstOrDefault();
                if (item.Range > range)
                {
                    range = item.Range;
                }
            }
            if (range >= dist)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AssignItems()
        {

        }

        public void InitializeGameBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    GameBoard[i, j] = null;
                }
            }
            for (int i = 0; i < CurrMonsters.Dataset.Count(); i++)
            {
                GameBoard[0, i] = CurrMonsters.Dataset[i];
            }
            for(int i=0; i<team.Dataset.Count(); i++)
            {
                GameBoard[2, i] = team.Dataset[i];
            }
        }

        

        //Check if this is required
        public int GetNewLoc(Creature creature, Creature[,] GameBoard)
        {
            //find the creature move it one place(or more) closer to the first monster/character found
            int newloc = 0;
            if (creature.Type == 0)
            {


            }
            else
            {

            }
            return newloc;
        }

        //this will return the closest enemy creature to the creature passed in
        private Creature GetClosestEnemy(Creature creature)
        {
            CreatureLocInfo info = GetLocInfo(creature);
          
            return GetClosestEnemy(info);
        }

        private void GameBoardRemove(Creature creature)
        {
            CreatureLocInfo info = GetLocInfo(creature);
            GameBoard[info.row, info.col] = null;
        }
        private CreatureLocInfo GetLocInfo(Creature creature)
        {
            CreatureLocInfo info = new CreatureLocInfo();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if(GameBoard[i, j] != null)
                    {
                        if (GameBoard[i, j].Id == creature.Id)
                        {
                            info.ID = creature.Id;
                            info.row = i;
                            info.col = j;
                            info.type = creature.Type;

                        }
                    }
                }
            }
            return info;
        }

        private Creature GetClosestEnemy(CreatureLocInfo info)
        {
            Creature foundEnemy = null;
            //enemy away distance is defined as: how many column away + how many row away       ***maybe use distance formula?***

            int distance = 0,minDist = 50;
            // List<CreatureLocInfo> distance = new List<CreatureLocInfo>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if(GameBoard[i, j] != null && ((GameBoard[i, j].Type) == (1- info.type)))
                    {
                        distance = (Math.Abs(info.row - i) + (Math.Abs(info.col - j)));
                        if (minDist >= distance)
                        {
                            minDist = distance;
                            foundEnemy = GameBoard[i, j];
                        }
                    }
                }

            }

            return foundEnemy;
        }

        public int GetDistance(Creature creature1, Creature creature2)
        {
            
            CreatureLocInfo info1 = GetLocInfo(creature1);
            CreatureLocInfo info2 = GetLocInfo(creature2);

            return (Math.Abs(info1.row - info2.row) + (Math.Abs(info1.col - info2.col)));
            //needs to return the distance between creature1 and creature2 and ensure that if they are in neighboring squares it will be one

        }
    }

    public class CreatureLocInfo 
    {
        public string ID;
        public int row;
        public int col;
        public int type;
    }

    public class CompareByAllCriteria : IComparer<Creature>
    {
        public int Compare(Creature x, Creature y)
        {
            int comparison = x.Speed.CompareTo(y.Speed);
            if (comparison != 0)
            {
                return comparison;
            }

            comparison = x.Level.CompareTo(y.Level);
            if (comparison != 0)
            {
                return comparison;
            }

            comparison = x.XP.CompareTo(y.XP);
            if (comparison != 0)
            {
                return comparison;
            }

            if (x.Type < y.Type)
            {
                return 1;
            }
            else
            {
                if (x.Type > y.Type)
                    return -1;
            }

            comparison = string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            if (comparison != 0)
            {
                return comparison;
            }
            return 0;
        }
    }

    public class CompareByLevel : IComparer<Creature>
    {
        public int Compare(Creature x, Creature y) => x.Level.CompareTo(y.Level);
    }

    public class CompareByXP : IComparer<Creature>
    {
        public int Compare(Creature x, Creature y) => x.XP.CompareTo(y.XP);
    }

    public class CompareByName : IComparer<Creature>
    {
        public int Compare(Creature x, Creature y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }
}

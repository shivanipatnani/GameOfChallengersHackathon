using GameOfChallengers.Models;
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
        public MonstersListViewModel CurrMonsters;
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

        public Score AutoBattle(Score score , int Potions, int miracle)
        {
            //without asking the player for input
            //this will run the turns in a loop until either all the team is dead or all the monsters are
            BattleScreen screen = new BattleScreen();
            string message;
            message = "Battle Start" + " Characters :" + team.Dataset.Count;
            Debug.WriteLine(message);
            message = "Battle Start" + " Monsters :" + CurrMonsters.Dataset.Count;
            Debug.WriteLine(message);
            while (CurrMonsters.Dataset.Count > 0)
            {
               
                
                if(team.Dataset.Count <= 0)
                {
                    break;
                }
                for (int i = 0; i < TurnOrder.Count; i++)
                {



                    //screen.BattleMessages(message);

                    //.WriteLine(Text);

                    //screen.BattleMessages(message);

                    message = "New Turn :" + TurnOrder[i].Name;
                    Debug.WriteLine(message);
                    screen.BattleMessages(message);

                    TurnController turn = new TurnController();
                    turns++;
                    if (TurnOrder[i].Type == 0)
                    {
                        
                        Creature character = TurnOrder[i];
                        //int loc = GetNewLoc(character, GameBoard);
                        //GameBoard = turn.Move(character, loc, GameBoard);
                       
                        Creature target = AutoTarget(character);//get a monster target for the character
                        if(target == null)
                        {
                            
                            break;
                           
                        }
                        if (!CanHit(character, target))
                        {
                            continue;
                        }

                        int hit = turn.Attack(character, target);
                        //Debug.WriteLine(hit.ToString());
                        if(GameGlobals.FocusedAttack == true)
                        {
                            bool hasItem = false;
                            List<string> ids = character.GetItemIDs();
                            if (ids.Count > 0)
                            {
                                hasItem = true;
                            }
                            bool wantsTo = false;
                            if (target.CurrHealth > (1 * turn.DamageToDo(character)))
                            {
                                wantsTo = true;
                            }
                            if (character.CanFocusAttack && hasItem && wantsTo)
                            {
                                hit = 3;
                                character.CanFocusAttack = false;
                            }
                        }
                        if (GameGlobals.AllowRoundHealing)
                        {
                            if(character.CurrHealth < (character.MaxHealth * .2))
                            {
                                if (Potions > 0)
                                {
                                    character.CurrHealth = character.MaxHealth;
                                    Potions--;
                                    Debug.WriteLine("Potion used, Potions Left " + Potions);
                                    hit = 0;
                                }
                            }
                        }
                        if (hit > 0)
                        {

                            int damageToDo = turn.DamageToDo(character);
                            if(hit == 2)//critical hit
                            {
                                damageToDo = damageToDo * 2;
                                Debug.WriteLine(character.Name + " critical hit " + target.Name);
                            }
                            if (hit == 3)//focused hit
                            {
                                Item lowValue = null;
                                int lowest = 1000;
                                List<string> itemIds = character.GetItemIDs();
                                var items = ItemsViewModel.Instance.Dataset;
                                for(int j=0; j<itemIds.Count; j++)
                                {
                                    var currItem = items.Where(a => a.Id == itemIds[j]).FirstOrDefault();
                                    int newLowest = currItem.Value;
                                    if (lowest > newLowest)
                                    {
                                        lowest = newLowest;
                                        lowValue = currItem;
                                    }
                                }
                                character.DropOneItem(lowValue.Id);
                                damageToDo = damageToDo * 10;
                                bool monsterAliveAfterFocus;
                                MC.TakeDamage(target, damageToDo);
                                Debug.WriteLine(character.Name + " performed a Focused Attack and lost item " + lowValue.Name);
                                monsterAliveAfterFocus = target.Alive;
                                if (monsterAliveAfterFocus == false)
                                {
                                    message = "Monster dead ";
                                    Debug.WriteLine(message);
                                    screen.BattleMessages(message);

                                    TurnOrder.Remove(target);
                                    //score.TotalMonstersKilled.Add(target);
                                    CurrMonsters.Dataset.Remove(target);
                                    GameBoardRemove(target);

                                    message = "Monster Removed :" + target.Name;
                                    Debug.WriteLine(message);
                                    screen.BattleMessages(message);

                                }
                                continue;
                            }
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
                                message = "Items Dropped :" + ItemPool.Count.ToString();
                                Debug.WriteLine(message);
                                screen.BattleMessages(message);

                                TurnOrder.Remove(target);
                                //score.TotalMonstersKilled.Add(target);
                                CurrMonsters.Dataset.Remove(target);
                                GameBoardRemove(target);

                                message = "Monster Removed :" + target.Name;
                                Debug.WriteLine(message);
                                screen.BattleMessages(message);

                            }
                        }
                        if(hit == -1)
                        {
                            Random rand = new Random();
                            int roll = rand.Next(1, 11);
                            if (GameGlobals.DisableRandomNumbers)
                            {
                                roll = 10;
                            }
                            if(roll == 1)
                            {
                                character.RHandItemID = null;
                            }else if(roll >= 2 && roll <= 4)
                            {
                                if(character.RHandItemID != null)
                                {
                                    var items = ItemsViewModel.Instance.Dataset;
                                    var item = items.Where(a => a.Id == character.RHandItemID).FirstOrDefault();
                                    if(item != null)
                                    {
                                        ItemPool.Add(item);
                                    }
                                    character.RHandItemID = null;
                                }
                                
                            }
                            else if(roll == 5 || roll == 6)
                            {
                                var allItems = character.GetItemIDs();//get all of the character's items' ids
                                if(allItems.Count > 0)
                                {
                                    int index = rand.Next(allItems.Count);//get a random index for getting an item id
                                    if (GameGlobals.DisableRandomNumbers)
                                    {
                                        index = 0;
                                    }
                                    if(allItems.Count > index)//check for a bad index
                                    {
                                        string itemID = allItems[index];
                                        var items = ItemsViewModel.Instance.Dataset;
                                        var item = items.Where(a => a.Id == itemID).FirstOrDefault();
                                        if (item != null)//find the item and make sure it is safe to add to the item pool
                                        {
                                            ItemPool.Add(item);
                                        }
                                        character.DropOneItem(itemID);
                                    }
                                    
                                }
                                
                            }
                            Debug.WriteLine("Critical miss, case " + roll.ToString());
                            
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
                        
                        int hit = turn.Attack(monster, target);
                        //Debug.WriteLine(hit.ToString());
                        if (hit > 0)
                        {
                            int damageToDo = turn.DamageToDo(monster);
                            if (hit == 2)
                            {
                                damageToDo = damageToDo * 2;
                                Debug.WriteLine(monster.Name + " critical hit " + target.Name);
                            }
                            bool characterAlive = CC.TakeDamage(target, damageToDo);
                            if(GameGlobals.MiracleMax == true)
                            {
                                if (!characterAlive && miracle > 0)
                                {
                                    target.Alive = true;
                                    target.CurrHealth = target.MaxHealth;
                                    miracle--;
                                    Debug.WriteLine("Miracle Max has revived " + target.Name);
                                    GameGlobals.MiracleMax = false;
                                    characterAlive = target.Alive;
                                }
                            }
                            if (!characterAlive)
                            {
                                message = "Character dead ";
                                Debug.WriteLine(message);
                                screen.BattleMessages(message);

                                ItemPool.AddRange(CC.DropItems(target));
                                message = "Items Dropped :" + ItemPool.Count.ToString();
                                Debug.WriteLine(message);
                                screen.BattleMessages(message);

                                TurnOrder.Remove(target);
                                //add dead character to the score list
                                team.Dataset.Remove(target);
                                GameBoardRemove(target);

                                message = "Character Removed :" + target.Name;
                                Debug.WriteLine(message);
                                screen.BattleMessages(message);
                            }
                        }
                        if (hit == -1)
                        {
                            Random rand = new Random();
                            int roll = rand.Next(1, 11);
                            if (GameGlobals.DisableRandomNumbers)
                            {
                                roll = 10;
                            }
                            if (roll == 1)
                            {
                                monster.RHandItemID = null;
                            }
                            else if (roll >= 2 && roll <= 4)
                            {
                                if (monster.RHandItemID != null)
                                {
                                    var items = ItemsViewModel.Instance.Dataset;
                                    var item = items.Where(a => a.Id == monster.RHandItemID).FirstOrDefault();
                                    if (item != null)
                                    {
                                        ItemPool.Add(item);
                                    }
                                    monster.RHandItemID = null;
                                }
                                else
                                {
                                    var items = ItemsViewModel.Instance.Dataset;
                                    int randItem = rand.Next(items.Count);//find the random item to add to the pool
                                    if (GameGlobals.DisableRandomNumbers)
                                    {
                                        randItem = 0;
                                    }
                                    Item item = new Item();
                                    if (items.Count > randItem)
                                    {
                                        item.Update(items[randItem]);
                                    }
                                    
                                    if (item != null)
                                    {
                                        ItemPool.Add(item);
                                    }
                                }

                            }
                            else if (roll == 5 || roll == 6)
                            {
                                var allItems = monster.GetItemIDs();//get all of the character's items' ids
                                if (allItems.Count > 0)
                                {
                                    int index = rand.Next(allItems.Count);//get a random index for getting an item id
                                    if (GameGlobals.DisableRandomNumbers)
                                    {
                                        index = 0;
                                    }
                                    if (allItems.Count > index)//check for a bad index
                                    {
                                        string itemID = allItems[index];
                                        var items = ItemsViewModel.Instance.Dataset;
                                        var item = items.Where(a => a.Id == itemID).FirstOrDefault();
                                        if (item != null)//find the item and make sure it is safe to add to the item pool
                                        {
                                            ItemPool.Add(item);
                                        }
                                        monster.DropOneItem(itemID);
                                    }

                                }
                                if(allItems.Count == 0)
                                {
                                    var items = ItemsViewModel.Instance.Dataset;
                                    int randItem = rand.Next(items.Count);//find the random item to add to the pool
                                    if (GameGlobals.DisableRandomNumbers)
                                    {
                                        randItem = 0;
                                    }
                                    Item item = new Item();
                                    if (items.Count > randItem)
                                    {
                                        item.Update(items[randItem]);
                                    }

                                    if (item != null)
                                    {
                                        ItemPool.Add(item);
                                    }
                                }

                            }
                            Debug.WriteLine("Critical miss, case " + roll.ToString());

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

        public void GameBoardRemove(Creature creature)
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

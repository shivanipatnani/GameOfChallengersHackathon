using GameOfChallengers.Models;
using GameOfChallengers.Services;
using GameOfChallengers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfChallengers.Controllers
{
    class MonsterController
    {
        public List<Item> DropItems(Creature monster)
        {
            //drop a random number of items when dead
            List<Item> Dropped = new List<Item>();
            int dateSeed = DateTime.Now.Millisecond;
            Random rand = new Random(dateSeed);
            int numOfItems = rand.Next(4);//drop 0, 1, 2, or 3(all) of its items
            List<string> itemIds = monster.GetItemIDs();
            var items = ItemsViewModel.Instance.Dataset;
            for (int i=0; i<numOfItems; i++)
            {
                var item = items.Where(a => a.Id == itemIds[i]).FirstOrDefault();
                Dropped.Add(item);
            }
            return Dropped;
        }
        
        public int GetBaseAttack(Creature monster)
        {
            List<string> itemIds = monster.GetItemIDs();
            int baseAttack = 0;//this will be based on the monster stats + any item boosts
            baseAttack += monster.Attack;
            for(int i=0; i<itemIds.Count; i++)
            {
                var items = ItemsViewModel.Instance.Dataset;
                var item = items.Where(a => a.Id == itemIds[i]).FirstOrDefault();
                if (item.Attribute == AttributeEnum.Attack)
                {
                    baseAttack += item.Value;
                }
            }
            return baseAttack;
        }

        public int GetBaseDamage(Creature monster)
        {
            List<string> itemIds = monster.GetHandIDs();
            int baseDamage = 0;//this will be based on the weapon stats
            for (int i = 0; i < itemIds.Count; i++)
            {
                var items = ItemsViewModel.Instance.Dataset;
                var item = items.Where(a => a.Id == itemIds[i]).FirstOrDefault();
                if (item.Attribute == AttributeEnum.Attack)
                {
                    baseDamage += item.Value;
                }
            }
            return baseDamage;
        }

        public int GetBaseSpeed(Creature monster)
        {
            List<string> itemIds = monster.GetItemIDs();
            int baseSpeed = 0;//this will be based on the monster stats + any item boosts
            baseSpeed += monster.Speed;
            for (int i = 0; i < itemIds.Count; i++)
            {
                var items = ItemsViewModel.Instance.Dataset;
                var item = items.Where(a => a.Id == itemIds[i]).FirstOrDefault();
                if (item.Attribute == AttributeEnum.Speed)
                {
                    baseSpeed += item.Value;
                }
            }
            return baseSpeed;
        }

        public int GetBaseDefense(Creature monster)
        {
            List<string> itemIds = monster.GetItemIDs();
            int baseDefense = 0;//this will be based on the monster stats including item boosts
            baseDefense += monster.Defense;
            for (int i = 0; i < itemIds.Count; i++)
            {
                var items = ItemsViewModel.Instance.Dataset;
                var item = items.Where(a => a.Id == itemIds[i]).FirstOrDefault();
                if (item.Attribute == AttributeEnum.Defense)
                {
                    baseDefense += item.Value;
                }
            }
            return baseDefense;
        }

        public int GiveXP(Creature monster, int damageGiven)
        {
            //this will calculate and return the amount of XP to be transferred on a hit and -= that much from the monster
            double percentToGive = ((double)damageGiven / (double)monster.CurrHealth);
            if(percentToGive > 1.0)
            {
                percentToGive = 1.0;
            }
            int XPToGive = (int)(monster.XP * percentToGive);
            monster.XP -= XPToGive;
            return XPToGive;
        }

        public bool TakeDamage(Creature monster, int amount)
        {
            //monster takes damage and checks for death
            monster.CurrHealth -= amount;
            if(monster.CurrHealth <= 0)
            {
                monster.Alive = false;
            }
            return monster.Alive;
        }

        
    }
}

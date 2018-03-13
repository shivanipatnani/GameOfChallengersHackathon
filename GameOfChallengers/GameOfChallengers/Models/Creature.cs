using System;
using System.Collections.Generic;
using SQLite;
using GameOfChallengers.Controllers;
using GameOfChallengers.Models;
using Newtonsoft.Json;
using GameOfChallengers.ViewModels;


namespace GameOfChallengers.Models
{
    public class Creature
    {
        [PrimaryKey]
        public string Id { get; set; }// unique id for each character and monster
        public int Type { get; set; }// character(0) or monster(1)
        public string Name { get; set; }// 25 char max
        public int Level { get; set; }// 1-20
        public bool OnTeam { get; set; }// if it is a character, is it on the team
        public int Attack { get; set; }// creature's base attack stat
        public int Defense { get; set; }// creature's base defense stat
        public int Speed { get; set; }// creature's base speed stat
        public int XP { get; set; }// 0-355000
        public int MaxHealth { get; set; }// creature's current max health
        public int CurrHealth { get; set; }// creature's current health
        public bool Alive { get; set; }// 1 is alive, 0 is dead
        public int Loc { get; set; }// location on game board
        public string ImageURI { get; set; }// image to be inserted
        public AttributeBase Attribute { get; set; }
        public string AttributeString { get; set; }
        public string UniqueItem { get; set; }
        public int Damage { get; set; }
        public bool CanFocusAttack { get; set; }


        //public CreatureInventory CInventory { get; set; }// inventory of items equipped to this creature
        public string HeadItemID { get; set; }
        public string BodyItemID { get; set; }
        public string FeetItemID { get; set; }
        public string LHandItemID { get; set; }
        public string RHandItemID { get; set; }
        public string LFingerItemID { get; set; }
        public string RFingerItemID { get; set; }


        public Creature()
        {
            //                      ***need id preset?***
            Alive = true;
            Level = 1;
            XP = 0;
            OnTeam = false;
            HeadItemID = null;
            BodyItemID = null;
            FeetItemID = null;
            LHandItemID = null;
            RHandItemID = "bow";//              ***TEMP***
            LFingerItemID = null;
            RFingerItemID = null;
            Attribute = new AttributeBase();
            CanFocusAttack = true;

        }

        public void Update(Creature newData)
        {
            if (newData == null)
            {
                return;
            }

            // Update all the fields in the Data, except for the Id
            Type = newData.Type;
            Name = newData.Name;
            Level = newData.Level;
            Attack = newData.Attack;
            Defense = newData.Defense;
            Speed = newData.Speed;
            XP = newData.XP;
            MaxHealth = newData.MaxHealth;
            CurrHealth = newData.CurrHealth;
            Alive = newData.Alive;
            Loc = newData.Loc;
            HeadItemID = newData.HeadItemID;
            BodyItemID = newData.BodyItemID;
            FeetItemID = newData.FeetItemID;
            LHandItemID = newData.LHandItemID;
            RHandItemID = newData.RHandItemID;
            LFingerItemID = newData.LFingerItemID;
            RFingerItemID = newData.RFingerItemID;
            ImageURI = newData.ImageURI;
            AttributeString = newData.AttributeString;
            Attribute = new AttributeBase(newData.AttributeString);
            CanFocusAttack = newData.CanFocusAttack;

        }

        public List<string> GetItemIDs()
        {
            List<string> itemIds = new List<string>();
            if (HeadItemID != null)
            {
                itemIds.Add(HeadItemID);
            }
            if (BodyItemID != null)
            {
                itemIds.Add(BodyItemID);
            }
            if (FeetItemID != null)
            {
                itemIds.Add(FeetItemID);
            }
            if (LHandItemID != null)
            {
                itemIds.Add(LHandItemID);
            }
            if (RHandItemID != null)
            {
                itemIds.Add(RHandItemID);
            }
            if (LFingerItemID != null)
            {
                itemIds.Add(LFingerItemID);
            }
            if (RFingerItemID != null)
            {
                itemIds.Add(RFingerItemID);
            }
            return itemIds;
        }

        public List<string> GetHandIDs()
        {
            List<string> itemIds = new List<string>();
            if (LHandItemID != null)
            {
                itemIds.Add(LHandItemID);
            }
            if (RHandItemID != null)
            {
                itemIds.Add(RHandItemID);
            }
            return itemIds;
        }

        public void DropOneItem(string id)
        {
            if (HeadItemID == id)
            {
                HeadItemID = null;
            }
            if (BodyItemID == id)
            {
                BodyItemID = null;
            }
            if (FeetItemID == id)
            {
                FeetItemID = null;
            }
            if (LHandItemID == id)
            {
                LHandItemID = null;
            }
            if (RHandItemID == id)
            {
                RHandItemID = null;
            }
            if (LFingerItemID == id)
            {
                LFingerItemID = null;
            }
            if (RFingerItemID == id)
            {
                RFingerItemID = null;
            }
        }

        

        public string FormatOutput()
        {
            var myReturn = string.Empty;
            var UniqueOutput = "None";
            var myUnique = ItemsViewModel.Instance.GetItem(UniqueItem);
            if (myUnique != null)
            {
                UniqueOutput = myUnique.FormatOutput();
            }

            myReturn += Name;
            myReturn += " , " + Type;
            myReturn += " , Level : " + Level;
            myReturn += " , Total Experience : " + XP;
           myReturn += " , " + Attribute.FormatOutput();
            //myReturn += " , Items : " + ItemSlotsFormatOutput();
            //myReturn += " Damage : " + GetBaseDamage();
            myReturn += " , Unique Item : " + UniqueOutput;
            return myReturn;
        }
        
        #region Items
        // Gets the unique item (if any) from this monster when it dies...
        public Item GetUniqueItem()
        {
            var myReturn = ItemsViewModel.Instance.GetItem(UniqueItem);

            return myReturn;
        }

        // Drop all the items the monster has
        public List<Item> DropAllItems()
        {
            var myReturn = new List<Item>();

            // Drop all Items
            Item myItem;

            myItem = ItemsViewModel.Instance.GetItem(UniqueItem);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }
            return myReturn;
        }

        #endregion Items

    }
}

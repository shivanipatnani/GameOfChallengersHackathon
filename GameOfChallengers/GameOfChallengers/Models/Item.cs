using System;
using GameOfChallengers.Controllers;
using SQLite;

namespace GameOfChallengers.Models
{
    public class Item
    {
        [PrimaryKey]
        public string Id { get; set; }// unique id for each item

        public string Name { get; set; }// 25 char max

        public string Description { get; set; }

        public int Value { get; set; }// how much the attribute is increased
        
        public int Range { get; set; }// if the item allows ranged attacks

        // The Damage the Item can do if it is used as a weapon in the primary hand
        public int Damage { get; set; }

        public AttributeEnum Attribute { get; set; }// enum for what creature attribute the item effects

        public ItemLocationEnum Location { get; set; }// enum for what creature location the item is attached to

        public string ImageURI { get; set; }// image to be inserted

        public Item()
        {
            CreateDefaultItem();
        }

        // Create a default item for the instantiation
        private void CreateDefaultItem()
        {
            Name = "Unknown";
            Description = "Unknown";
            ImageURI = ItemsController.DefaultImageURI;

            Range = 0;
            Value = 0;
            Damage = 0;

            Location = ItemLocationEnum.Unknown;
            Attribute = AttributeEnum.Unknown;

            ImageURI = null;
        }

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            var myReturn = Name + " , " +
                            Description + " for " +
                            Location.ToString() + " with " +
                            Attribute.ToString() +
                            "+" + Value + " , " +
                            "Damage : " + Damage + " , " +
                            "Range : " + Range;

            return myReturn.Trim();
        }

        public Item(Item data)
        {
            Update(data);
        }

        // Constructor for Item called if needed to create a new item with set values.
        public Item(string name, string description, string imageuri, int range, int value, int damage, ItemLocationEnum location, AttributeEnum attribute)
        {
            // Create default, and then override...
            CreateDefaultItem();

            Name = name;
            Description = description;
            ImageURI = imageuri;

            Range = range;
            Value = value;
            Damage = damage;

            Location = location;
            Attribute = attribute;
        }

        // Update for Item, that will update the fields one by one.
        public void Update(Item newData)
        {
            if (newData == null)
            {
                return;
            }

            // Update all the fields in the Data, except for the Id and guid
            Name = newData.Name;
            Description = newData.Description;
            Value = newData.Value;
            Attribute = newData.Attribute;
            Location = newData.Location;
            ImageURI = newData.ImageURI;
            Range = newData.Range;
            Damage = newData.Damage;
        }

        //// Will update the Item to be stronger...
        //public void ScaleLevel(int level)
        //{
        //    var newValue = 1;

        //    if (GameGlobals.ForceRollsToNotRandom)
        //    {
        //        newValue = level;
        //    }
        //    else
        //    {
        //        // Add value 1 to level passed in...
        //        newValue = HelperEngine.RollDice(1, level);
        //    }

        //    Value = newValue;
        //}
    }
}
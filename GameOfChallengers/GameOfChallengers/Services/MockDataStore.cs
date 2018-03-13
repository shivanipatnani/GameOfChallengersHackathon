using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameOfChallengers.Models;

namespace GameOfChallengers.Services
{
    public sealed class MockDataStore : IDataStore
    {

        private static MockDataStore _instance;

        public static MockDataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MockDataStore();
                }
                return _instance;
            }
        }

        private List<Item> _itemDataset = new List<Item>();
        private List<Creature> _creatureDataset = new List<Creature>();
        private List<Score> _scoreDataset = new List<Score>();
        private MockDataStore()
        {
            InitilizeSeedData();
        }

        public void InitilizeSeedData()
        {
         
        
            var mockItems = new List<Item>
            {
                new Item { Id = "bow", Name = "Longbow", Value = 3, Range = 10, Attribute = AttributeEnum.Attack, Location = ItemLocationEnum.PrimaryHand, ImageURI="bow.jpeg"},
                new Item { Id = "helmet", Name = "Steel Helmet", Value = 3, Range = 0, Attribute = AttributeEnum.Defense, Location = ItemLocationEnum.Head},
                new Item { Id = "boots", Name = "Running Boots", Value = 1, Range = 0, Attribute = AttributeEnum.Speed, Location = ItemLocationEnum.Feet},
                new Item { Id = "gun", Name = "Gun", Value = 10, Range = 30, Attribute = AttributeEnum.Attack, Location = ItemLocationEnum.PrimaryHand, ImageURI="gun.jpeg"},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Sword", Value = 3, Range = 0, Attribute = 0, Location = 0, ImageURI="sword.jpeg"},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Trident", Value = 3, Range = 0, Attribute = 0, Location = 0, ImageURI="trident.jpeg"},
                
            };


            foreach (var data in mockItems)
            {
                _itemDataset.Add(data);
            }

            var mockCreature = new List<Creature>
            {
                //characters
                new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "Fighter 1", Level = 1, Attack = 1, Defense = 1, Speed = 1, XP = 100, MaxHealth = 10, CurrHealth = 5, Alive = true, Loc = 1,ImageURI = "fighter1.jpg"/*, CInventory = null*/ },
                new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "Fighter 2", Level = 2, Attack = 2, Defense = 2, Speed = 2, XP = 200, MaxHealth = 20, CurrHealth = 10, Alive = true, Loc = 2,ImageURI = "fighter2.jpeg"/*, CInventory = null*/ },
                new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "Fighter 3", Level = 3, Attack = 3, Defense = 3, Speed = 3, XP = 300, MaxHealth = 30, CurrHealth = 30, Alive = true, Loc = 3,ImageURI = "fighter3.jpeg"/*, CInventory = null*/ },
                new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "Fighter 4", Level = 2, Attack = 4, Defense = 4, Speed = 4, XP = 300, MaxHealth = 40, CurrHealth = 20, Alive = true, Loc = 3,ImageURI = "fighter4.jpg"/*, CInventory = null*/ },
                new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "Fighter 5", Level = 5, Attack = 5, Defense = 5, Speed = 3, XP = 300, MaxHealth = 50, CurrHealth = 25, Alive = true, Loc = 3,ImageURI = "fighter5.jpeg"/*, CInventory = null*/ },
                new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "Fighter 6", Level = 6, Attack = 6, Defense = 3, Speed = 5, XP = 300, MaxHealth = 60, CurrHealth = 30, Alive = true, Loc = 3,ImageURI = "fighter6.jpeg"/*, CInventory = null*/ },
                
                //monsters
                new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "Monster 1", Level = 1, Attack = 1, Defense = 1, Speed = 1, XP = 100, MaxHealth = 10, CurrHealth = 5, Alive = true, Loc = 1,ImageURI = "Monster1.jpeg"/*, CInventory = null*/ },
                new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "Monster 2", Level = 2, Attack = 2, Defense = 2, Speed = 2, XP = 200, MaxHealth = 20, CurrHealth = 10, Alive = true, Loc = 2,ImageURI = "monster2.jpeg"/*, CInventory = null*/ },
                new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "Monster 3", Level = 3, Attack = 3, Defense = 3, Speed = 3, XP = 300, MaxHealth = 30, CurrHealth = 15, Alive = true, Loc = 3,ImageURI = "monster3.jpeg"/*, CInventory = null*/ },
                new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "Monster 4", Level = 4, Attack = 4, Defense = 3, Speed = 3, XP = 300, MaxHealth = 30, CurrHealth = 15, Alive = true, Loc = 3,ImageURI = "monster4.jpeg"/*, CInventory = null*/ },
                new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "Monster 5", Level = 5, Attack = 5, Defense = 3, Speed = 3, XP = 300, MaxHealth = 30, CurrHealth = 15, Alive = true, Loc = 3,ImageURI = "monster5.jpeg"/*, CInventory = null*/ },
                new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "Monster 6", Level = 6, Attack = 6, Defense = 3, Speed = 3, XP = 300, MaxHealth = 30, CurrHealth = 15, Alive = true, Loc = 3,ImageURI = "monster6.jpeg"/*, CInventory = null*/ },


            };

            foreach (var data in mockCreature)
            {
                _creatureDataset.Add(data);
            }



            var mockScore = new List<Score>
            {
               new Score { Id = Guid.NewGuid().ToString(), Name = "Player 1", Date = DateTime.Now, FinalScore = 100, Auto = false,  Round = 0, TotalXP = 0, Turns = 0/*, TotalMonstersKilled = null, TotalItemsDropped = null*/ },

            };

            foreach (var data in mockScore)
            {
                _scoreDataset.Add(data);
            }

        }

        // Creature
        public async Task<bool> AddAsync_Creature(Creature data)
        {
            _creatureDataset.Add(data);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync_Creature(Creature data)
        {
            var myData = _creatureDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync_Creature(Creature data)
        {
            var myData = _creatureDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _creatureDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        public async Task<Creature> GetAsync_Creature(string id)
        {
            return await Task.FromResult(_creatureDataset.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Creature>> GetAllAsync_Creature(bool forceRefresh = false)
        {
            return await Task.FromResult(_creatureDataset);
        }


        // Item
        public async Task<bool> InsertUpdateAsync_Item(Item data)
        {

            // Check to see if the item exist
            var oldData = await GetAsync_Item(data.Id);
            if (oldData == null)
            {
                _itemDataset.Add(data);
                return true;
            }

            // Compare it, if different update in the DB
            var UpdateResult = await UpdateAsync_Item(data);
            if (UpdateResult)
            {
                await AddAsync_Item(data);
                return true;
            }

            return false;
        }
        public async Task<bool> AddAsync_Item(Item data)
        {
            _itemDataset.Add(data);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync_Item(Item data)
        {
            var myData = _itemDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync_Item(Item data)
        {
            var myData = _itemDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _itemDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetAsync_Item(string id)
        {
            return await Task.FromResult(_itemDataset.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false)
        {
            return await Task.FromResult(_itemDataset);
        }


        // Score
        public async Task<bool> AddAsync_Score(Score data)
        {
            _scoreDataset.Add(data);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync_Score(Score data)
        {
            var myData = _scoreDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync_Score(Score data)
        {
            var myData = _scoreDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _scoreDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        public async Task<Score> GetAsync_Score(string id)
        {
            return await Task.FromResult(_scoreDataset.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            return await Task.FromResult(_scoreDataset);
        }

    }
}
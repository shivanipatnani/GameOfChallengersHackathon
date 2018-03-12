using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameOfChallengers.ViewModels;
using GameOfChallengers.Models;

namespace GameOfChallengers.Services
{
    public sealed class SQLDataStore : IDataStore
    {
        private static SQLDataStore _instance;

        public static SQLDataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SQLDataStore();
                }
                return _instance;
            }
        }

        private SQLDataStore()
        {
            App.Database.CreateTableAsync<Item>().Wait();
            App.Database.CreateTableAsync<Creature>().Wait();
            App.Database.CreateTableAsync<Score>().Wait();
        }

        // Create the Database Tables
        private void CreateTables()
        {
            App.Database.CreateTableAsync<Item>().Wait();
            App.Database.CreateTableAsync<Creature>().Wait();
            App.Database.CreateTableAsync<Score>().Wait();
            
        }

        // Delete the Datbase Tables by dropping them
        private void DeleteTables()
        {
            App.Database.DropTableAsync<Item>().Wait();
            App.Database.DropTableAsync<Creature>().Wait();
            App.Database.DropTableAsync<Score>().Wait();
            
        }

        // Tells the View Models to update themselves.
        private void NotifyViewModelsOfDataChange()
        {
            ItemsViewModel.Instance.SetNeedsRefresh(true);
            MonstersViewModel.Instance.SetNeedsRefresh(true);
            CharactersViewModel.Instance.SetNeedsRefresh(true);
            ScoresViewModel.Instance.SetNeedsRefresh(true);
        }

        public void InitializeDatabaseNewTables()
        {
            // Delete the tables
            DeleteTables();

            // make them again
            CreateTables();

            // Populate them
            InitilizeSeedData();

            // Tell View Models they need to refresh
            NotifyViewModelsOfDataChange();
        }
        private async void InitilizeSeedData()
        {

            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "Sword", Value = 3, Range = 0, Attribute = 0, Location = 0 });
            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "Boots", Value = 3, Range = 0, Attribute = 0, Location = 0 });
            await AddAsync_Item(new Item { Id = Guid.NewGuid().ToString(), Name = "Ring", Value = 3, Range = 0, Attribute = 0, Location = 0 });
            

            /*NEED TO ADD  each item ID set to null on each creature*/
            //characters
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "First Character", Level = 1, OnTeam = true, Attack = 10, Defense = 10, Speed = 1, XP = 100, MaxHealth = 10, CurrHealth = 5, Alive = true, Loc = 1 });
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "Second Character", Level = 2, OnTeam = true, Attack = 20, Defense = 20, Speed = 2, XP = 200, MaxHealth = 20, CurrHealth = 10, Alive = true, Loc = 2 });
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "Third Character", Level = 3, OnTeam = true, Attack = 30, Defense = 30, Speed = 3, XP = 300, MaxHealth = 30, CurrHealth = 15, Alive = true, Loc = 3 });
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "Fourth Character", Level = 4, OnTeam = true, Attack = 40, Defense = 40, Speed = 4, XP = 400, MaxHealth = 40, CurrHealth = 20, Alive = true, Loc = 4 });
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "Fifth Character", Level = 5, OnTeam = true, Attack = 50, Defense = 50, Speed = 5, XP = 500, MaxHealth = 50, CurrHealth = 25, Alive = true, Loc = 5 });
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 0, Name = "Sixth Character", Level = 6, OnTeam = true, Attack = 60, Defense = 60, Speed = 6, XP = 600, MaxHealth = 60, CurrHealth = 30, Alive = true, Loc = 6 });

            //monsters
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "First Monster", Level = 1, OnTeam = false, Attack = 10, Defense = 10, Speed = 1, XP = 100, MaxHealth = 10, CurrHealth = 5, Alive = true, Loc = 1 });
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "Second Monster", Level = 2, OnTeam = false, Attack = 20, Defense = 20, Speed = 2, XP = 200, MaxHealth = 20, CurrHealth = 10, Alive = true, Loc = 2 });
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "Third Monster", Level = 3, OnTeam = false, Attack = 30, Defense = 30, Speed = 3, XP = 300, MaxHealth = 30, CurrHealth = 15, Alive = true, Loc = 3 });
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "Fourth Monster", Level = 4, OnTeam = false, Attack = 40, Defense = 40, Speed = 4, XP = 400, MaxHealth = 40, CurrHealth = 20, Alive = true, Loc = 4 });
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "Fifth Monster", Level = 5, OnTeam = false, Attack = 50, Defense = 50, Speed = 5, XP = 500, MaxHealth = 50, CurrHealth = 25, Alive = true, Loc = 5 });
            await AddAsync_Creature(new Creature { Id = Guid.NewGuid().ToString(), Type = 1, Name = "Sixth Monster", Level = 6, OnTeam = false, Attack = 60, Defense = 60, Speed = 6, XP = 600, MaxHealth = 60, CurrHealth = 30, Alive = true, Loc = 6 });

            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "Player 1", Date = DateTime.Now, FinalScore = 100, Auto = false, Round = 0, TotalXP = 0, Turns = 0/*, TotalMonstersKilled = null, TotalItemsDropped = null*/ });
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "Player 2", Date = DateTime.Now, FinalScore = 200, Auto = false, Round = 0, TotalXP = 0, Turns = 0/*, TotalMonstersKilled = null, TotalItemsDropped = null*/ });
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "Player 3", Date = DateTime.Now, FinalScore = 300, Auto = false, Round = 0, TotalXP = 0, Turns = 0/*, TotalMonstersKilled = null, TotalItemsDropped = null*/ });

        }

        // Item
        public async Task<bool> InsertUpdateAsync_Item(Item data)
        {

            // Check to see if the item exist
            var oldData = await GetAsync_Item(data.Id);
            if (oldData == null)
            {
                // If it does not exist, add it to the DB
                var InsertResult = await AddAsync_Item(data);
                if (InsertResult)
                {
                    return true;
                }

                return false;
            }

            // Compare it, if different update in the DB
            var UpdateResult = await UpdateAsync_Item(data);
            if (UpdateResult)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> AddAsync_Item(Item data)
        {
            var result = await App.Database.InsertAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateAsync_Item(Item data)
        {
            var result = await App.Database.UpdateAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync_Item(Item data)
        {
            var result = await App.Database.DeleteAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<Item> GetAsync_Item(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            // Need to add a try catch here, to catch when looking for something that does not exist in the db...
            try
            {
                var result = await App.Database.GetAsync<Item>(id);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false)
        {
            var result = await App.Database.Table<Item>().ToListAsync();
            return result;
        }

        // Creature
        public async Task<bool> AddAsync_Creature(Creature data)
        {
            var result = await App.Database.InsertAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateAsync_Creature(Creature data)
        {
            var result = await App.Database.UpdateAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync_Creature(Creature data)
        {
            var result = await App.Database.DeleteAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<Creature> GetAsync_Creature(string id)
        {
            var result = await App.Database.GetAsync<Creature>(id);
            return result;
        }

        public async Task<IEnumerable<Creature>> GetAllAsync_Creature(bool forceRefresh = false)
        {
            var result = await App.Database.Table<Creature>().ToListAsync();
            return result;
        }

        // Score
        public async Task<bool> AddAsync_Score(Score data)
        {
            var result = await App.Database.InsertAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateAsync_Score(Score data)
        {
            var result = await App.Database.UpdateAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync_Score(Score data)
        {
            var result = await App.Database.DeleteAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<Score> GetAsync_Score(string id)
        {
            var result = await App.Database.GetAsync<Score>(id);
            return result;
        }

        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            var result = await App.Database.Table<Score>().ToListAsync();
            return result;

        }

    }
}
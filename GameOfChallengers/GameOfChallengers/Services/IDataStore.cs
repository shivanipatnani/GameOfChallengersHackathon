using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameOfChallengers.Models;

namespace GameOfChallengers.Services
{
    public interface IDataStore
    {
        Task<bool> InsertUpdateAsync_Item(Item data);
        Task<bool> AddAsync_Item(Item item);
        Task<bool> UpdateAsync_Item(Item item);
        Task<bool> DeleteAsync_Item(Item item);
        Task<Item> GetAsync_Item(string id);
        Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false);

        Task<bool> AddAsync_Creature(Creature data);
        Task<bool> UpdateAsync_Creature(Creature data);
        Task<bool> DeleteAsync_Creature(Creature data);
        Task<Creature> GetAsync_Creature(string id);
        Task<IEnumerable<Creature>> GetAllAsync_Creature(bool forceRefresh = false);


        Task<bool> AddAsync_Score(Score data);
        Task<bool> UpdateAsync_Score(Score data);
        Task<bool> DeleteAsync_Score(Score data);
        Task<Score> GetAsync_Score(string id);
        Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false);

    }
}

﻿using BreakTimeApp.Models;

namespace BreakTimeApp.Services
{
    public interface ITimeStoreItemDataService
    {
        Task<IEnumerable<TimeStoreItemDb>> GetAllTimeStoreItemsAsync();
        Task<TimeStoreItemDb> GetTimeStoreItemByIdAsync(string id);
        Task AddTimeStoreItemAsync(TimeStoreItemDb item);
        Task UpdateTimeStoreItemAsync(TimeStoreItemDb item);
        Task DeleteTimeStoreItemAsync(string id);
    }
}

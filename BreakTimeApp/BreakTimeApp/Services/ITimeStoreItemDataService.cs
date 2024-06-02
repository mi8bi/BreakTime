using BreakTimeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

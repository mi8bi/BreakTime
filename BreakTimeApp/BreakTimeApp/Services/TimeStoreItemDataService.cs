using BreakTimeApp.Helpers;
using BreakTimeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BreakTimeApp.Services
{
    internal class TimeStoreItemDataService : ITimeStoreItemDataService
    {
        private readonly TimeStoreItemDbContext _context;

        public TimeStoreItemDataService(TimeStoreItemDbContext context) 
        {
            _context = context;
        }

        [LogAspect]
        public async Task AddTimeStoreItemAsync(TimeStoreItemDb item)
        {
            _context.TimeStoreItems.Add(item);
            await _context.SaveChangesAsync();
        }

        [LogAspect]
        public async Task DeleteTimeStoreItemAsync(string id)
        {
            var item = await _context.TimeStoreItems.FindAsync(id);
            if (item != null)
            {
                _context.TimeStoreItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        [LogAspect]
        public async Task<IEnumerable<TimeStoreItemDb>> GetAllTimeStoreItemsAsync()
        {
            return await _context.TimeStoreItems.ToListAsync();
        }

        [LogAspect]
        public async Task<TimeStoreItemDb> GetTimeStoreItemByIdAsync(string id)
        {
            return await _context.TimeStoreItems.FindAsync(id);
        }

        [LogAspect]
        public async Task UpdateTimeStoreItemAsync(TimeStoreItemDb item)
        {
            _context.TimeStoreItems.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}

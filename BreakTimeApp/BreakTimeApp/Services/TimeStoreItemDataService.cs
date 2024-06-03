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
        public void AddTimeStoreItem(TimeStoreItemDb item)
        {
            _context.TimeStoreItems.Add(item);
            _context.SaveChanges();
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
        public void DeleteTimeStoreItem(string id)
        {
            var item = _context.TimeStoreItems.Find(id);
            if (item != null)
            {
                _context.TimeStoreItems.Remove(item);
                _context.SaveChangesAsync();
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
            var item = await _context.TimeStoreItems.FindAsync(id);

            // Entity Framework Core は、同じエンティティがコンテキストに存在する場合、
            // 新しくデータベースから取得するのではなく、キャッシュされたエンティティを返すため、
            // 最新のデータをデータベースから取得するようにする
            if (item != null)
            {
                await _context.Entry(item).ReloadAsync();
            }

            return item;
        }

        [LogAspect]
        public async Task UpdateTimeStoreItemAsync(TimeStoreItemDb item)
        {
            _context.TimeStoreItems.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}

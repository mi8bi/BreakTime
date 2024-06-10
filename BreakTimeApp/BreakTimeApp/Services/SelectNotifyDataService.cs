using BreakTimeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BreakTimeApp.Services
{
    public class SelectNotifyDataService : ISelectNotifyDataService
    {
        private readonly SelectNotifyDbContext _context;

        public SelectNotifyDataService(SelectNotifyDbContext context) 
        {
            _context = context;
        }

        public async Task AddSelectNotifyItemAsync(SelectNotifyDb item)
        {
            _context.SelectNotifyItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSelectNotifyItemAsync(int id)
        {
            var item = await _context.SelectNotifyItems.FindAsync(id);
            if (item != null)
            {
                _context.SelectNotifyItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SelectNotifyDb>> GetAllSelectNotifyItemsAsync()
        {
            return await _context.SelectNotifyItems.ToListAsync();
        }

        public async Task<SelectNotifyDb> GetSelectNotifyItemByIdAsync(int id)
        {
            var item = await _context.SelectNotifyItems.FindAsync(id);

            // Entity Framework Core は、同じエンティティがコンテキストに存在する場合、
            // 新しくデータベースから取得するのではなく、キャッシュされたエンティティを返すため、
            // 最新のデータをデータベースから取得するようにする
            if (item != null)
            {
                await _context.Entry(item).ReloadAsync();
            }

            return item;
        }

        public async Task UpdateSelectNotifyItemAsync(SelectNotifyDb item)
        {
            var existingEntity = _context.ChangeTracker.Entries<SelectNotifyDb>()
                                         .FirstOrDefault(e => e.Entity.Id == item.Id)?.Entity;
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.SelectNotifyItems.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}

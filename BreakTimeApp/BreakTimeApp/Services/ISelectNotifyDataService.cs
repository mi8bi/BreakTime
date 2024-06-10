using BreakTimeApp.Models;

namespace BreakTimeApp.Services
{
    public interface ISelectNotifyDataService
    {
        Task<IEnumerable<SelectNotifyDb>> GetAllSelectNotifyItemsAsync();
        Task<SelectNotifyDb> GetSelectNotifyItemByIdAsync(int id);
        Task AddSelectNotifyItemAsync(SelectNotifyDb item);
        Task UpdateSelectNotifyItemAsync(SelectNotifyDb item);
        Task DeleteSelectNotifyItemAsync(int id);
    }
}

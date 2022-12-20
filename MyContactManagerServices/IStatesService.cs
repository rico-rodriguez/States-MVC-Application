using ContactWebModels;

namespace MyContactManagerServices
{
    public interface IStatesService
    {
        Task<List<State>> GetAllAsync();
        Task<State> GetAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> AddorUpdateAsync(State state);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteAsync(State state);
    }
}
using ContactWebModels;
using MyContactManagerRepo;

namespace MyContactManagerServices
{
    public class StatesService : IStatesService
    {
        private readonly IStatesRepository _statesRepository;
        public StatesService(IStatesRepository statesRepository)
        {
            _statesRepository = statesRepository;
        }

        public async Task<List<State>> GetAllAsync()
        {
            return await _statesRepository.GetAllAsync();
        }

        public async Task<State> GetAsync(int id)
        {
            return await _statesRepository.GetAsync(id);

        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _statesRepository.ExistsAsync(id);
        }

        public async Task<int> AddorUpdateAsync(State state)
        {
            return await _statesRepository.AddorUpdateAsync(state);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _statesRepository.DeleteAsync(id);
        }

        public async Task<int> DeleteAsync(State state)
        {
            return await _statesRepository.DeleteAsync(state);
        }
    }
}

using ContactWebModels;
using Moq;
using MyContactManagerRepo;
using MyContactManagerServices;
using Shouldly;

namespace MyContactManagerUnitTests
{
    public class TestStatesService
    {
        private IStatesService _statesService;
        private Mock<IStatesRepository> _repository;

        public TestStatesService()
        {
            CreateMOQs();
            _statesService = new StatesService(_repository.Object);
        }

        private void CreateMOQs()
        {
            _repository = new Mock<IStatesRepository>();
            var states = GetStatesTestData();
            var singleState = GetSingleState();
            _repository.Setup(x => x.GetAllAsync()).Returns(states);
            _repository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(singleState);
        }

        private async Task<List<State>> GetStatesTestData()
        {
            return new List<State>()
            {
                new State() { Id = 1, Name = "Alabama", Abbreviation = "AL" },
                new State() { Id = 2, Name = "Alaska", Abbreviation = "AK" },
                new State() { Id = 3, Name = "Arizona", Abbreviation = "AZ" },
                new State() { Id = 4, Name = "Arkansas", Abbreviation = "AR" },
                new State() { Id = 5, Name = "California", Abbreviation = "CA" },
                new State() { Id = 6, Name = "Colorado", Abbreviation = "CO" },
                new State() { Id = 7, Name = "Connecticut", Abbreviation = "CT" },
                new State() { Id = 8, Name = "Delaware", Abbreviation = "DE" },
                new State() { Id = 9, Name = "District of Columbia", Abbreviation = "DC" },
                new State() { Id = 10, Name = "Florida", Abbreviation = "FL" },
                new State() { Id = 11, Name = "Georgia", Abbreviation = "GA" },
                new State() { Id = 12, Name = "Hawaii", Abbreviation = "HI" },
                new State() { Id = 13, Name = "Idaho", Abbreviation = "ID" },
                new State() { Id = 14, Name = "Illinois", Abbreviation = "IL" },
                new State() { Id = 15, Name = "Indiana", Abbreviation = "IN" },
                new State() { Id = 16, Name = "Iwoa", Abbreviation = "IA" },
                new State() { Id = 17, Name = "Kansas", Abbreviation = "KS" },
                new State() { Id = 18, Name = "Kentucky", Abbreviation = "KY" },
                new State() { Id = 19, Name = "Louisiana", Abbreviation = "LA" },
                new State() { Id = 20, Name = "Maine", Abbreviation = "ME" }
            };
        }

        private async Task<State> GetSingleState()
        {
            var states = await GetStatesTestData();
            return states[0];
        }

        [Fact]
        public async Task TestGetSingleState()
        {
            var state = _statesService.GetAsync(1).Result;
            state.Name.ShouldBe("Alabama");
            state.Abbreviation.ShouldBe("AL");

        }

        [Fact]
        public async Task TestGetAllStates()
        {
            var states = _statesService.GetAllAsync().Result;
            states[0].Name.ShouldBe("Alabama");
            states[0].Abbreviation.ShouldBe("AL");
            states[1].Name.ShouldBe("Alaska");
            states[1].Abbreviation.ShouldBe("AK");
            states[2].Name.ShouldBe("Arizona");
            states[2].Abbreviation.ShouldBe("AZ");

        }
    }
}
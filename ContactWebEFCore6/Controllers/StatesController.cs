using ContactWebEFCore6.Models;
using ContactWebModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyContactManagerServices;

namespace ContactWebEFCore6.Controllers
{
    public class StatesController : Controller
    {
        private readonly IStatesService _statesService;
        private readonly IMemoryCache _memoryCache;

        public StatesController(IStatesService statesService, IMemoryCache memoryCache)
        {
            _statesService = statesService;
            _memoryCache = memoryCache;
        }

        // GET: States
        public async Task<IActionResult> Index()
        {
            List<State> states = await GetStatesFromCache();
            return View(states);
        }

        private async Task<List<State>> GetStatesFromCache()
        {
            var states = new List<State>();
            if (!_memoryCache.TryGetValue(ContactCacheConstants.statesData, out states))
            {
                states = await _statesService.GetAllAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _memoryCache.Set(ContactCacheConstants.statesData, states, cacheEntryOptions);
            }

            return states;
        }

        //private async Task<List<State>> GetStates()
        //{
        //    var session = HttpContext.Session;
        //    var statesData = session.GetString("statesData");
        //    if (!string.IsNullOrWhiteSpace(statesData))
        //    {
        //        return JsonConvert.DeserializeObject<List<State>>(session.GetString(ContactCacheConstants.statesData));
        //    }
        //    List<State> states = await _statesService.GetAllAsync();
        //    session.SetString(ContactCacheConstants.statesData, JsonConvert.SerializeObject(states));
        //    return states;
        //}
        // GET: States/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _statesService.GetAsync((int)id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // GET: States/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: States/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Abbreviation")] State state)
        {
            if (ModelState.IsValid)
            {
                await _statesService.AddorUpdateAsync(state);
                _memoryCache.Remove(ContactCacheConstants.statesData);
                return RedirectToAction(nameof(Index));
            }
            return View(state);
        }

        // GET: States/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || !await _statesService.ExistsAsync((int)id))
            {
                return NotFound();
            }
            var state = await _statesService.GetAsync((int)id);
            if (state == null)
            {
                return NotFound();
            }
            return View(state);
        }

        // POST: States/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Abbreviation")] State state)
        {
            if (id != state.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _statesService.AddorUpdateAsync(state);
                    _memoryCache.Remove(ContactCacheConstants.statesData);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateExists(state.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(state);
        }

        // GET: States/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || !await _statesService.ExistsAsync((int)id))
            {
                return NotFound();
            }
            var state = await _statesService.GetAsync((int)id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var state = await _statesService.GetAsync(id);
            await _statesService.DeleteAsync(state);
            _memoryCache.Remove(ContactCacheConstants.statesData);
            return RedirectToAction(nameof(Index));
        }

        private bool StateExists(int id)
        {
            return _statesService.ExistsAsync(id).Result;
        }
    }
}

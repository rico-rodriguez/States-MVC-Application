using ContactWebEFCore6.Models;
using ContactWebModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyContactManagerData;
using Newtonsoft.Json;

namespace ContactWebEFCore6.Controllers
{
    public class StatesController : Controller
    {
        private readonly MyContactManagerDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public StatesController(MyContactManagerDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
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
                states = await _context.States.OrderBy(x => x.Name).ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _memoryCache.Set(ContactCacheConstants.statesData, states, cacheEntryOptions);
            }

            return states;
        }

        private async Task<List<State>> GetStates()
        {
            var session = HttpContext.Session;
            var statesData = session.GetString("statesData");
            if (!string.IsNullOrWhiteSpace(statesData))
            {
                return JsonConvert.DeserializeObject<List<State>>(session.GetString(ContactCacheConstants.statesData));
            }
            List<State> states = await _context.States.OrderBy(x => x.Name).ToListAsync();
            session.SetString(ContactCacheConstants.statesData, JsonConvert.SerializeObject(states));
            return states;
        }
        // GET: States/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.States
                .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Add(state);
                await _context.SaveChangesAsync();
                _memoryCache.Remove(ContactCacheConstants.statesData);
                return RedirectToAction(nameof(Index));
            }
            return View(state);
        }

        // GET: States/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.States.FindAsync(id);
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
                    _context.Update(state);
                    await _context.SaveChangesAsync();
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
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.States
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var state = await _context.States.FindAsync(id);
            _context.States.Remove(state);
            await _context.SaveChangesAsync();
            _memoryCache.Remove(ContactCacheConstants.statesData);
            return RedirectToAction(nameof(Index));
        }

        private bool StateExists(int id)
        {
            return _context.States.Any(e => e.Id == id);
        }
    }
}

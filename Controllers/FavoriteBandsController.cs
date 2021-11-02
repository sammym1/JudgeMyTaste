using JudgeMyTaste.Data;
using JudgeMyTaste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace Controllers
{
    public class FavoriteBandsController : Controller
    {
        private readonly JudgeMyTasteContext _context;
        private readonly ILogger<FavoriteBandsController> _logger;
        private IConfiguration Configuration;

      

        public FavoriteBandsController(ILogger<FavoriteBandsController> logger,JudgeMyTasteContext context, IConfiguration _configuration)
        {
            _context = context;
            Configuration = _configuration;
            _logger = logger;
        }

        // GET: FavoriteBands
        public async Task<IActionResult> Index()
        {
            Task<string> x = GetAsync(1);
            return View(await _context.FavoriteBands.ToListAsync());

        }

        // GET: FavoriteBands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           // Task<string>  x = GetAsync();
            var favoriteBand = await _context.FavoriteBands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favoriteBand == null)
            {
                return NotFound();
            }

            return View(favoriteBand);
        }

        private async Task<string> GetAsync(int?id)
        {
           
            var query = $"SELECT Name FROM FavoriteBands WHERE Id = " + id;
            var rvalue = string.Empty;
            var connection = new SqlConnection(Configuration.GetConnectionString("MS_TableConnectionString"));
            //using (var connection = new SqlConnection(Configuration.GetConnectionString("MS_TableConnectionString")))
            //{
           // connection.Open();
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var orderDetail = await connection.QuerySingleOrDefaultAsync<string>(query).ConfigureAwait(true);
                watch.Stop();
            
                _logger.LogInformation("Elapsed during DB select request: " + watch.ElapsedMilliseconds + "ms", NullLogger.Instance);
                _logger.LogInformation("SELECT returned " + orderDetail);

                rvalue = orderDetail;
                // FiddleHelper.WriteTable(new List<OrderDetail>() { orderDetail });
           // }
            return rvalue;

        }

// GET: FavoriteBands/Create
public IActionResult Create()
        {
            return View();
        }

        // POST: FavoriteBands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.



    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,EnteredBy,EnteredOn")] FavoriteBand favoriteBand)
        {

           
            if (ModelState.IsValid)
            {
                _context.Add(favoriteBand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(favoriteBand);
        }

        // GET: FavoriteBands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Task<string> x = GetAsync(id);
            var favoriteBand = await _context.FavoriteBands.FindAsync(id);
            if (favoriteBand == null)
            {
                return NotFound();
            }
            return View(favoriteBand);
        }

        // POST: FavoriteBands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,EnteredBy,EnteredOn")] FavoriteBand favoriteBand)
        {
            if (id != favoriteBand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favoriteBand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteBandExists(favoriteBand.Id))
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
            return View(favoriteBand);
        }

        // GET: FavoriteBands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favoriteBand = await _context.FavoriteBands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favoriteBand == null)
            {
                return NotFound();
            }

            return View(favoriteBand);
        }

        // POST: FavoriteBands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favoriteBand = await _context.FavoriteBands.FindAsync(id);
            _context.FavoriteBands.Remove(favoriteBand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteBandExists(int id)
        {
            return _context.FavoriteBands.Any(e => e.Id == id);
        }
    }
}

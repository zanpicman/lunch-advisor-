using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LunchAdvisor.Data;
using LunchAdvisor.Models;
using System.Security.Claims;

namespace LunchAdvisor.Controllers
{
    public class WaiterRatingsController : Controller
    {
        private readonly LunchAdvisorContext _context;

        public WaiterRatingsController(LunchAdvisorContext context)
        {
            _context = context;
        }

        // GET: WaiterRatings
        public async Task<IActionResult> Index()
        {
            var lunchAdvisorContext = _context.WaiterRating.Include(w => w.User).Include(w => w.Waiter);
            return View(await lunchAdvisorContext.ToListAsync());
        }

        // GET: WaiterRatings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WaiterRating == null)
            {
                return NotFound();
            }

            var waiterRating = await _context.WaiterRating
                .Include(w => w.User)
                .Include(w => w.Waiter)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (waiterRating == null)
            {
                return NotFound();
            }

            return View(waiterRating);
        }

        public async Task<IActionResult> Review(int? id)
        {
            ViewBag.Id = id;

            var Waiter = await _context.Waiter
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Waiter == null)
            {
                return NotFound();
            }

            ViewBag.Name = Waiter.Name;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(WaiterRating waiterRating)
        {
            waiterRating.date = DateTime.Now;
            waiterRating.WaiterID = waiterRating.ID;
            waiterRating.ID = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            waiterRating.UserID = claims.Value;


            ModelState.Clear();
            TryValidateModel(waiterRating);
            if (ModelState.IsValid)
            {
                _context.Add(waiterRating);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Waiters", new {id = waiterRating.ID});
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", waiterRating.UserID);
            ViewData["WaiterID"] = new SelectList(_context.Waiter, "ID", "ID", waiterRating.WaiterID);
            return View(waiterRating);
        }


        // GET: WaiterRatings/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["WaiterID"] = new SelectList(_context.Waiter, "ID", "ID");
            return View();
        }

        // POST: WaiterRatings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,WaiterID,UserID,Name,Description,date,rating")] WaiterRating waiterRating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(waiterRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", waiterRating.UserID);
            ViewData["WaiterID"] = new SelectList(_context.Waiter, "ID", "ID", waiterRating.WaiterID);
            return View(waiterRating);
        }

        // GET: WaiterRatings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WaiterRating == null)
            {
                return NotFound();
            }

            var waiterRating = await _context.WaiterRating.FindAsync(id);
            if (waiterRating == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", waiterRating.UserID);
            ViewData["WaiterID"] = new SelectList(_context.Waiter, "ID", "ID", waiterRating.WaiterID);
            return View(waiterRating);
        }

        // POST: WaiterRatings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,WaiterID,UserID,Name,Description,date,rating")] WaiterRating waiterRating)
        {
            if (id != waiterRating.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(waiterRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WaiterRatingExists(waiterRating.ID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", waiterRating.UserID);
            ViewData["WaiterID"] = new SelectList(_context.Waiter, "ID", "ID", waiterRating.WaiterID);
            return View(waiterRating);
        }

        // GET: WaiterRatings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WaiterRating == null)
            {
                return NotFound();
            }

            var waiterRating = await _context.WaiterRating
                .Include(w => w.User)
                .Include(w => w.Waiter)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (waiterRating == null)
            {
                return NotFound();
            }

            return View(waiterRating);
        }

        // POST: WaiterRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WaiterRating == null)
            {
                return Problem("Entity set 'LunchAdvisorContext.WaiterRating'  is null.");
            }
            var waiterRating = await _context.WaiterRating.FindAsync(id);
            if (waiterRating != null)
            {
                _context.WaiterRating.Remove(waiterRating);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WaiterRatingExists(int id)
        {
          return _context.WaiterRating.Any(e => e.ID == id);
        }
    }
}

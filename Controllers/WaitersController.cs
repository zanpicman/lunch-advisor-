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
    public class WaitersController : Controller
    {
        private readonly LunchAdvisorContext _context;

        public WaitersController(LunchAdvisorContext context)
        {
            _context = context;
        }

        // GET: Waiters
        public async Task<IActionResult> Index()
        {
            var lunchAdvisorContext = _context.Waiter.Include(w => w.Restaurant);
            return View(await lunchAdvisorContext.ToListAsync());
        }

        // GET: Waiters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Waiter == null)
            {
                return NotFound();
            }

            var waiter = await _context.Waiter
                .Include(w => w.Restaurant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (waiter == null)
            {
                return NotFound();
            }

            return View(waiter);
        }

        // GET: Waiters/Create
        public IActionResult Create()
        {
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "ID");
            return View();
        }

        // POST: Waiters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,RestaurantID,Name")] Waiter waiter)
        {
            /*
             * Get user id
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Console.WriteLine(claims.Value);
            */


            if (ModelState.IsValid)
            {
                _context.Add(waiter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "ID", waiter.RestaurantID);
            return View(waiter);
        }

        // GET: Waiters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Waiter == null)
            {
                return NotFound();
            }

            var waiter = await _context.Waiter.FindAsync(id);
            if (waiter == null)
            {
                return NotFound();
            }
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "ID", waiter.RestaurantID);
            return View(waiter);
        }

        // POST: Waiters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,RestaurantID,Name")] Waiter waiter)
        {
            if (id != waiter.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(waiter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WaiterExists(waiter.ID))
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
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "ID", waiter.RestaurantID);
            return View(waiter);
        }

        // GET: Waiters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Waiter == null)
            {
                return NotFound();
            }

            var waiter = await _context.Waiter
                .Include(w => w.Restaurant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (waiter == null)
            {
                return NotFound();
            }

            return View(waiter);
        }

        // POST: Waiters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Waiter == null)
            {
                return Problem("Entity set 'LunchAdvisorContext.Waiter'  is null.");
            }
            var waiter = await _context.Waiter.FindAsync(id);
            if (waiter != null)
            {
                _context.Waiter.Remove(waiter);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WaiterExists(int id)
        {
          return _context.Waiter.Any(e => e.ID == id);
        }
    }
}

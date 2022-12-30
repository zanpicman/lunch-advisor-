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
    public class RestaurantRatingsController : Controller
    {
        private readonly LunchAdvisorContext _context;

        public RestaurantRatingsController(LunchAdvisorContext context)
        {
            _context = context;
        }

        // GET: RestaurantRatings
        public async Task<IActionResult> Index()
        {
            var lunchAdvisorContext = _context.RestaurantRating.Include(r => r.Restaurant).Include(r => r.User);
            return View(await lunchAdvisorContext.ToListAsync());
        }

        // GET: RestaurantRatings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RestaurantRating == null)
            {
                return NotFound();
            }

            var restaurantRating = await _context.RestaurantRating
                .Include(r => r.Restaurant)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restaurantRating == null)
            {
                return NotFound();
            }

            return View(restaurantRating);
        }

        public async Task<IActionResult> Review(int? id)
        {
            ViewBag.Id = id;

            var Restaurant = await _context.Restaurant
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Restaurant == null)
            {
                return NotFound();
            }

            ViewBag.Name = Restaurant.Name;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(RestaurantRating restaurantRating)
        {
            restaurantRating.date = DateTime.Now;
            restaurantRating.RestaurantID = restaurantRating.ID;
            restaurantRating.ID = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            restaurantRating.UserID = claims.Value;

            
            ModelState.Clear();
            TryValidateModel(restaurantRating);
            if (ModelState.IsValid)
            {
                _context.Add(restaurantRating);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Restaurants", new {id = restaurantRating.RestaurantID});
            }
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "ID", restaurantRating.RestaurantID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", restaurantRating.UserID);
            return View(restaurantRating);
        }

        // GET: RestaurantRatings/Create
        public IActionResult Create()
        {
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "ID");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: RestaurantRatings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,RestaurantID,UserID,Name,Description,date,rating")] RestaurantRating restaurantRating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurantRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "ID", restaurantRating.RestaurantID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", restaurantRating.UserID);
            return View(restaurantRating);
        }

        // GET: RestaurantRatings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RestaurantRating == null)
            {
                return NotFound();
            }

            var restaurantRating = await _context.RestaurantRating.FindAsync(id);
            if (restaurantRating == null)
            {
                return NotFound();
            }
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "ID", restaurantRating.RestaurantID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", restaurantRating.UserID);
            return View(restaurantRating);
        }

        // POST: RestaurantRatings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,RestaurantID,UserID,Name,Description,date,rating")] RestaurantRating restaurantRating)
        {
            if (id != restaurantRating.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurantRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantRatingExists(restaurantRating.ID))
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
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", "ID", restaurantRating.RestaurantID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", restaurantRating.UserID);
            return View(restaurantRating);
        }

        // GET: RestaurantRatings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RestaurantRating == null)
            {
                return NotFound();
            }

            var restaurantRating = await _context.RestaurantRating
                .Include(r => r.Restaurant)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restaurantRating == null)
            {
                return NotFound();
            }

            return View(restaurantRating);
        }

        // POST: RestaurantRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RestaurantRating == null)
            {
                return Problem("Entity set 'LunchAdvisorContext.RestaurantRating'  is null.");
            }
            var restaurantRating = await _context.RestaurantRating.FindAsync(id);
            if (restaurantRating != null)
            {
                _context.RestaurantRating.Remove(restaurantRating);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantRatingExists(int id)
        {
          return _context.RestaurantRating.Any(e => e.ID == id);
        }
    }
}

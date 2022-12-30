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
    public class DishRatingsController : Controller
    {
        private readonly LunchAdvisorContext _context;

        public DishRatingsController(LunchAdvisorContext context)
        {
            _context = context;
        }

        // GET: DishRatings
        public async Task<IActionResult> Index()
        {
            var lunchAdvisorContext = _context.DishRating.Include(d => d.Dish).Include(d => d.User);
            return View(await lunchAdvisorContext.ToListAsync());
        }

        // GET: DishRatings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DishRating == null)
            {
                return NotFound();
            }

            var dishRating = await _context.DishRating
                .Include(d => d.Dish)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dishRating == null)
            {
                return NotFound();
            }

            return View(dishRating);
        }

        public async Task<IActionResult> Review(int? id)
        {
            ViewBag.Id = id;

            var Dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Dish == null)
            {
                return NotFound();
            }

            ViewBag.Name = Dish.Name;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(DishRating dishRating)
        {
            dishRating.date = DateTime.Now;
            dishRating.DishID = dishRating.ID;
            dishRating.ID = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            dishRating.UserID = claims.Value;


            ModelState.Clear();
            TryValidateModel(dishRating);
            if (ModelState.IsValid)
            {
                _context.Add(dishRating);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Dishes", new { id = dishRating.DishID });
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "ID", dishRating.DishID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", dishRating.UserID);
            return View(dishRating);
        }

        // GET: DishRatings/Create
        public IActionResult Create()
        {
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "ID");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: DishRatings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DishID,UserID,Name,Description,date,rating,imageURL")] DishRating dishRating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "ID", dishRating.DishID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", dishRating.UserID);
            return View(dishRating);
        }

        // GET: DishRatings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DishRating == null)
            {
                return NotFound();
            }

            var dishRating = await _context.DishRating.FindAsync(id);
            if (dishRating == null)
            {
                return NotFound();
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "ID", dishRating.DishID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", dishRating.UserID);
            return View(dishRating);
        }

        // POST: DishRatings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DishID,UserID,Name,Description,date,rating,imageURL")] DishRating dishRating)
        {
            if (id != dishRating.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishRatingExists(dishRating.ID))
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
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "ID", dishRating.DishID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", dishRating.UserID);
            return View(dishRating);
        }

        // GET: DishRatings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DishRating == null)
            {
                return NotFound();
            }

            var dishRating = await _context.DishRating
                .Include(d => d.Dish)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dishRating == null)
            {
                return NotFound();
            }

            return View(dishRating);
        }

        // POST: DishRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DishRating == null)
            {
                return Problem("Entity set 'LunchAdvisorContext.DishRating'  is null.");
            }
            var dishRating = await _context.DishRating.FindAsync(id);
            if (dishRating != null)
            {
                _context.DishRating.Remove(dishRating);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishRatingExists(int id)
        {
          return _context.DishRating.Any(e => e.ID == id);
        }
    }
}

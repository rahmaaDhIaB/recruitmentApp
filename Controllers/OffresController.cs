using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.Models.Auth;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;

namespace App.Controllers
{
    public class OffresController : Controller
    {
        private readonly AppDbContext _context;

        public OffresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Offres
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Offres.Include(o => o.User);
            return View(await appDbContext.ToListAsync());
        }

        //get my offers
        //public async Task<IActionResult> MyOffers()
        //{
        //    // Retrieve the user information from session
        //    var userJson = HttpContext.Session.GetString("CurrentUser");

        //    if (!string.IsNullOrEmpty(userJson))
        //    {
        //        var user = JsonConvert.DeserializeObject<User>(userJson);

        //        // Retrieve all offers for the current user
        //        var userOffers = await _context.Offres
        //            .Include(o => o.User)
        //            .Where(o => o.UserId == user.id)
        //            .ToListAsync();

        //        return View(userOffers);
        //    }

        //    // If user information is missing, redirect to login or handle accordingly
        //    var currentAction = ControllerContext.ActionDescriptor.RouteValues["action"];
        //    var currentController = ControllerContext.ActionDescriptor.RouteValues["controller"];

        //    return RedirectToAction(currentAction, currentController);

        //}

        public IActionResult MyOffers()
        {
            var userJson = HttpContext.Session.GetString("CurrentUser");

            if (!string.IsNullOrEmpty(userJson))
            {
                var user = JsonConvert.DeserializeObject<User>(userJson);

                // Assuming you have a DbSet<Offre> named _context.Offres in your DbContext
                var userOffers = _context.Offres
                    .Where(o => o.UserId == user.id)
                    .ToList();

                // Pass the list of offers to the view
                return View(userOffers);
            }

            var currentAction = ControllerContext.ActionDescriptor.RouteValues["action"];
              var currentController = ControllerContext.ActionDescriptor.RouteValues["controller"];

               return RedirectToAction(currentAction, currentController);

        }



        //GET: Offres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Offres == null)
            {
                return NotFound();
            }

            var offre = await _context.Offres
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offre == null)
            {
                return NotFound();
            }
            var userJson = HttpContext.Session.GetString("CurrentUser");
            var user = JsonConvert.DeserializeObject<User>(userJson);
            ViewBag.CurrentUser = user;

            return View(offre);
        }













        // GET: Offres/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "id", "name");
            return View();
        }

        // POST: Offres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Salaire,Competance,Responsabilite,Remuneration")] Offre offre)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userJson = HttpContext.Session.GetString("CurrentUser");

                    if (!string.IsNullOrEmpty(userJson))
                    {
                        var user = JsonConvert.DeserializeObject<User>(userJson);

                        offre.UserId = user.id;

                        _context.Add(offre);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(offre);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return View(offre);
            }
        }


        // GET: Offres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Offres == null)
            {
                return NotFound();
            }

            var offre = await _context.Offres.FindAsync(id);
            if (offre == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "id", "name", offre.UserId);
            return View(offre);
        }

        // POST: Offres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Salaire,Competance,Responsabilite,Remuneration,UserId")] Offre offre)
        {
            if (id != offre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OffreExists(offre.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "id", "name", offre.UserId);
            return View(offre);
        }

        // GET: Offres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Offres == null)
            {
                return NotFound();
            }

            var offre = await _context.Offres
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offre == null)
            {
                return NotFound();
            }

            return View(offre);
        }

        // POST: Offres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Offres == null)
            {
                return Problem("Entity set 'AppDbContext.Offres'  is null.");
            }
            var offre = await _context.Offres.FindAsync(id);
            if (offre != null)
            {
                _context.Offres.Remove(offre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OffreExists(int id)
        {
          return (_context.Offres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

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

namespace App.Controllers
{
    public class CandidaturesController : Controller
    {
        private readonly AppDbContext _context;

        public CandidaturesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Candidatures
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Candidatures.Include(c => c.Offre).Include(c => c.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Candidatures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Candidatures == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidatures
                .Include(c => c.Offre)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidature == null)
            {
                return NotFound();
            }

            return View(candidature);
        }

        //// GET: Candidatures/Create
        //public IActionResult Create()
        //{
        //    ViewData["OffreId"] = new SelectList(_context.Offres, "Id", "Id");
        //    ViewData["UserId"] = new SelectList(_context.Users, "id", "name");
        //    return View();
        //}

        //// POST: Candidatures/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,UserId,OffreId,Statut")] Candidature candidature)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(candidature);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["OffreId"] = new SelectList(_context.Offres, "Id", "Id", candidature.OffreId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "id", "name", candidature.UserId);
        //    return View(candidature);
        //}














        //// GET: Candidatures/Create
        //public IActionResult Create(int? userId, int? offreId)
        //{
        //    if (userId == null || offreId == null)
        //    {
        //        return NotFound();
        //    }

        //    // Ensure that the user is logged in
        //    var currentUserJson = HttpContext.Session.GetString("CurrentUser");
        //    if (string.IsNullOrEmpty(currentUserJson))
        //    {
        //        return RedirectToAction("Login", "Auth"); // Redirect to login if user not logged in
        //    }

        //    var user = JsonConvert.DeserializeObject<User>(currentUserJson);
        //    if (user == null || user.id != userId)
        //    {
        //        return NotFound(); // Ensure the user in session matches the provided userId
        //    }

        //    ViewData["UserId"] = userId;
        //    ViewData["OffreId"] = offreId;
        //    return View()

        //}

        //// POST: Candidatures/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("UserId,OffreId,Statut")] Candidature candidature)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        candidature.Statut = "en cours";

        //        _context.Add(candidature);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index", "Candidatures");
        //    }

        //    ViewData["OffreId"] = candidature.OffreId;
        //    ViewData["UserId"] = candidature.UserId;
        //    return View("index");
        //}








        // GET: Candidatures/Create
        public IActionResult Create(int? offreId)
        {
            if (offreId == null)
            {
                return NotFound();
            }

            // Ensure that the user is logged in
            var currentUserJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(currentUserJson))
            {
                return RedirectToAction("Login", "Auth"); // Redirect to login if user not logged in
            }

            var user = JsonConvert.DeserializeObject<User>(currentUserJson);
            if (user == null)
            {
                return NotFound(); // Ensure the user in session is valid
            }

            // Create a Candidature object with filled values
            var candidature = new candidature
            {
                UserId = user.id,
                OffreId = offreId.Value,
                Statut = "en cours"
            };

            // Add the candidature to the database
            _context.Add(candidature);
            _context.SaveChanges();

            // Redirect to the HomeC action in the Home/Auth controller
            return RedirectToAction("HomeC", "Auth");
        }










        public IActionResult MyCandidatures()
        {
            // Ensure that the user is logged in
            var currentUserJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(currentUserJson))
            {
                return RedirectToAction("Login", "Auth"); // Redirect to login if user not logged in
            }

            var user = JsonConvert.DeserializeObject<User>(currentUserJson);
            if (user == null)
            {
                return NotFound(); // Ensure the user in session is valid
            }

            // Retrieve all offers for the current user
            var userOffers = _context.Candidatures
                .Where(c => c.UserId == user.id)
                .Select(c => c.Offre)
                .ToList();

            // Pass the offers to the view
            return View(userOffers);
        }


        public IActionResult MyJobs()
        {
            // Ensure that the user is logged in
            var currentUserJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(currentUserJson))
            {
                return RedirectToAction("Login", "Auth"); // Redirect to login if the user is not logged in
            }

            var user = JsonConvert.DeserializeObject<User>(currentUserJson);
            if (user == null)
            {
                return NotFound(); // Ensure the user in session is valid
            }

            // Retrieve job requests for the current user
            var recruiterOffers = _context.Offres
                  .Where(offer => offer.UserId == user.id)
                    .Include(offer => offer.Candidatures)
                  .ThenInclude(candidature => candidature.User)
                   .ToList();


            // Pass the job requests to the view
            return View("MyJobs", recruiterOffers);
        }













        //public IActionResult ViewProfile(int userId)
        //{
        //    // Ensure that the user is logged in and is a recruiter
        //    var currentUserJson = HttpContext.Session.GetString("CurrentUser");
        //    if (string.IsNullOrEmpty(currentUserJson))
        //    {
        //        return RedirectToAction("Login", "Auth"); // Redirect to login if the user is not logged in
        //    }

        //    var recruiter = JsonConvert.DeserializeObject<User>(currentUserJson);
        //    if (recruiter == null)
        //    {
        //        return NotFound(); // Ensure the user in session is a valid recruiter
        //    }

        //    // Retrieve candidate information
        //    var candidates = _context.Users.ToList();

        //    if (candidates == null)
        //    {
        //        return NotFound(); // Candidate not found
        //    }

        //    return View(candidates);
        //}


        public IActionResult ViewProfile(int offerId)
        {
            // Ensure that the user is logged in and is a recruiter
            var currentUserJson = HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(currentUserJson))
            {
                return RedirectToAction("Login", "Auth"); // Redirect to login if the user is not logged in
            }

            var recruiter = JsonConvert.DeserializeObject<User>(currentUserJson);
            if (recruiter == null)
            {
                return NotFound(); // Ensure the user in session is a valid recruiter
            }

            // Retrieve candidate information for a specific offer
            var candidates = _context.Candidatures
                .Where(c => c.OffreId == offerId)
                .Select(c => c.User)
                .ToList();

            if (candidates == null)
            {
                return NotFound(); // Candidates not found
            }

            return View(candidates);
        }











        [HttpGet]
        public async Task<IActionResult> GestionCondidature(int candidatureId, string action)
        {
            var candidature = await _context.Candidatures
                .Include(c => c.User)  // Load related User data
                .FirstOrDefaultAsync(c => c.Id == candidatureId);

            if (candidature == null)
            {
                return NotFound();

            }

            // Check if the user ID matches the user ID in the candidature
            if (candidature.UserId != candidature.User?.id)
            {
                return BadRequest("Invalid user ID");
            }

            // Update the status based on the specified action (case-insensitive)
            // Update the status based on the specified action (case-insensitive)
            if (string.Equals(action, "accept", StringComparison.OrdinalIgnoreCase))
            {
                candidature.Statut = "accept";
            }
            else if (string.Equals(action, "refuse", StringComparison.OrdinalIgnoreCase))
            {
                candidature.Statut = "refuse";
            }
            else
            {
                return BadRequest($"Invalid action: {action}");
            }

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "User", new { id = candidature.UserId });
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues
                return BadRequest("Failed to update candidature status");
            }
        }













        // GET: Candidatures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Candidatures == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidatures.FindAsync(id);
            if (candidature == null)
            {
                return NotFound();
            }
            ViewData["OffreId"] = new SelectList(_context.Offres, "Id", "Id", candidature.OffreId);
            ViewData["UserId"] = new SelectList(_context.Users, "id", "name", candidature.UserId);
            return View(candidature);
        }

        // POST: Candidatures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,UserId,OffreId,Statut")] candidature candidature)
        {
            if (id != candidature.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidatureExists(candidature.Id))
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
            ViewData["OffreId"] = new SelectList(_context.Offres, "Id", "Id", candidature.OffreId);
            ViewData["UserId"] = new SelectList(_context.Users, "id", "name", candidature.UserId);
            return View(candidature);
        }

        // GET: Candidatures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Candidatures == null)
            {
                return NotFound();
            }

            var candidature = await _context.Candidatures
                .Include(c => c.Offre)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidature == null)
            {
                return NotFound();
            }

            return View(candidature);
        }

        // POST: Candidatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Candidatures == null)
            {
                return Problem("Entity set 'AppDbContext.Candidatures'  is null.");
            }
            var candidature = await _context.Candidatures.FindAsync(id);
            if (candidature != null)
            {
                _context.Candidatures.Remove(candidature);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidatureExists(int? id)
        {
          return (_context.Candidatures?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

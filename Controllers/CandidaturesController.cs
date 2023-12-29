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

            bool userHasCandidature = _context.Candidatures
        .Any(c => c.UserId == user.id && c.OffreId == offreId.Value);


            if (userHasCandidature)
            {
                // If the user already has a candidature, you may want to handle this case accordingly.
                // For example, display a message or redirect to a different action.
                return BadRequest($"Invalid action: deja postila  offre hedha  ");
            }

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

        public class ViewProfileViewModel
        {
            public List<User> Candidates { get; set; }
            public int OfferId { get; set; }
        }

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



            //var viewModel = new ViewProfileViewModel
            //{
            //    Candidates = candidates,
            //    OfferId = offerId
            //};

            ViewBag.OfferId = offerId;

            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;

            return View(candidates);
        }











        //[HttpGet]
        //public async Task<IActionResult> GestionCondidature(int candidatureId, int offerId, string action)
        //{
        //    var candidature = await _context.Candidatures
        //        .Include(c => c.User)  // Load related User data
        //        .FirstOrDefaultAsync(c => c.Id == candidatureId);

        //    if (candidature == null)
        //    {
        //        return NotFound();

        //    }

        //    // Check if the user ID matches the user ID in the candidature
        //    if (candidature.UserId != candidature.User?.id)
        //    {
        //        return BadRequest("Invalid user ID");
        //    }

        //    // Update the status based on the specified action (case-insensitive)
        //    // Update the status based on the specified action (case-insensitive)
        //    if (string.Equals(action, "accept", StringComparison.OrdinalIgnoreCase))
        //    {
        //        candidature.Statut = "accept";
        //    }
        //    else if (string.Equals(action, "refuse", StringComparison.OrdinalIgnoreCase))
        //    {
        //        candidature.Statut = "refuse";
        //    }
        //    else
        //    {
        //        return BadRequest($"Invalid action: {action}");
        //    }

        //    // Save changes to the database
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Details", "User", new { id = candidature.UserId });
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        // Handle concurrency issues
        //        return BadRequest("Failed to update candidature status");
        //    }
        //}








        //////[HttpGet]
        //////public IActionResult GestionCondidature(int candidatureId, int offerId, string action)
        //////{
        //////    // Check if candidatureId or offerId is invalid
        //////    if (candidatureId <= 0 || offerId <= 0)
        //////    {
        //////        return BadRequest($"Invalid candidatureId ({candidatureId}) or offerId ({offerId}).");
        //////    }

        //////    // Additional logic based on the action parameter
        //////    if (action == "GestionCondidature")
        //////    {
        //////        return Ok($"Accepted candidature with id {candidatureId} for offer {offerId}.");
        //////    }
        //////    else if (action == "Refuse")
        //////    {
        //////        return Ok($"Refused candidature with id {candidatureId} for offer {offerId}.");
        //////    }
        //////    else
        //////    {
        //////        // Invalid action parameter
        //////        return BadRequest($"Invalid action parameter: {action}");
        //////    }

        //////    // Your remaining logic

        //////    // Return a success result or redirect as needed
        //////    // This part is unreachable because you have already returned from the method
        //////    return Ok($"Valid candidatureId ({candidatureId}) or offerId ({offerId}).");
        //////}

        public IActionResult acceptCondidature(int userId, int offerId)
        {
            // Check if userId or offerId is invalid
            if (userId <= 0 || offerId <= 0)
            {
                return BadRequest($"Invalid userId ({userId}) or offerId ({offerId}).");
            }

            // Find the candidature in the database by userId and offerId
            var candidature = _context.Candidatures
                .FirstOrDefault(c => c.UserId == userId && c.OffreId == offerId);

            // Check if the candidature exists
            if (candidature == null)
            {
                return NotFound($"Candidature not found for userId {userId} and offerId {offerId}.");
            }

            // Update the Statut to "accepter"
            candidature.Statut = "accepter";

            // Save changes to the database
            _context.SaveChanges();

            // Return a success message
            //return Ok($"Candidature for userId {userId} and offerId {offerId} has been accepted.");
            TempData["SuccessMessage"] = "accepted successfully";

            // Redirect to the ViewProfile action
            return RedirectToAction("ViewProfile", new { offerId });
        }


        public IActionResult refuseCondidature(int userId, int offerId)
        {
            // Check if userId or offerId is invalid
            if (userId <= 0 || offerId <= 0)
            {
                return BadRequest($"Invalid userId ({userId}) or offerId ({offerId}).");
            }

            // Find the candidature in the database by userId and offerId
            var candidature = _context.Candidatures
                .FirstOrDefault(c => c.UserId == userId && c.OffreId == offerId);

            // Check if the candidature exists
            if (candidature == null)
            {
                return NotFound($"Candidature not found for userId {userId} and offerId {offerId}.");
            }

            // Update the Statut to "accepter"
            candidature.Statut = "refuser";

            // Save changes to the database
            _context.SaveChanges();

            // Return a success message
            //return Ok($"Candidature for userId {userId} and offerId {offerId} has been accepted.");
            TempData["SuccessMessage"] = "deleted successfully";

            // Redirect to the ViewProfile action
            return RedirectToAction("ViewProfile", new { offerId });
        }

        public IActionResult MoreDetails(int offerId)
        {
            // Ensure that the user is logged in
            var currentUserJson = HttpContext.Session.GetString("CurrentUser");

            if (string.IsNullOrEmpty(currentUserJson))
            {
                return RedirectToAction("Login", "Auth"); // Redirect to login if user not logged in
            }

            var user = JsonConvert.DeserializeObject<User>(currentUserJson);

            // Retrieve the details for the specific offer and user
            var candidatureDetails = _context.Candidatures
                .Where(c => c.OffreId == offerId && c.UserId == user.id)
                .FirstOrDefault();

            if (candidatureDetails == null)
            {
                return NotFound(); // Candidature details not found
            }

            var offreDetails = _context.Offres
         .Where(o => o.Id == offerId)
         .FirstOrDefault();


            ViewBag.OfferDetails = offreDetails;


            return View(candidatureDetails);
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

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
using Microsoft.AspNetCore.Http;

namespace App.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        //GET: Auth
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.Users'  is null.");
        }

        // GET: Auth/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        public async Task<IActionResult> myprofile(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        public IActionResult signin()
        {
            return View();
        }








        [HttpPost]
        public IActionResult Login(string email, string password)
        {

            var user = _context.Users.FirstOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(user));

                if (user.role == "Candidate")
                {
                    
                    return RedirectToAction(nameof(HomeC));

                }
                else if (user.role == "Recruiter")
                {
                    return RedirectToAction(nameof(HomeR));
                }
               
            }

            return RedirectToAction(nameof(register)); // Replace "Auth" with the actual name of your controller handling the Register action
       
   //      return View(user);
        }







        public IActionResult HomeR()
        {
            // Retrieve user information from session
            var userJson = HttpContext.Session.GetString("CurrentUser");

            if (!string.IsNullOrEmpty(userJson))
            {
                var user = JsonConvert.DeserializeObject<User>(userJson);
                // Now 'user' contains information about the currently logged-in user

                return View();
            }

            return RedirectToAction(nameof(Login));
        }


        public IActionResult HomeC()
        {
            // Retrieve user information from session
            var userJson = HttpContext.Session.GetString("CurrentUser");

            if (!string.IsNullOrEmpty(userJson))
            {
                var user = JsonConvert.DeserializeObject<User>(userJson);
                // Now 'user' contains information about the currently logged-in user

                // Pass user information to the view
                ViewBag.UserName = user.name;

                
                ViewBag.CurrentUser = user.id;


                return RedirectToAction("index", "offres", new { id = user.id });
            }

            return RedirectToAction(nameof(Login));
        }





        public IActionResult Logout()
        {
            // Clear user information from session
            HttpContext.Session.Remove("CurrentUser");

            // Redirect to the login page or any other desired page
            return RedirectToAction(nameof(signin));
        

        }


        // GET: Auth/Create
        public IActionResult register()
        {
            return View();
        }

        // POST: Auth/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> register([Bind("id,name,email,password,number,address,age,description,role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(signin));
            }
            return View(user);

        }

        // GET: Auth/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Auth/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,email,password,number,address,age,description,role")] User user)
        {
            if (id != user.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.id))
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
            return View(user);
        }

        // GET: Auth/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Auth/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'AppDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
